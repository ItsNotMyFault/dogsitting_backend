using dogsitting_backend.Domain;
using System.ComponentModel.DataAnnotations;

namespace dogsitting_backend.ApplicationServices.dto
{
    public class CreateAnimalDto
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public double WeightKg { get; set; }
        public string Notes { get; set; }
        public DateTime? Birthdate { get; set; }

        //public Animal CreateDomain()
        //{
        //    Gender gender;
        //    try
        //    {
        //        gender = (Gender)Enum.Parse(typeof(Gender), this.Gender, true);
        //    }
        //    catch (Exception) {

        //        gender = Gender.Unknown;
        //    }

        //    return new Animal()
        //    {
        //        Name = Name,
        //        Species = Species,
        //        Breed = Breed,
        //        WeightKg = this.WeightKg,
        //        Gender = gender,
        //    };
        //}
    }
}
