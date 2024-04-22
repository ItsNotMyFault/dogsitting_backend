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

namespace dogsitting_backend.Controllers
{

    public class AuthenticationController : ControllerBase
    {
        private readonly ClaimsPrincipal claimsPrincipal;
        private UserService userService;
        private AuthUser authUser;

        public AuthenticationController(IHttpContextAccessor httpContextAccessor, UserService userService, UserManager<AuthUser> userManager)
        {
            this.claimsPrincipal = httpContextAccessor.HttpContext.User;
            authUser = userManager.GetUserAsync(claimsPrincipal).Result;
            this.userService = userService;

        }

        [HttpGet("user")]
        [AllowAnonymous]
        public IActionResult GetLoggedInUser()
        {
            if(this.authUser == null)
            {
                throw new Exception("not authentified");
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            string json = JsonConvert.SerializeObject(this.authUser.ApplicationUser, settings);
            return Ok(json);
        }


        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://localhost:4000"
                //RedirectUri = "https://localhost:5188"
                //RedirectUri = Url.Action(nameof(FacebookLoginCallback))
            };
            return Challenge(properties, "Facebook");
        }

        [AllowAnonymous]
        [HttpGet("signin-facebook")]
        public async Task<IActionResult> FacebookLoginCallback(Object context)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Facebook");

            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction(nameof(Accessdenied));
            }

            // Authentication succeeded, perform additional logic like storing user details or issuing JWT token.

            return Ok(404);
        }


        [AllowAnonymous]
        [HttpGet("accessdenied")]
        public IActionResult Accessdenied()
        {
            this.HttpContext.Response.Redirect("https://localhost:4000/accessdenied");
            return Ok("fail");
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
        public IActionResult LogOff()
        {
            return Ok("Success");
        }

    }
}
