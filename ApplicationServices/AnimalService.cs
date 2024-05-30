using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;

namespace dogsitting_backend.ApplicationServices
{
    public class AnimalService
    {
        private AnimalSQLRepository AnimalRepository;
        public AnimalService(AnimalSQLRepository animalRepository)
        {
            this.AnimalRepository = animalRepository;
        }

        public async Task<Animal> GetAnimal(Guid animalId)
        {
            return await AnimalRepository.GetAnimalAsync(animalId);
        }

        public async Task<List<Animal>> GetAnimalsByUserId(Guid userId)
        {
            return await AnimalRepository.GetUserAnimalsAsync(userId);
        }

        public async Task CreateAnimal(CreateAnimalDto animal)
        {
            await  AnimalRepository.Create(new Animal(animal));
        }

        public async Task UpdateAnimal(Guid animalId, Animal animal)
        {
            Animal foundAnimal = await this.AnimalRepository.GetAnimalAsync(animalId);
            animal.Id = foundAnimal.Id;
            await AnimalRepository.Update(animal);
        }

        public async Task<List<Animal>> GetAnimals()
        {
            return await AnimalRepository.GetAnimalsAsync();
        }


    }
}
