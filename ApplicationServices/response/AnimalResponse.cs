using dogsitting_backend.Domain.media;
using dogsitting_backend.Domain;
using dogsitting_backend.ApplicationServices.dto;

namespace dogsitting_backend.ApplicationServices.response
{
    public class AnimalResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public Gender Gender { get; set; }
        public double WeightKg { get; set; }
        public string? Notes { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }

        public Media? Media { get; set; }

        public AnimalResponse(Animal animal)
        {
            this.Id = animal.Id;
            this.Name = animal.Name;
            this.Species = animal.Species;
            this.Breed = animal.Breed;
            this.Gender = animal.Gender;
            this.WeightKg = animal.WeightKg;
            this.Notes = animal.Notes;
            this.Birthdate = animal.Birthdate;
            this.Media = animal.Media;
        }
    }

    
}
