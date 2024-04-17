using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Infrastructure;
using dogsitting_backend.Startup;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
builder.Services.AddControllers();


IConfigurationRoot Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;

var isdev = Environment.IsDevelopment();
services.AddSingleton<IConfiguration>(Configuration);

builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.Name = "Dogsitting_Session_Cookie";
});

services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/accessdenied";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.Name = "Dogsitting_ConfAppCookie";
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.Events = new CookieAuthenticationEvents()
    {
        OnSignedIn = async context =>
        {
            await Task.CompletedTask;
        },
        OnSigningIn = async context =>
        {
            ClaimsIdentity claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            await Task.CompletedTask;
        },
    };
});


builder.Services.AddOAuthServices();
//builder.Services.AddJWTBearer();
builder.Services.AddHttpContextAccessor();



services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder =>
        {
            builder.WithOrigins(new string[] {
                                "http://localhost:5188",
            }).SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddDbContext<DogsittingDBContext>(options =>
{
    string connetionString = builder.Configuration.GetSection("ConnectionString:Dev:dogsitting").Value;
    options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
}
);

builder.Services.AddMvc();
builder.Logging.AddConsole();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});

//builder.Services.AddHsts(options =>
//{
//    options.Preload = true;
//    options.IncludeSubDomains = true;
//    options.MaxAge = TimeSpan.FromDays(60);
//});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    IServiceProvider scopeServices = scope.ServiceProvider;

    DogsittingDBContext context = scopeServices.GetRequiredService<DogsittingDBContext>();
    context.Database.EnsureCreated();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseSession();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
