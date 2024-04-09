﻿using System;
using System.Collections.Generic;

namespace dogsitting_backend.Domain
{
    public class Calendar
    {
        public Guid Id { get; set; }
        public List<DateTimePeriod> UnavailablePeriods { get; set; }
        public virtual List<Reservation> Reservations { get; set; }

        public int MaxWeekDaysLodgerCount = 1;
        public int MaxWeekendDaysLodgerCount = 3;

        public Calendar()
        {
            this.Reservations = [];
        }

        //arrival and departure of lodgers, create events for each arrival/departure.
        public void GetArrivalDepartureEvents()
        {
            //create a list of all events where a DEPARTURE take place
            //create a second list of all events where an ARRIVAL take place
        }


        public void GetBusyEvents()
        {
            //create a list of fulltime day events with a condition on the calendar setting to determine
            //if each day is full / busy / free
        }

    }
}
