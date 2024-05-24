using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    public class ReservationResponse
    {

        // Define backing fields for DateFrom and DateTo
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ApplicationUser Client { get; set; }
        public Team Team { get; set; }
        public Guid Id { get; set; }
        public int LodgerCount { get; set; } = 1;
        public string ReservationCalendarTitle { get => $"{this.Client?.Name} ({this.LodgerCount})"; }
        public string ReservationTitle { get => $"{this.Client.Name}"; }

        public ReservationResponse(Reservation reservation)
        {
            this.DateFrom = reservation.DateFrom.ToLocalTime();
            this.DateTo = reservation.DateTo.ToLocalTime();
            this.ApprovedAt = reservation.ApprovedAt?.ToLocalTime();
            this.CreatedAt = reservation.CreatedAt?.ToLocalTime();
            this.Client = reservation.Client;
            this.Team = reservation?.Calendar?.Team;
            this.LodgerCount = reservation.LodgerCount;
            this.Id = reservation.Id;
        }

    }
}
