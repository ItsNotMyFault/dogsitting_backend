using dogsitting_backend.Domain.auth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;

namespace dogsitting_backend.ApplicationServices
{
    public class AuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly ClaimsPrincipal claimsPrincipal;
        public AuthService(
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.claimsPrincipal = httpContextAccessor.HttpContext.User;
            this._userManager = userManager;
            this._signInManager = signInManager;

        }


        public async Task<AuthUser> GetCurrentUserAsync()
        {
            AuthUser applicationUser = await _userManager.GetUserAsync(this.claimsPrincipal);
            return applicationUser;
        }

        public async Task Signout()
        {
            await this._signInManager.SignOutAsync();
        }


        public async Task<bool> AuthenticateWithExternalProvider(ClaimsIdentity claimsIdentity, OAuthTokenResponse oAuthTokenResponse)
        {
            if (claimsIdentity == null && oAuthTokenResponse == null)
            {
                throw new Exception("Missing oauth token and claims");
            }

            Debug.WriteLine("this.claimsPrincipal----------------");
            Debug.WriteLine(this.claimsPrincipal);
            Debug.WriteLine(claimsIdentity);
            if (this.claimsPrincipal != null)
            {
                AuthUser user = await this._userManager.GetUserAsync(this.claimsPrincipal);
                if (user != null)
                {
                    return true;
                }
            }


            string loginProvider = claimsIdentity.AuthenticationType;
            string providerkey = claimsIdentity.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value;
            string accesstoken = oAuthTokenResponse.AccessToken;
            string emailAddress = claimsIdentity.FindFirst(x => x.Type.Contains("emailaddress")).Value;

            string providerDisplayName = loginProvider.ToLower();
            string identityName = claimsIdentity.Name;//user display name

            //AuthenticationUser authenticationUser = new()
            //{
            //    Email = emailAddress,
            //    NormalizedUserName = name,
            //    UserName = $"{givenname} {surname}",
            //    ApplicationUser = new ApplicationUser() { Email = emailAddress, FirstName = givenname, LastName = surname }
            //};

            //user exists => check UserLogin with matching provider exists
            // => if yes => use signin manager to authenticate (should add entry in usertoken).
            // => if no => AddloginProvider
            AuthUser authenticationUser = null;
            try
            {
                authenticationUser = await _userManager.FindByLoginAsync(loginProvider, providerkey);
            }
            catch (Exception e)
            {
                var err = e;
            }

            if (authenticationUser != null)
            {
                await this._signInManager.SignInAsync(authenticationUser, false);
            }
            else
            {
                UserLoginInfo userLoginInfo = new(loginProvider, providerkey, providerDisplayName);
                AuthUser? appUser = null;
                try
                {
                    appUser = await this._userManager.FindByEmailAsync(emailAddress);

                }
                catch (Exception e)
                {
                    var err = e;
                }
                if (appUser != null)
                {

                    await this._userManager.AddLoginAsync(appUser, userLoginInfo);
                    await this._signInManager.SignInAsync(appUser, false);
                }
                else
                {
                    AuthUser createdUser = this.CreateApplicationUser(claimsIdentity);
                    await this._userManager.AddLoginAsync(createdUser, userLoginInfo);
                    await this._signInManager.SignInAsync(createdUser, false);
                }

            }

            return true;

        }

        private AuthUser CreateApplicationUser(ClaimsIdentity claimsIdentity)
        {
            try
            {
                List<Claim> claims = claimsIdentity.Claims.ToList();
                string prefixClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
                var nameidentifier = claimsIdentity.FindFirst(x => x.Type.Contains("nameidentifier")).Value;
                var emailAddress = claimsIdentity.FindFirst(x => x.Type.Contains($"{prefixClaimType}emailaddress")).Value;
                var name = claimsIdentity.FindFirst(x => x.Type.Contains($"{prefixClaimType}name")).Value;
                var givenname = claimsIdentity.FindFirst(x => x.Type.Contains("givenname")).Value;
                var surname = claimsIdentity.FindFirst(x => x.Type.Contains("surname")).Value;

                AuthUser newUser = new()
                {
                    Email = emailAddress,
                    NormalizedEmail = emailAddress.ToUpper(),
                    UserName = $"{givenname}.{surname}",
                    NormalizedUserName = $"{givenname}.{surname}".ToUpper(),
                    ApplicationUser = new Domain.ApplicationUser(givenname, surname)
                    {
                        Email = emailAddress,
                    }
                };
                IdentityResult creationResult = this._userManager.CreateAsync(newUser).Result;

                if (creationResult.Succeeded)
                {
                    //add role to user
                }
                else
                {
                    throw new Exception(creationResult.Errors.ToString());
                }

                if (newUser == null)
                {
                    throw new Exception("User is null");
                }
                return newUser;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while creating user (external provider): {e.Message}");
            }

        }

    }
}
