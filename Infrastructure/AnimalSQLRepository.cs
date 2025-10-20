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
            return await this.Context.Animals.Include(a => a.Media).ToListAsync();
        }


        public async Task<List<Animal>> GetUserAnimalsAsync(Guid userId)
        {
            return await this.Context.Animals.Where(animal => animal.User.Id == userId).Include(animal => animal.User).Include(animal => animal.Media).ToListAsync();
        }

        public async Task<Animal> GetById(Guid animalId)
        {
            return await this.Context.Animals.Where(animal => animal.Id == animalId).Include(animal => animal.Media).FirstAsync();
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
