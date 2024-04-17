using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.Domain
{
    public class DBModel
    {
        [Key]
        public Guid Id { get; set; }

    }
}
