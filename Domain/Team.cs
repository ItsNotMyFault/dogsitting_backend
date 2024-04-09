using dogsitting_backend.domain;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dogsitting_backend.Domain
{
    public class Team : DBModel
    {
        public string Name { get; set; }

        public ICollection<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();
        [NotMapped]
        public Calendar Calendar { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<Reservation> Reservations{ get; set; }


        public Team()
        {
            this.Name = "team name";
        }

        public Team(string name)
        {
            this.Name = name;
        }

    }
}
