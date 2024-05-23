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
            this.httpContextAccessor = httpContextAccessor;
            this.claimsPrincipal = httpContextAccessor.HttpContext.User;
            _authUser = userManager.GetUserAsync(claimsPrincipal).Result;
            this._authService = authService;

        }

        [HttpGet("authuser")]
        [AllowAnonymous]
        public IActionResult GetLoggedInUser()
        {
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


            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction(nameof(Accessdenied));
            }
            ClaimsPrincipal claimsPrincipal = authenticateResult.Principal;
            ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            string serializedAccessToken = HttpContext.Session.GetString("facebook_accesstoken");
            OAuthTokenResponse tokenResponse = this.RetrieveTokenResponseFromString(serializedAccessToken);
            this._authService.AuthenticateWithExternalProvider(claimsIdentity, tokenResponse);

            return RedirectToAction(nameof(Home));
        }

        private OAuthTokenResponse RetrieveTokenResponseFromString(string serializedAccessToken)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(serializedAccessToken);
            OAuthTokenResponse tokenResponse = OAuthTokenResponse.Success(jsonDocument);

            // Extract values from JsonDocument
            string accessToken = jsonDocument.RootElement.GetProperty("AccessToken").GetString();
            string tokenType = jsonDocument.RootElement.GetProperty("TokenType").GetString();
            string refreshToken = jsonDocument.RootElement.GetProperty("RefreshToken").GetString();
            string expiresIn = jsonDocument.RootElement.GetProperty("ExpiresIn").GetString();
            tokenResponse.AccessToken = accessToken;
            tokenResponse.TokenType = tokenType;
            tokenResponse.ExpiresIn = expiresIn;
            tokenResponse.RefreshToken = refreshToken;
            return tokenResponse;
        }

        [AllowAnonymous]
        [HttpGet("accessdenied")]
        public void Accessdenied()
        {
            this.HttpContext.Response.Redirect("https://localhost:4000/accessdenied");
        }

        [AllowAnonymous]
        [HttpGet("home")]
        public void Home()
        {
            this.HttpContext.Response.Redirect("https://localhost:4000/home");
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
            await this._authService.Signout();
            
            this.HttpContext.Response.Redirect("https://localhost:4000/home");
            return Ok("success");
        }

    }
}
