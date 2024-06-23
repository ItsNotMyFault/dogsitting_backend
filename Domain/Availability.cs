using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    [Table("Availabilities")]
    public class Availability : DBModel
    {

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public DateTimePeriod Period
        {
            get => new(this.DateFrom, this.DateTo);
            set
            {
                DateFrom = value.StartDate;
                DateTo = value.EndDate;
            }
        }

        [ForeignKey("Calendar")]
        public Guid CalendarId { get; set; }

        public Calendar Calendar { get; set; }
        public bool IsAllday { get; set; }
        public bool IsAvailable { get; set; }

        public Availability(string date, bool isAvailable)
        {
            this.Period = new DateTimePeriod(date);
            this.IsAvailable = isAvailable;
        }

        public Availability() { }


        public override string ToString()
        {
            return $"{Period}";
        }

    }
}
