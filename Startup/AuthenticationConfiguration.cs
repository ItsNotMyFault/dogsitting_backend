using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace dogsitting_backend.Startup
{
    public static class AuthenticationConfiguration
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
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = appid;
                facebookOptions.AppSecret = appsecret;
                facebookOptions.
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                facebookOptions.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
                {
                    OnAccessDenied = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnCreatingTicket = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnRedirectToAuthorizationEndpoint = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnRemoteFailure = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnTicketReceived = async context =>
                    {
                        await Task.CompletedTask;
                    },
                };
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
