using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Domain.media;
using dogsitting_backend.Domain.Utils;

namespace dogsitting_backend.Domain
{
    public class Team : DBModel
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();  //navigation property
        public ICollection<TeamMedia> TeamMedias { get; set; }
        public virtual Calendar Calendar { get; set; }

        public Team()
        {
            this.Name = "team name";
        }

        public Team(CreateTeamDto team)
        {
            this.Name = team.Name;
        }

        public Team(string name)
        {
            this.Name = name;
            this.NormalizeTeamName();
        }

        public void NormalizeTeamName()
        {
            this.NormalizedName = StringUtils.ToKebabCase(this.Name);
        }

    }
}
