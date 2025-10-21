using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using dogsitting_backend.Domain;
using Google.Protobuf.WellKnownTypes;
using dogsitting_backend.ApplicationServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using dogsitting_backend.Domain.auth;
using ZstdSharp;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using NuGet.Protocol;
using System.Text.Json;
using Humanizer;
using System.Threading.Tasks;



namespace dogsitting_backend.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private AuthService _authService;
        private AuthUser _authUser;
        private IHttpContextAccessor httpContextAccessor;
        private IConfiguration _configuration;
        private UserManager<AuthUser> _userManager;
        private string frontendUrl = "http://localhost:4000";

        public AuthenticationController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, AuthService authService, UserManager<AuthUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _configuration = configuration;
            this.frontendUrl = _configuration.GetValue<string>("FrontendUrl");
            this._userManager = userManager;
        }

        [HttpGet("authuser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoggedInUser()
        {
            ClaimsPrincipal claimsPrincipal = httpContextAccessor.HttpContext.User;
            this._authUser = await _userManager.GetUserAsync(claimsPrincipal);
            if (this._authUser == null)
            {
                throw new Exception("not authentified");
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            string json = JsonConvert.SerializeObject(this._authUser.ApplicationUser, settings);
            return Ok(json);
        }


        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://localhost:5188/facebook-callback"
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }


        [HttpGet("facebook-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookLoginCallback()
        {
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            Console.WriteLine($"=========FacebookLoginCallback=========");
            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction(nameof(Accessdenied));
            }
            ClaimsPrincipal claimsPrincipal = authenticateResult.Principal;
            ClaimsIdentity? claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            string? serializedAccessToken = HttpContext.Session.GetString("facebook_accesstoken");
            string? serializedTokenResponse = HttpContext.Session.GetString("facebook_tokenResponse");
            OAuthTokenResponse tokenResponse = RetrieveTokenResponseFromString(serializedTokenResponse);
            await _authService.AuthenticateWithExternalProvider(claimsIdentity, tokenResponse);


            Response.Cookies.Append("ds_auth_token", serializedAccessToken, new CookieOptions
            {
                HttpOnly = true, // important!
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = "localhost"
            });

            return RedirectToAction(nameof(FacebookSuccess));
        }

        [AllowAnonymous]
        [HttpGet("home")]
        public void FacebookSuccess()
        {
            HttpContext.Response.Redirect($"{this.frontendUrl}/api/auth/facebook-success");
        }

        private OAuthTokenResponse RetrieveTokenResponseFromString(string serializedAccessToken)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(serializedAccessToken);
            OAuthTokenResponse tokenResponse = OAuthTokenResponse.Success(jsonDocument);

            tokenResponse.AccessToken = jsonDocument.RootElement.GetProperty("AccessToken").GetString();
            tokenResponse.TokenType = jsonDocument.RootElement.GetProperty("TokenType").GetString();
            tokenResponse.RefreshToken = jsonDocument.RootElement.GetProperty("RefreshToken").GetString();
            tokenResponse.ExpiresIn = jsonDocument.RootElement.GetProperty("ExpiresIn").GetString();
            return tokenResponse;
        }

        [AllowAnonymous]
        [HttpGet("accessdenied")]
        public void Accessdenied()
        {
            HttpContext.Response.Redirect($"{this.frontendUrl}/accessdenied");
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("ConsumeExternalProviderAccessToken")]
        public ChallengeResult ConsumeExternalProviderAccessToken([FromBody] ConsumeAccessToken accessTokenResponse)
        {
            string returnUrl = "";
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : "";
            AuthenticationProperties authenticationProperties = new() { RedirectUri = returnUrl, };
            ChallengeResult challengeResult = new(accessTokenResponse.providerName, authenticationProperties);
            return challengeResult;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("Login/{provider}")]
        public IActionResult LoginExternal([FromRoute] string provider)
        {
            string returnUrl = "";
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            AuthenticationProperties authenticationProperties = new() { RedirectUri = returnUrl };
            return new ChallengeResult(provider, authenticationProperties);
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("LogOff")]
        public async Task<IActionResult> LogOff()
        {
            await _authService.Signout();

            HttpContext.Response.Redirect($"{this.frontendUrl}/home");
            return Ok("success");
        }

    }
}
