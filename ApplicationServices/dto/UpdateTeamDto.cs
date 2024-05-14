using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.ApplicationServices.dto
{
    public class UpdateTeamDto
    {
        [Required]
        public string? Name { get; set; }
        public bool UseAvailabilities { get; set; }
        public bool UseUnavailabilities { get; set; }
        public int MaxWeekDaysLodgerCount { get; set; }
        public int MaxWeekendDaysLodgerCount { get; set; }
    }
}
