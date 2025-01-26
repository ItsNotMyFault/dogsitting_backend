using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.media;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace dogsitting_backend.Domain;

public class ApplicationUser : DBModel
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
    public string? PhoneNumber { get; set; }
    public string Name { get => this.FirstName + " " + this.LastName; }

    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Team> Teams { get; } = new List<Team>();
    [Newtonsoft.Json.JsonIgnore]
    public virtual IList<Reservation> Reservations { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual IList<ApplicationRole> Roles { get; set; }
    public virtual IList<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
    public ICollection<UserMedia> UserMedias { get; set; }


}


