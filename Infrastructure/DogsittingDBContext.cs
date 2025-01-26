using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Domain.media;
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
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ReservationMedia> ReservationMedia { get; set; }
        public DbSet<TeamMedia> TeamMedia { get; set; }
        public DbSet<UserMedia> UserMedia { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            MediaBuilder(modelBuilder);

            modelBuilder.Entity<Calendar>().ToTable("Calendars").HasOne(x => x.Team).WithOne(x => x.Calendar);
            modelBuilder.Entity<Calendar>().ToTable("Calendars").HasMany(t => t.Reservations).WithOne(r => r.Calendar);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasMany(t => t.Reservations).WithOne(r => r.Client);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasMany(t => t.UserLogins).WithOne(r => r.User);

            modelBuilder.Entity<Animal>()
            .Property(a => a.Gender)
            .HasConversion<string>();


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
        left => left.HasOne(typeof(ApplicationUser)).WithMany().HasForeignKey("userId").HasPrincipalKey(nameof(ApplicationUser.Id)),
            join =>
            {
                join.ToTable("UserRoles"); // Specify the name of the join table
                join.HasKey("userId", "roleId"); // Specify the composite primary key
            }
);

            base.OnModelCreating(modelBuilder);
        }

        private void MediaBuilder(ModelBuilder modelBuilder)
        {
            //reservation media
            modelBuilder.Entity<ReservationMedia>()
            .HasKey(rm => new { rm.ReservationId, rm.MediaId });

            modelBuilder.Entity<ReservationMedia>()
                .HasOne(rm => rm.Reservation)
                .WithMany(r => r.ReservationMedias)
                .HasForeignKey(rm => rm.ReservationId);

            modelBuilder.Entity<ReservationMedia>()
                .HasOne(rm => rm.Media)
                .WithMany(m => m.ReservationMedias)
                .HasForeignKey(rm => rm.MediaId);
            //team media
            modelBuilder.Entity<TeamMedia>()
                .HasKey(tm => new { tm.TeamId, tm.MediaId });

            modelBuilder.Entity<TeamMedia>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMedias)
                .HasForeignKey(tm => tm.TeamId);

            modelBuilder.Entity<TeamMedia>()
                .HasOne(tm => tm.Media)
                .WithMany(m => m.TeamMedias)
                .HasForeignKey(tm => tm.MediaId);
            //user Media
            modelBuilder.Entity<UserMedia>()
                .HasKey(um => new { um.UserId, um.MediaId });

            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMedias)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.Media)
                .WithMany(m => m.UserMedias)
                .HasForeignKey(um => um.MediaId);
        }
    }
}

