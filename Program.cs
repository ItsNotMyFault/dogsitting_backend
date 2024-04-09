using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


IConfigurationRoot Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;

var isdev = Environment.IsDevelopment();

builder.Services.AddSingleton<IConfiguration>(Configuration);
builder.Services.AddTransient<TeamService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<TeamSQLRepository>();
builder.Services.AddTransient<ReservationSQLRepository>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddDbContext<DogsittingDBContext>(options =>
{
    var connetionString = builder.Configuration.GetSection("ConnectionString:Dev:dogsitting").Value;
    //options.UseMySQL();
    options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
}
);

builder.Services.AddMvc();
builder.Logging.AddConsole();


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DogsittingDBContext>();
    context.Database.EnsureCreated();
    // DbInitializer.Initialize(context);
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
