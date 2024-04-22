using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dogsitting_backend.Domain.calendar;

namespace dogsitting_backend.Domain
{
    [Table("Reservations")]
    public class Reservation :DBModel
    {

        // Define backing fields for DateFrom and DateTo
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        // Map DateFrom and DateTo fields to DateTimePeriod inner class
        public DateTimePeriod Period
        {
            get => new DateTimePeriod(this.DateFrom, this.DateTo);
            set
            {
                DateFrom = value.StartDate;
                DateTo = value.EndDate;
            }
        }

        [ForeignKey("Client")]
        public Guid UserId { get; set; }
        public virtual ApplicationUser Client { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual Calendar Calendar { get; set; }

        public Guid CalendarId { get; set; }

        public int LodgerCount { get; set; } = 1;
        public string ReservationCalendarTitle{ get => $"{this.Client.Name} ({this.LodgerCount})"; }
        public string ReservationTitle { get => $"{this.Client.Name}"; }

        public Reservation() { }

        public Reservation(DateTimePeriod period, ApplicationUser client)
        {
            this.Period = period;
            this.Client = client;
        }

        //list all events (admin) to see all dogos


        public List<CalendarEvent> GetEvents()
        {
            //get events for client mode.

            //get busy status for client mode.

            //
            var dailyEvents = this.Calendar.GetDailyCalendarEvents();
            List<CalendarEvent> events = [];
            foreach (DateTime datetime in this.Period.EachDay())
            {
                dailyEvents.Where(eve => eve.DateTimePeriod.StartDate.Equals(datetime)).ToList();
                events.Add(new CalendarEvent(datetime));
            };
            return events;
        }
    }
}
