using Microsoft.AspNetCore.Identity;
using dogsitting_backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using dogsitting_backend.Domain.auth;

namespace umbrella.portal.CustomProvider
{
    public class CustomRoleStore : IRoleStore<ApplicationRole>, IQueryableRoleStore<ApplicationRole>
    {
        private readonly IGenericRepository<ApplicationRole> roleGenereicRepository;

        public CustomRoleStore(IGenericRepository<ApplicationRole> roleGenereicRepository)
        {
            this.roleGenereicRepository = roleGenereicRepository;
        }


        public IQueryable<ApplicationRole> Roles => QueryableRoles().Result;
        public async Task<IQueryable<ApplicationRole>> QueryableRoles()
        {
            IEnumerable<ApplicationRole> roles = await this.roleGenereicRepository.GetAllAsync();
            return roles.AsQueryable();
        }

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(roleId, out Guid idGuid))
            {
                throw new ArgumentException("Not a valid Guid id", nameof(roleId));
            }

            return this.roleGenereicRepository.GetByIdAsync(idGuid);
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (String.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentException("Not a valid rolename", nameof(normalizedRoleName));
            }

            return this.roleGenereicRepository.Build().FirstOrDefaultAsync(role => role.Name == normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}