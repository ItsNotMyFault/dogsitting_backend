using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.Domain.auth
{
    public class ApplicationRole
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}