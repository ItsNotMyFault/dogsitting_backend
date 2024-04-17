using dogsitting_backend.ApplicationServices;

namespace dogsitting_backend.Startup
{
    public static class RegisterAppServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            
            services.AddTransient<TeamService>();
            services.AddTransient<ReservationService>();
            services.AddTransient<UserService>();

        }
    }
}
