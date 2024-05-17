using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Infrastructure;
using dogsitting_backend.Startup;
using dogsitting_backend.Startup.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//}); 


IConfigurationRoot Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
TimeZoneInfo localTimeZone = TimeZoneInfo.Utc;
//CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

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
builder.Services.AddHttpContextAccessor();

services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy",
        builder =>
        {
            builder.WithOrigins(new string[] {
                "http://localhost:4000",
                "https://localhost:4000",
                "http://localhost:5188",
                "https://localhost:5188",
                "https://www.facebook.com"
            })
            //.SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod();
            //.AllowCredentials();
        });
});

builder.Services.AddDbContext<DogsittingDBContext>(options =>
{
    try
    {
        string connetionString = builder.Configuration.GetSection("ConnectionString:Dev:dogsitting").Value;
        options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
    }
    catch(Exception err)
    {
        var ex = err;
    }
    
}
);

builder.Services.AddMvc();
builder.Logging.AddConsole();


services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

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

app.UseDeveloperExceptionPage();
app.UseCors("VueCorsPolicy");
app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
