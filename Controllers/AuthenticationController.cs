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



namespace dogsitting_backend.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ClaimsPrincipal claimsPrincipal;
        private AuthService _authService;
        private AuthUser _authUser;
        private IHttpContextAccessor httpContextAccessor;

        public AuthenticationController(IHttpContextAccessor httpContextAccessor, AuthService authService, UserManager<AuthUser> userManager)
        {
            Console.WriteLine($"=========AuthenticationController=========");
            this.httpContextAccessor = httpContextAccessor;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            Console.WriteLine(httpContextAccessor);
            Console.WriteLine(httpContextAccessor.HttpContext.User);
            _authUser = userManager.GetUserAsync(claimsPrincipal).Result;
            Console.WriteLine("_authUser");
            Console.WriteLine(_authUser);
            _authService = authService;

        }

        [HttpGet("authuser")]
        [AllowAnonymous]
        public IActionResult GetLoggedInUser()
        {
            if (_authUser == null)
            {
                throw new Exception("not authentified");
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            string json = JsonConvert.SerializeObject(_authUser.ApplicationUser, settings);
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
            Console.WriteLine(authenticateResult.Ticket);
            Console.WriteLine(authenticateResult.Succeeded);
            Console.WriteLine(HttpContext.Session);
            Console.WriteLine(authenticateResult.Principal);
            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction(nameof(Accessdenied));
            }
            ClaimsPrincipal claimsPrincipal = authenticateResult.Principal;
            ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            string serializedAccessToken = HttpContext.Session.GetString("facebook_accesstoken");
            string serializedTokenResponse = HttpContext.Session.GetString("facebook_tokenResponse");
            Console.WriteLine("--------------------------");
            Console.WriteLine(serializedAccessToken);
            Console.WriteLine("--------------------------");
            Console.WriteLine(serializedTokenResponse);
            OAuthTokenResponse tokenResponse = RetrieveTokenResponseFromString(serializedTokenResponse);
            await _authService.AuthenticateWithExternalProvider(claimsIdentity, tokenResponse);

            return RedirectToAction(nameof(Home));
        }

        private OAuthTokenResponse RetrieveTokenResponseFromString(string serializedAccessToken)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(serializedAccessToken);
            Console.WriteLine(jsonDocument);
            OAuthTokenResponse tokenResponse = OAuthTokenResponse.Success(jsonDocument);
            Console.WriteLine(tokenResponse);

            // Extract values from JsonDocument
            string accessToken = jsonDocument.RootElement.GetProperty("AccessToken").GetString();
            Console.WriteLine("BROOOOOOOOOOOOO");
            Console.WriteLine(accessToken);
            string tokenType = jsonDocument.RootElement.GetProperty("TokenType").GetString();
            string refreshToken = jsonDocument.RootElement.GetProperty("RefreshToken").GetString();
            string expiresIn = jsonDocument.RootElement.GetProperty("ExpiresIn").GetString();
            tokenResponse.AccessToken = accessToken;
            tokenResponse.TokenType = tokenType;
            tokenResponse.ExpiresIn = expiresIn;
            tokenResponse.RefreshToken = refreshToken;
            Console.WriteLine(tokenResponse);
            return tokenResponse;
        }

        [AllowAnonymous]
        [HttpGet("accessdenied")]
        public void Accessdenied()
        {
            HttpContext.Response.Redirect("https://localhost:4000/accessdenied");
        }

        [AllowAnonymous]
        [HttpGet("home")]
        public void Home()
        {
            HttpContext.Response.Redirect("https://localhost:4000/");
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

            HttpContext.Response.Redirect("https://localhost:4000/home");
            return Ok("success");
        }

    }
}
