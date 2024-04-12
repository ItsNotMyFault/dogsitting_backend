using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain
{
    public class Calendar
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Allow the user to configure if he wants to enable periods or block them.
        /// Can only choose once at a time.
        /// </summary>
        [NotMapped]
        public List<DateTimePeriod> UnavailablePeriods { get; set; }
        [NotMapped]
        public List<DateTimePeriod> AvailablePeriods { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<Reservation> Reservations { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public bool UseAvailabilities { get; set; }
        public bool UseUnavailabilities { get; set; }

        public int MaxWeekDaysLodgerCount = 1;
        public int MaxWeekendDaysLodgerCount = 3;
        private DateTimePeriod GeneralPeriod = new(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(12));

        public Calendar()
        {

            this.Reservations = [];
        }

        public List<CalendarEvent> GetDailyCalendarEvents()
        {
            List<CalendarEvent> events = [];
            foreach (DateTime datetime in this.GeneralPeriod.EachDay())
            {
                events.Add(new CalendarEvent(datetime));
            };
            return events;
        }

        //arrival and departure of lodgers, create events for each arrival/departure.
        public Object GetArrivalDepartureEvents()
        {
            List<DateTime> arrivalDates = this.Reservations.Select(reservation => reservation.DateFrom).Distinct().ToList();
            List<DateTime> departureDates = this.Reservations.Select(reservation => reservation.DateTo).Distinct().ToList();

            List<CalendarEvent> arrivalEvents = [];
            List<CalendarEvent> departureEvents = [];
            foreach (Reservation reservation in Reservations.Where(r => arrivalDates.Contains(r.DateFrom)))
            {
                arrivalEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (arrival)"));
            }

            foreach (Reservation reservation in Reservations.Where(r => departureDates.Contains(r.DateTo)))
            {
                departureEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (departure)"));
            }

            return null;

        }

        private List<CalendarEvent> BusyEvents = new List<CalendarEvent>();
        private List<DateTime> GetBusyDates()
        {
            return this.BusyEvents.Select(ev => ev.DateTimePeriod.StartDate).ToList();
        }
        public List<BusyCalendarEvent> GetBusyEvents()
        {

            List<BusyCalendarEvent> BusyEvents = new List<BusyCalendarEvent>();

            this.Reservations.ForEach(reservation =>
            {
                reservation.GetEvents().ForEach(ev =>
                {
                    if (!this.GetBusyDates().Contains(ev.DateTimePeriod.StartDate))
                    {
                        //adds a new date as "busy"
                        BusyEvents.Add(new BusyCalendarEvent(reservation, ev.DateTimePeriod.StartDate));
                    }
                    else
                    {
                        //add coresponding amount of lodger to existing
                        BusyCalendarEvent busyEvent = BusyEvents.Where(busyEvent => busyEvent.DateTimePeriod.StartDate == ev.DateTimePeriod.StartDate).First();
                        busyEvent.AddLodger(reservation.LodgerCount);
                    }
                });
            });
            BusyEvents.ForEach(ev => { ev.ComputeBusyness(this); });
            return BusyEvents;
            //create a list of fulltime day events with a condition on the calendar setting to determine
            //if each day is full / busy / free
        }

    }
}
