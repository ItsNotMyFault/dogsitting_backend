using dogsitting_backend.domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        // Define backing fields for DateFrom and DateTo
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        // Map DateFrom and DateTo fields to DateTimePeriod inner class
        [NotMapped]
        public DateTimePeriod Period
        {
            get => new DateTimePeriod(this.DateFrom, this.DateTo);
            set
            {
                DateFrom = value.StartDate;
                DateTo = value.EndDate;
            }
        }
        [NotMapped]
        public ApplicationUser Client { get; set; }
        public Calendar Calendar{ get; set; }
        public Guid CalendarId { get; set; }

        public int LodgerCount { get; set; } = 1;

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

            foreach (DateTime datetime in Period.EachDay())
            {
                
            };
            return [];
        }
    }
}
