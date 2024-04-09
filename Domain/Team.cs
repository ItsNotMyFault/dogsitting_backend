using dogsitting_backend.domain;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace dogsitting_backend.Domain
{
    public class Team : DBModel
    {
        public string Name { get; set; }

        public ICollection<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();
        //public virtual Calendar Calendar { get; set; }


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
