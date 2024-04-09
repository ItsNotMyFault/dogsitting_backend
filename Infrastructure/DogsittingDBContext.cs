using dogsitting_backend.domain;
using dogsitting_backend.Domain;
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
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            //modelBuilder.Entity<Reservation>().ToTable("Reservations").HasOne(e => e.Team);
            //modelBuilder.Entity<Reservation>().ToTable("Reservations").HasOne(e => e.Client);
            // modelBuilder.Entity<Team>()
            //.HasOne(t => t.Admin)
            //.WithMany()
            //.HasForeignKey(t => t.User_Id)
            //.IsRequired();
            //modelBuilder.Entity<Team>().HasOne(t => t.Calendar);
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Admins)
                .WithMany(t => t.Teams)
            .UsingEntity("TeamUsers",
                right => right.HasOne(typeof(ApplicationUser)).WithMany().HasForeignKey("user_id").HasPrincipalKey(nameof(ApplicationUser.Id)),
                left => left.HasOne(typeof(Team)).WithMany().HasForeignKey("team_id").HasPrincipalKey(nameof(Team.Id)),
                join =>
                {
                    join.ToTable("TeamUsers"); // Specify the name of the join table
                    join.HasKey("user_id", "team_id"); // Specify the composite primary key
                }
            ); 


        }
    }
}
