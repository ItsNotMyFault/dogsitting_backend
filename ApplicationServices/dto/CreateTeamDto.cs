using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.ApplicationServices.dto
{
    public class CreateTeamDto
    {
        [Required]
        public string Name { get; set; }
    }
}
