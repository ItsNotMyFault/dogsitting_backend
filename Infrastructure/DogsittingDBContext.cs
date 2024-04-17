using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.Infrastructure
{
    public class DogsittingDBContext : DbContext
    {
        public DogsittingDBContext(DbContextOptions<DogsittingDBContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
          .HasOne(r => r.Calendar) // One-to-one relationship with Calendar
          .WithMany(c => c.Reservations)
          .HasForeignKey(r => r.CalendarId)
          .IsRequired();

            modelBuilder.Entity<Calendar>().ToTable("Calendars").HasOne(x => x.Team).WithOne(x => x.Calendar);
            modelBuilder.Entity<Calendar>().ToTable("Calendars").HasMany(t => t.Reservations).WithOne(r => r.Calendar);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasMany(t => t.Reservations).WithOne(r => r.Client);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasMany(t => t.UserLogins).WithOne(r => r.User);
            //TODO: Fix la relation Reservation => client et la relation Calendrier => team


            modelBuilder.Entity<Team>()
                .HasMany(t => t.Admins)
                .WithMany(t => t.Teams)
            .UsingEntity("TeamUsers",
                right => right.HasOne(typeof(ApplicationUser)).WithMany().HasForeignKey("userId").HasPrincipalKey(nameof(ApplicationUser.Id)),
                left => left.HasOne(typeof(Team)).WithMany().HasForeignKey("teamId").HasPrincipalKey(nameof(Team.Id)),
                join =>
                {
                    join.ToTable("TeamUsers"); // Specify the name of the join table
                    join.HasKey("userId", "teamId"); // Specify the composite primary key
                }
            );

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(t => t.Roles)
            .WithMany(t => t.Users)
        .UsingEntity("UserRoles",
        right => right.HasOne(typeof(ApplicationRole)).WithMany().HasForeignKey("roleId").HasPrincipalKey(nameof(ApplicationRole.Id)),
        left=> left.HasOne(typeof(ApplicationUser)).WithMany().HasForeignKey("userId").HasPrincipalKey(nameof(ApplicationUser.Id)),
            join =>
            {
                join.ToTable("UserRoles"); // Specify the name of the join table
                join.HasKey("userId", "roleId"); // Specify the composite primary key
            }
);


        }
    }
}

