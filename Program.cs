using dogsitting_backend.Infrastructure;
using dogsitting_backend.Startup;
using dogsitting_backend.Startup.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
builder.Services.AddControllers();

IConfigurationRoot Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;
//does this really work? UTC setup
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
TimeZoneInfo localTimeZone = TimeZoneInfo.Utc;

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
            await Task.CompletedTask;
        },
        
    };
});


builder.Services.AddOAuthServices();
builder.Services.AddHttpContextAccessor();


services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4000",
        builder =>
        {
            builder.WithOrigins("https://localhost:4000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Allow credentials if cookies or other credentials are needed
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

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    IServiceProvider scopeServices = scope.ServiceProvider;

    DogsittingDBContext context = scopeServices.GetRequiredService<DogsittingDBContext>();
    context.Database.EnsureCreated();
}

app.UseDeveloperExceptionPage();

app.UseCors("AllowLocalhost4000");

app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
