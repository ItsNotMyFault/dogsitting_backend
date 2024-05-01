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
        public async Task<ApplicationUser> Details(Guid id)
        {
            if (id == null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([Bind("Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            //if (ModelState.IsValid)
            //{
            applicationUser.Id = Guid.NewGuid();
            await this._userSQLRepository.Create(applicationUser);
            //}
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Edit(Guid id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                throw new Exception("User not found");
            }

            await this._userSQLRepository.Update(applicationUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]//what does that do?
        public async Task Delete(Guid id)
        {
            await this._userSQLRepository.Delete(id);
        }

    }
}
