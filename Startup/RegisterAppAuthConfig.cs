using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Infrastructure;
using dogsitting_backend.Infrastructure.store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using umbrella.portal.CustomProvider;

namespace dogsitting_backend.Startup
{
    public static class RegisterAppAuthConfig
    {
        private readonly static string appid = "440736849687856";
        private readonly static string appsecret = "71dc26b3c7ce1bf1328abce52aa9aca8";

        public static void AddOAuthServices(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            })
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = appid;
                facebookOptions.AppSecret = appsecret;
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                //facebookOptions.SaveTokens = true;
                facebookOptions.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
                {
                    OnAccessDenied = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnCreatingTicket = async context =>
                    {
                        OAuthTokenResponse tokenResponse = context.TokenResponse;
                        string serializedAccessToken = JsonConvert.SerializeObject(tokenResponse);
                        context.HttpContext.Session.SetString("facebook_accesstoken", serializedAccessToken);
                        ClaimsIdentity claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        UserService authenticationService = context.HttpContext.RequestServices.GetRequiredService(typeof(UserService)) as UserService;

                        bool isSuccess = await authenticationService.AuthenticateWithExternalProvider(claimsIdentity, tokenResponse);
                        await Task.CompletedTask;
                    },
                    OnRedirectToAuthorizationEndpoint = async context =>
                    {
                        context.HttpContext.Response.Redirect(context.RedirectUri + "&display=popup&pip");
                        await Task.CompletedTask;
                    },
                    OnRemoteFailure = async context =>
                    {
                        var errorMessage = context.Failure.Message;
                        await Task.CompletedTask;
                    },
                    OnTicketReceived = async context =>
                    {
                        await Task.CompletedTask;
                    },
                };
            });
            services.AddIdentity<AuthUser, ApplicationRole>()
                .AddUserStore<CustomUserStore>()
                .AddRoleStore<CustomRoleStore>();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
                opts.Password.RequireNonAlphanumeric = false;
            });



        }
        //.AddCookie(IdentityConstants.ApplicationScheme)

        public static void AddJWTBearer(this IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddBearerToken("facebook", options =>
            {
                options.Events = new BearerTokenEvents()
                {
                    
                    OnMessageReceived = async context =>
                    {
                        await Task.CompletedTask;
                    },
                };
            });
        }
    }
}
