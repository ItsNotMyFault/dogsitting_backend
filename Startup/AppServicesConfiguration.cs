using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Infrastructure;
using Google.Protobuf.WellKnownTypes;

namespace dogsitting_backend.Startup
{
    public static class AppServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            
            services.AddTransient<TeamService>();
            services.AddTransient<ReservationService>();
            services.AddTransient<TeamSQLRepository>();
            services.AddTransient<ReservationSQLRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
