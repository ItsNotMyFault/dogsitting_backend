using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain.repositories;
using dogsitting_backend.Infrastructure;
using Google.Protobuf.WellKnownTypes;

namespace dogsitting_backend.Startup
{
    public static class RegisterAppRepositories
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            
            services.AddTransient<TeamSQLRepository>();
            services.AddTransient<CalendarSQLRepository>();
            services.AddTransient<IReservationRepository, ReservationSQLRepository>();
            services.AddTransient<MediaSQLRepository>();
            services.AddTransient<UserSQLRepository>();
            services.AddTransient<RoleSQLRepository>();
            services.AddTransient<AnimalSQLRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
