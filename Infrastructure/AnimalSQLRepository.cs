using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class AnimalSQLRepository
    {
        public DogsittingDBContext Context { get; set; }

        public AnimalSQLRepository(DogsittingDBContext context)
        {
            this.Context = context;
        }

        public async Task<List<Animal>> GetAnimalsAsync()
        {
            return await this.Context.Animals.ToListAsync();
        }


        public async Task<List<Animal>> GetUserAnimalsAsync(Guid userId)
        {
            return await this.Context.Animals.Include(animal => animal.User).Where(animal => animal.User.Id == userId).ToListAsync();
        }

        public async Task<Animal> GetAnimalAsync(Guid animalId)
        {
            return await this.Context.Animals.FindAsync(animalId);
        }

        public async Task<Animal> Create(Animal animal)
        {
            this.Context.Animals.Add(animal);
            await this.Context.SaveChangesAsync();
            return animal;
        }

        public async Task<Animal> Update(Animal animal)
        {
            this.Context.Animals.Update(animal);
            await this.Context.SaveChangesAsync();
            return animal;
        }

    }
}
