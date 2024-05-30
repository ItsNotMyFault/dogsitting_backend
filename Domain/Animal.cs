using dogsitting_backend.ApplicationServices.dto;

namespace dogsitting_backend.Domain
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string? Gender { get; set; }
        public double WeightKg { get; set; }
        public string Notes { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Animal() { }

        public Animal(CreateAnimalDto createAnimalDto)
        {
            this.Name = createAnimalDto.Name;
            this.Species = createAnimalDto.Species;
            this.Breed = createAnimalDto.Breed;
            this.Gender = createAnimalDto.Gender;
            this.WeightKg = createAnimalDto.WeightKg;
            this.Notes = createAnimalDto.Notes;
            this.Birthdate = createAnimalDto.Birthdate;
        }
    }
}
