using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.calendar
{
    [NotMapped]
    public class ReservationCalendarEvent : CalendarEvent
    {
        public Guid ReservationId { get; set; }

        public ReservationCalendarEvent(Guid reservationId, DateTimePeriod DateTimePeriod, string eventSubject) : base (DateTimePeriod)
        {
            this.EventSubject = eventSubject;
            this.DateTimePeriod = DateTimePeriod;
            this.Id = reservationId;
        }
    }
}
