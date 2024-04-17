using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Infrastructure;
using Google.Protobuf.WellKnownTypes;

namespace dogsitting_backend.Startup
{
    public static class RegisterAppRepositories
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            
            services.AddTransient<TeamSQLRepository>();
            services.AddTransient<ReservationSQLRepository>();
            services.AddTransient<UserSQLRepository>();
            services.AddTransient<RoleSQLRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
