using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace dogsitting_backend.Domain.auth;

public class AuthUser : IdentityUser
{
    public ApplicationUser ApplicationUser { get; set; }

    public AuthUser(ApplicationUser ApplicationUser)
    {
        this.ApplicationUser = ApplicationUser;
        this.UserName = $"{ApplicationUser?.FirstName} {ApplicationUser?.LastName}";
        this.Email = ApplicationUser?.Email;
    }

    public AuthUser() { }

    public IList<ApplicationRole> Roles { get; set; }

    public bool HasRole(string roleName)
    {
        bool hasRole = Roles.Select(x => x.Name).Contains(roleName.ToUpper());
        return hasRole;
    }

    public bool HasOneOfThoseRoles(List<string> rolesName)
    {
        bool hasOneofTheRoles = rolesName.Select(x => HasRole(x)).Contains(true);
        return hasOneofTheRoles;
    }

}


