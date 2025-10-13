using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Domain.media;

namespace dogsitting_backend.Domain
{
    [Table("Reservations")]
    public class Reservation : DBModel
    {

        // Define backing fields for DateFrom and DateTo
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        // Map DateFrom and DateTo fields to DateTimePeriod inner class
        public DateTimePeriod Period
        {
            get => new(this.DateFrom, this.DateTo);
            set
            {
                DateFrom = value.StartDate;
                DateTo = value.EndDate;
            }
        }

        [ForeignKey("Client")]
        public Guid UserId { get; set; }
        public virtual ApplicationUser Client { get; set; }


        public virtual Calendar Calendar { get; set; }
        public Guid CalendarId { get; set; }
        //public ICollection<Media> Medias { get; set; } = new List<Media>(); //navigation property
        public ICollection<ReservationMedia> ReservationMedias { get; set; } = new List<ReservationMedia>(); //navigation property

        public required int LodgerCount { get; set; }
        public string? Notes { get; set; }
        public string ReservationCalendarTitle { get => $"{this.Client?.Name} ({this.LodgerCount})"; }
        public string ReservationTitle { get => $"{this.Client?.Name}"; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public Reservation() { }//required for db initialization
        public Reservation(Calendar calendar)
        {
            this.Calendar = calendar;
            this.CalendarId = calendar.Id;
        }

        public Reservation(DateTimePeriod period, ApplicationUser client)
        {
            this.Period = period;
            this.Client = client;
        }


        public List<CalendarEvent> GetDailyEvents()
        {
            List<CalendarEvent> events = [];
            List<CalendarEvent> events2 = [];
            IEnumerable<DateTime> dates = this.Period.ToLocalTimezone().EachDay();
            foreach (DateTime datetime in dates)
            {
                events.Add(new CalendarEvent(datetime));
            }
            ;
            return events;
        }

        public CalendarEvent GetReservationEvent()
        {
            return null;
        }

        public override string ToString()
        {
            return $"{Period}";
        }
    }
}
