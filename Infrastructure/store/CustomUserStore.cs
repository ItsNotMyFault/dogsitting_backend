using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System.Linq;

namespace dogsitting_backend.Infrastructure.store
{

    public class CustomUserStore :
        IUserStore<AuthUser>,
        IUserEmailStore<AuthUser>,
        IUserRoleStore<AuthUser>,
        IUserLoginStore<AuthUser>,
        IUserPasswordStore<AuthUser>
    {

        private readonly IGenericRepository<ApplicationUser> userGenereicRepository;
        private readonly IGenericRepository<ApplicationRole> roleGenericRepository;
        private readonly IGenericRepository<UserLogin> userLoginGenericRepository;
        private readonly UserSQLRepository userSQLRepository;
        private readonly RoleSQLRepository RoleSQLRepository;


        public CustomUserStore(
            IGenericRepository<ApplicationUser> userGenereicRepository,
            IGenericRepository<ApplicationRole> roleGenericRepository,
            IGenericRepository<UserLogin> UserLoginGenericRepository,
            UserSQLRepository userSQLRepository,
            RoleSQLRepository roleSQLRepository
        )
        {
            this.userGenereicRepository = userGenereicRepository;
            this.roleGenericRepository = roleGenericRepository;
            this.userLoginGenericRepository = UserLoginGenericRepository;
            this.userSQLRepository = userSQLRepository;
            this.RoleSQLRepository = roleSQLRepository;
        }


        public IQueryable<AuthUser> Users => QueryableUsers().Result;
        public async Task<IQueryable<AuthUser>> QueryableUsers()
        {
            //List<ApplicationUser> applicationUsers = this.userGenereicRepository.Build().Result.ToList();
            List<ApplicationUser> applicationUsers = this.userGenereicRepository.Build().Include(t => t.Roles).ToList();
            //foreach (AuthenticationUser applicationUser in applicationUsers)
            //{
            //    //List<ApplicationRole> roles = (await _usersRolesTable.GetRolesAsync(applicationUser)).ToList();
            //    //applicationUser.Roles = roles.OrderByDescending(x => x.Name).ToList();
            //}
            return (IQueryable<AuthUser>)applicationUsers.ToList().AsQueryable();
        }



        public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }
            if (user.ApplicationUser == null)
            {
                throw new ArgumentNullException(nameof(user.ApplicationUser), "Parameter user.ApplicationUser is not set to an instance of an object.");
            }

            this.userGenereicRepository.AddAsync(user.ApplicationUser);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            this.userGenereicRepository.DeleteAsync(user.ApplicationUser);
            return null;

        }

        public async Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(userId, out Guid idGuid))
            {
                throw new ArgumentException("Not a valid Guid id", nameof(userId));
            }
            //passing wrong ID passing provider ID instead of userID
            var applicationuser = this.userSQLRepository.FindById(idGuid).Result;
            if (applicationuser == null)
            {
                return null;
            }

            List<ApplicationRole> roles = await this.RoleSQLRepository.GetUserRolesAsync(applicationuser);
            var authenticatedUser = new AuthUser(applicationuser)
            {
                Roles = roles
            };
            return authenticatedUser;
        }

        public async Task<AuthUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {

            return null;
        }

        public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.ApplicationUser.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);

        }

        public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            if (string.IsNullOrEmpty(normalizedName))
            {
                throw new ArgumentNullException(nameof(normalizedName), "Parameter normalizedName cannot be null or empty.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }


        public Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName), "Parameter userName cannot be null or empty.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;

            //return Task.FromResult(_usersTable.SetUserNameAsync(user, userName, cancellationToken));
            return null;
        }

        public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            return null;
        }
        public Task<IdentityResult> UpdateAccountAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            return null;
        }



        public Task SetEmailAsync(AuthUser user, string email, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Parameter email cannot be null or empty.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetEmailAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AuthUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var applicationuser = this.userSQLRepository.FindByEmail(normalizedEmail).Result;
            return Task.FromResult<AuthUser>(new AuthUser(applicationuser));
        }

        public Task<string> GetNormalizedEmailAsync(AuthUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(AuthUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            if (string.IsNullOrEmpty(normalizedEmail))
            {
                throw new ArgumentNullException(nameof(normalizedEmail), "Parameter normalizedEmail cannot be null or empty.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
        }

        public Task AddToRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(AuthUser user, CancellationToken cancellationToken)
        {
            List<ApplicationRole> userRoles = this.RoleSQLRepository.GetUserRolesAsync(user.ApplicationUser).Result;
            var roles = Task.FromResult<IList<string>>(userRoles.Select(t => t.NormalizedName).ToList());
            return roles;
        }

        public Task<bool> IsInRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user is not set to an instance of an object.");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                return Task.FromResult(false);
            }
            roleName = roleName.ToUpper();


            var userRoles = GetRolesAsync(user, cancellationToken).Result;
            return Task.FromResult(userRoles.Contains(roleName));
        }

        public Task<IList<AuthUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task AddLoginAsync(AuthUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            try
            {
                var newUserLogin = new UserLogin(
                    login.LoginProvider,
                    login.ProviderKey,
                    user.ApplicationUser.Id,
                    login.ProviderDisplayName
                );

                await this.userLoginGenericRepository.AddAsync(newUserLogin);
            }
            catch (Exception ex)
            {
                var e = ex;
                // throw new ApplicationException("Failed to add user login", ex);
            }
        }
        public Task RemoveLoginAsync(AuthUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            UserLogin userLogin = userLoginGenericRepository.Build().FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            return userLoginGenericRepository.DeleteAsync(userLogin);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider), "Parameter loginProvider is not set");
            }
            if (string.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException(nameof(providerKey), "Parameter providerKey is not set");
            }
            ApplicationUser? user = null;
            try
            {
                user = await this.userSQLRepository.FindByLoginProvider(loginProvider, providerKey);
            }
            catch (Exception e)
            {
                var tt = e;
            }
            return user != null ? new AuthUser(user) : null;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("true");
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("true");
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task SetPasswordHashAsync(AuthUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task<string?> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("true");
        }

        public Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<bool>(false);
        }
    }
}
