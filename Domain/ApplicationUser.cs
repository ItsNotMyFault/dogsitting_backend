using dogsitting_backend.Domain;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dogsitting_backend.Domain;

public class ApplicationUser : DBModel, IIdentity
{
    public ApplicationUser() { }
    public ApplicationUser(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;

    }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string Name { get => this.FirstName + " " + this.LastName; }

    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Team> Teams { get; } = [];
    //public string Email { get; set; }
    //public int Enable { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual IList<Reservation> Reservations { get; set; }
    public IList<ApplicationRole> Roles { get; set; }

    [NotMapped]
    [Newtonsoft.Json.JsonIgnore]
    public  string? AuthenticationType
    {
        get => "authType";
        set
        {
            AuthenticationType = "yes";
        }
    }
    [NotMapped]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsAuthenticated
    {
        get => IsAuthenticated;
        set
        {
            IsAuthenticated = false;
        }
    }

    public bool HasRole(string roleName)
    {
        bool hasRole = this.Roles.Select(x => x.Name).Contains(roleName.ToUpper());
        return hasRole;
    }

    public bool HasOneOfThoseRoles(List<string> rolesName)
    {
        bool hasOneofTheRoles = rolesName.Select(x => this.HasRole(x)).Contains(true);
        return hasOneofTheRoles;
    }

}


