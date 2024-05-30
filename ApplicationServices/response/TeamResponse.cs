using dogsitting_backend.Domain;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.ApplicationServices.response
{
    public class TeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool? UseAvailabilities { get; set; }
        public bool? UseUnavailabilities { get; set; }
        public int? MaxWeekDaysLodgerCount { get; set; }
        public int? MaxWeekendDaysLodgerCount { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public ICollection<ApplicationUser> Admins { get; set; } = [];
        public List<TeamMediaResponse> TeamMediaResponses { get; set; } = new List<TeamMediaResponse>();


        public TeamResponse(Team team, List<TeamMediaResponse> teamMediaResponses = null)
        {
            this.Id = team.Id;
            this.Name = team.Name;
            this.NormalizedName = team.NormalizedName;
            if(team.Calendar != null)
            {
                this.UseAvailabilities = team.Calendar?.UseAvailabilities;
                this.UseUnavailabilities = team.Calendar?.UseUnavailabilities;
                this.MaxWeekDaysLodgerCount = team.Calendar?.MaxWeekDaysLodgerCount;
                this.MaxWeekendDaysLodgerCount = team.Calendar?.MaxWeekendDaysLodgerCount;
            }

            this.Admins = team.Admins;
            this.CreatedAt = team.CreatedAt;
            this.ApprovedAt = team.ApprovedAt;
            if (teamMediaResponses != null && teamMediaResponses.Count > 0)
            {
                TeamMediaResponses = teamMediaResponses;
            }
        }
    }
}
