using dogsitting_backend.domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        [NotMapped]
        public DateTimePeriod Period { get; set; }
        [NotMapped]
        public ApplicationUser Client { get; set; }
        public Guid Team_Id { get; set; }
 
        public Team Team { get; set; }
        private int LodgerCount = 1;

        public Reservation() { }

        public Reservation(DateTimePeriod period, ApplicationUser client, Team team)
        {
            this.Period = period;
            this.Client = client;
            this.Team = team;
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
