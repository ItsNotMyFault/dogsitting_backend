using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.ApplicationServices
{
    public class ApplicationUserService
    {
        private readonly UserSQLRepository _userSQLRepository;

        public ApplicationUserService(UserSQLRepository userSQLRepository)
        {
            this._userSQLRepository = userSQLRepository;
        }


        public Task<List<ApplicationUser>> Index()
        {
            return this._userSQLRepository.GetAlUsersAsync();
        }

        // GET: ApplicationUser/Details/5
        public async Task<ApplicationUser> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception("Id param is required");
            }

            ApplicationUser applicationUser = await this._userSQLRepository.FindById(id);
            if (applicationUser == null)
            {
                throw new Exception("User id not found");
            }

            return applicationUser;
        }

        public async Task Create([Bind("Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            //if (ModelState.IsValid)
            //{
            applicationUser.Id = Guid.NewGuid();
            await this._userSQLRepository.Create(applicationUser);
            //}
        }


        public async Task Edit(Guid id, UpdateUserDto updateUserDto)
        {

            if(updateUserDto == null)
            {
                throw new Exception("User profile parameters are null.");
            }
            var applicationUser = await this.GetUserById(id);

            // Update user properties based on the DTO
            applicationUser.FirstName = updateUserDto.FirstName;
            applicationUser.LastName = updateUserDto.LastName;
            applicationUser.Email = updateUserDto.Email;
            applicationUser.PhoneNumber = updateUserDto.PhoneNumber;
            await this._userSQLRepository.Update(applicationUser);
        }

        public async Task Delete(Guid id)
        {
            await this._userSQLRepository.Delete(id);
        }

    }
}
