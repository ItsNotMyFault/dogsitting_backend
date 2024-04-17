using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class RoleSQLRepository
    {
        public DogsittingDBContext context { get; set; }
        private readonly IGenericRepository<ApplicationUser> UserGenericRepository;

        public RoleSQLRepository(DogsittingDBContext context, IGenericRepository<ApplicationUser> userGenericRepository)
        {
            this.context = context;
            this.UserGenericRepository = userGenericRepository;
        }

        public async Task<List<ApplicationRole>> GetAllAsync()
        {

            return await this.context.Roles.ToListAsync();
        }

        public async Task<List<ApplicationRole>> GetUserRolesAsync(ApplicationUser applicationUser)
        {

            return await this.context.Roles.Where(r => r.Users.Any(u => u.Id == applicationUser.Id)).ToListAsync();
        }


        public async Task<Object> Create(ApplicationRole role)
        {
            this.context.Roles.Add(role);
            await this.context.SaveChangesAsync();
            return role;
        }


    }
}
