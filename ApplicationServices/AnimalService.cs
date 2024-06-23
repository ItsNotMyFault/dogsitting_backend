using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.media;
using dogsitting_backend.Infrastructure;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Collections.Generic;

namespace dogsitting_backend.ApplicationServices
{
    public class AnimalService
    {
        private AnimalSQLRepository AnimalRepository;
        private readonly MediaSQLRepository _mediaSQLRepository;
        public AnimalService(AnimalSQLRepository animalRepository, MediaSQLRepository mediaSQLRepository)
        {
            this.AnimalRepository = animalRepository;
            this._mediaSQLRepository = mediaSQLRepository;
        }

        public async Task<AnimalResponse> GetAnimalById(Guid animalId)
        {
            Animal animal = await AnimalRepository.GetById(animalId);
            return new AnimalResponse(animal);
        }

        public async Task<List<AnimalResponse>> GetAnimalsByUserId(Guid userId)
        {
            List<Animal> animals = await AnimalRepository.GetUserAnimalsAsync(userId);
            return animals.Select(animal => new AnimalResponse(animal)).ToList();
        }

        public async Task CreateUserAnimal(CreateAnimalDto animal, Guid userId, IFormFile? file)
        {
            Animal newAnimal = new(animal, userId);
            if (file != null)
            {
                Media media = new(file);
                await this._mediaSQLRepository.AddMediaAsync(media);
                newAnimal.Media = media;
                newAnimal.MediaId = media.Id;
                newAnimal.UserId = userId;
            }

            await AnimalRepository.Create(newAnimal);
        }

        public async Task UpdateAnimal(Guid animalId, CreateAnimalDto animal, Guid userId)
        {
            Animal foundAnimal = await this.AnimalRepository.GetById(animalId);
            if (animal == null)
            {
                throw new Exception("Animal not found.");
            }
            foundAnimal.Update(animal, userId);
            await AnimalRepository.Update(foundAnimal);
        }

        public async Task<List<Animal>> GetAnimals()
        {
            return await AnimalRepository.GetAnimalsAsync();
        }

        public async Task UpdateAnimalMedia(Guid animalId, IFormFile file)
        {
            Animal animal = await this.AnimalRepository.GetById(animalId);
            if (animal == null)
            {
                throw new Exception("Animal not found.");
            }

            Media newMedia = new(file);
            if (animal.MediaId != null)
            {
                await this._mediaSQLRepository.DeleteMediaAsync(animal.MediaId);
            }
            animal.MediaId = Guid.Empty;
            animal.Media = newMedia;
            await this.AnimalRepository.Update(animal);
        }

        public async Task DeleteAnimalMedia(Guid animalId)
        {
            Animal animal = await this.AnimalRepository.GetById(animalId);
            await this._mediaSQLRepository.DeleteMediaAsync(animal.MediaId);
            animal.MediaId = Guid.Empty;
            animal.Media = null;
            await this.AnimalRepository.Update(animal);
        }


    }
}
