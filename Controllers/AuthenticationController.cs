using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using dogsitting_backend.Domain;
using Google.Protobuf.WellKnownTypes;

namespace dogsitting_backend.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ClaimsPrincipal claimsPrincipal;

        public AuthenticationController(IHttpContextAccessor httpContextAccessor)
        {
            this.claimsPrincipal = httpContextAccessor.HttpContext.User;

        }



        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(FacebookLoginCallback))
            };

            return Challenge(properties, "Facebook");
        }

        [AllowAnonymous]
        [HttpGet("signin-facebook")]
        public async Task<IActionResult> FacebookLoginCallback()
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
