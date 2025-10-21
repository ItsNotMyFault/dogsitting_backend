using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Controllers;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Infrastructure;
using dogsitting_backend.Infrastructure.store;
using Google.Protobuf.WellKnownTypes;
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
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.Json;
using System.Text.RegularExpressions;
using umbrella.portal.CustomProvider;
using static System.Net.WebRequestMethods;

namespace dogsitting_backend.Startup
{
    public static class RegisterAppAuthConfig
    {
        //TODO set those in appsettings.json
        private readonly static string appid = "440736849687856";
        private readonly static string appsecret = "71dc26b3c7ce1bf1328abce52aa9aca8";

        static string ReplaceRedirectUri(string url, string newRedirectUri)
        {
            string pattern = @"(redirect_uri=)([^&]*)";
            string replacement = $"$1{newRedirectUri}";

            string result = Regex.Replace(url, pattern, replacement);

            return result;
        }

        public static void AddOAuthServices(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = appid;
                facebookOptions.AppSecret = appsecret;
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                //specifying the callback prevent the redirect to my controller method for some reason. Which causes a OnRemoteFailure event.
                //facebookOptions.CallbackPath = "/facebook-callback";
                facebookOptions.AuthorizationEndpoint = "https://www.facebook.com/v14.0/dialog/oauth";
                facebookOptions.Events = new OAuthEvents()
                {
                    OnAccessDenied = async context =>
                    {
                        Console.WriteLine($"=========OnAccessDenied=========");
                        await Task.CompletedTask;
                    },
                    OnCreatingTicket = async context =>
                    {
                        Console.WriteLine($"=========OnCreatingTicket=========");
                        var accessToken = context.AccessToken; // Use direct access token
                        OAuthTokenResponse tokenResponse = context.TokenResponse;

                        context.HttpContext.Session.SetString("facebook_accesstoken", JsonConvert.SerializeObject(accessToken));
                        context.HttpContext.Session.SetString("facebook_tokenResponse", JsonConvert.SerializeObject(tokenResponse));
                        var getToken = context.HttpContext.Session.Get("facebook_accesstoken");
                        await Task.CompletedTask;
                    },
                    OnTicketReceived = async context =>
                    {
                        Console.WriteLine($"=========OnTicketReceived=========");
                        await Task.CompletedTask;
                    },
                    OnRedirectToAuthorizationEndpoint = async context =>
                    {
                        Console.WriteLine($"=========OnRedirectToAuthorizationEndpoint=========");
                        context.HttpContext.Response.Redirect(context.RedirectUri);
                        await Task.CompletedTask;
                    },
                    OnRemoteFailure = async context =>
                    {
                        Console.WriteLine($"=========OnRemoteFailure=========");
                        var errorMessage = context.Failure.Message;
                        context.HttpContext.Response.Redirect("https://localhost:4000/accessdenied");
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppRolePolicy.PolicyAdmin.ToString(), policy => policy.RequireRole("ADMIN", "SUPERADMIN", "SuperAdmin"));
                options.AddPolicy(AppRolePolicy.PolicyClient.ToString(), policy => policy.RequireRole("ADMIN", "CLIENT", "SUPERADMIN", "SuperAdmin"));
                options.AddPolicy(AppRolePolicy.PolicySuperAdmin.ToString(), policy => policy.RequireRole("ADMIN", "SUPERADMIN", "SuperAdmin"));
            });



        }
    }
}
