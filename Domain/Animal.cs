using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain.media;

namespace dogsitting_backend.Domain
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public Gender Gender { get; set; }
        public double WeightKg { get; set; }
        public string Notes { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Guid? MediaId { get; set; }
        public virtual Media? Media { get; set; }

        public Animal() { }

        public Animal(CreateAnimalDto createAnimalDto, Guid userId)
        {
            this.Update(createAnimalDto, userId);
        }

        public void Update(CreateAnimalDto createAnimalDto, Guid userId)
        {
            this.Name = createAnimalDto.Name;
            this.Species = createAnimalDto.Species;
            this.Breed = createAnimalDto.Breed;
            try
            {
                this.Gender = (Gender)Enum.Parse(typeof(Gender), createAnimalDto.Gender, true);
            }
            catch (Exception)
            {
                this.Gender = Gender.Unknown;
            }
            this.WeightKg = createAnimalDto.WeightKg;
            this.Notes = createAnimalDto.Notes;
            this.Birthdate = createAnimalDto.Birthdate;
            this.UserId = userId;
            this.CreatedAt = DateTime.Now;
        }
    }
}
