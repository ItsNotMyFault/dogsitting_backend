using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace dogsitting_backend.Domain.calendar
{
    [NotMapped]
    public class AvailableCalendarEvent : CalendarEvent
    {
        public bool IsAvailable { get; set; }
        public AvailableCalendarEvent(Availability availability) : base(availability.DateFrom)
        {
            this.DateTimePeriod = availability.Period;
            this.IsAvailable = availability.IsAvailable;
        }

    }
}
