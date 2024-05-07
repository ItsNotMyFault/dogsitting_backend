using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.ApplicationServices.dto
{
    public class UpdateTeamDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
