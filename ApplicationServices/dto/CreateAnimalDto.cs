using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.ApplicationServices.dto
{
    public class CreateAnimalDto
    {
        [Required]
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public double WeightKg { get; set; }
        public string Notes { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
