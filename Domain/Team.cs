using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    public class Team : DBModel
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public DateTime? ApprovedAt { get; set;}

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();
        public virtual Calendar Calendar { get; set; }

        public Team()
        {
            this.Name = "team name";
        }

        public Team(string name)
        {
            this.Name = name;
            this.NormalizeTeamName();
        }

        public void NormalizeTeamName()
        {
            this.NormalizedName = this.Name.ToLower().Trim().Replace(" ", ".");
        }

    }
}
