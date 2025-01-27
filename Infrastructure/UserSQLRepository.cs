using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class UserSQLRepository
    {
        public DogsittingDBContext context { get; set; }
        private readonly IGenericRepository<ApplicationUser> UserGenericRepository;

        public UserSQLRepository(DogsittingDBContext context, IGenericRepository<ApplicationUser> userGenericRepository)
        {
            this.context = context;
            this.UserGenericRepository = userGenericRepository;
        }

        public async Task<List<ApplicationUser>> GetAlUsersAsync()
        {

            return await this.context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> FindByLoginProvider(string loginProvider, string providerKey)
        {
            return await this.context.Users.Include(x => x.UserLogins).Where(x => x.UserLogins.Any(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindByEmail(string email)
        {
            return await this.context.Users.Where(x => x.Email == email).FirstAsync();
        }

        public async Task<ApplicationUser> FindById(Guid userId)
        {
            return await this.context.Users.Where(x => x.Id == userId).FirstAsync();
        }


        public async Task<Object> Create(ApplicationUser user)
        {
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<Object> Update(ApplicationUser user)
        {
            this.context.Users.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(Guid id)
        {
            var applicationUser = await this.context.Users.FindAsync(id);
            if (applicationUser != null)
            {
                this.context.Users.Remove(applicationUser);
            }

            await this.context.SaveChangesAsync();
        }

        private bool ApplicationUserExists(Guid id)
        {
            return this.context.Users.Any(e => e.Id == id);
        }



    }
}
