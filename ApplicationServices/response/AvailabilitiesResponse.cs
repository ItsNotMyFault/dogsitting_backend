using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    public class AvailabilitiesResponse
    {

        // Define backing fields for DateFrom and DateTo
        public required List<ReservationEvent> BusyEvents { get; set; }
        public required List<AvailableCalendarEvent> AvailableEvents { get; set; }




    }
}
