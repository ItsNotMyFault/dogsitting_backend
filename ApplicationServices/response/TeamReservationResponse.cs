using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    public class TeamReservationResponse
    {

        // Define backing fields for DateFrom and DateTo
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public ApplicationUser Client { get; set; }
        public Team Team { get; set; }

        public int LodgerCount { get; set; } = 1;
        public string ReservationCalendarTitle { get => $"{this.Client?.Name} ({this.LodgerCount})"; }
        public string ReservationTitle { get => $"{this.Client.Name}"; }

        public TeamReservationResponse(Reservation reservation)
        {
            this.DateFrom = reservation.DateFrom;
            this.DateTo = reservation.DateTo;
            this.Client = reservation.Client;
            this.Team = reservation.Calendar.Team;

            this.LodgerCount = reservation.LodgerCount;
        }

    }
}
