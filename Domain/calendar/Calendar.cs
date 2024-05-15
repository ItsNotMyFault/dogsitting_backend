using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.calendar
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
        public List<CalendarEvent> DepartureEvents { get; set; } = new List<CalendarEvent>();
        public List<CalendarEvent> ArrivalEvents { get; set; } = new List<CalendarEvent>();

        //[Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        [ForeignKey("Team")]
        public Guid TeamId { get; set; }
        public virtual Team Team { get; set; }

        public Calendar() { }
        public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

        public bool UseAvailabilities { get; set; }
        public bool UseUnavailabilities { get; set; }

        public int MaxWeekDaysLodgerCount { get; set; }
        public int MaxWeekendDaysLodgerCount { get; set; }

        private DateTimePeriod GeneralPeriod = new(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(12));


        public List<CalendarEvent> GetDailyCalendarEvents()
        {
            List<CalendarEvent> events = [];
            foreach (DateTime datetime in GeneralPeriod.EachDay())
            {
                events.Add(new CalendarEvent(datetime));
            };
            return events;
        }

        //arrival and departure of lodgers, create events for each arrival/departure.
        public List<CalendarEvent> GetArrivalEvents()
        {
            List<DateTime> arrivalDates = Reservations.Select(reservation => reservation.DateFrom).Distinct().ToList();
            foreach (Reservation reservation in Reservations.Where(r => arrivalDates.Contains(r.DateFrom)))
            {
                ArrivalEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (arrival)", reservation.Period));
            }
            return ArrivalEvents;
        }

        public List<CalendarEvent> GetDepartureEvents()
        {
            List<DateTime> arrivalDates = Reservations.Select(reservation => reservation.DateFrom).Distinct().ToList();
            List<DateTime> departureDates = Reservations.Select(reservation => reservation.DateTo).Distinct().ToList();

            foreach (Reservation reservation in Reservations.Where(r => departureDates.Contains(r.DateTo)))
            {
                DepartureEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (departure)", reservation.Period));
            }

            return DepartureEvents;

        }



        public List<BusyCalendarEvent> GetComputedBusyEvents()
        {
            List<BusyCalendarEvent> BusyEvents = [];

            this.Reservations.ToList().ForEach(reservation =>
            {
                reservation.GetDailyEvents().ForEach(ev =>
                {
                    bool busyDayAlreadyExists = BusyEvents.Any(busyEve =>
                    {
                        return ev.DateTimePeriod.IsPeriodOverlappedByPeriod(busyEve.DateTimePeriod);
                    });
                    if (!busyDayAlreadyExists)
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

            //todo add validation if day already added to busyeventlist.
            this.Availabilities.ToList().ForEach(availability =>
            {
                bool busyDayAlreadyExists = BusyEvents.Any(busyEve =>
                {
                    return availability.Period.IsPeriodOverlappedByPeriod(busyEve.DateTimePeriod);
                });
                if (!busyDayAlreadyExists)
                {
                    BusyEvents.Add(new BusyCalendarEvent(availability));
                }
            });

            return BusyEvents;
            //create a list of fulltime day events with a condition on the calendar setting to determine
            //if each day is full / busy / free
        }

        public List<AvailableCalendarEvent> GetAvailableEvents()
        {
            List<AvailableCalendarEvent> AvailableEvents = [];
            this.Availabilities.ToList().ForEach(availability =>
            {
                AvailableEvents.Add(new AvailableCalendarEvent(availability));
            });
            return AvailableEvents;
        }

        public List<BusyCalendarEvent> GetBusyEvents()
        {

            List<BusyCalendarEvent> BusyEvents = [];
            this.Reservations.ToList().ForEach(reservation =>
            {
                reservation.GetDailyEvents().ForEach(ev =>
                {
                    bool busyDayAlreadyExists = BusyEvents.Any(busyEve =>
                    {
                        return ev.DateTimePeriod.IsPeriodOverlappedByPeriod(busyEve.DateTimePeriod);
                    });
                    if (!busyDayAlreadyExists)
                    {
                        //adds a new date as "busy"
                        BusyEvents.Add(new BusyCalendarEvent(reservation, ev.DateTimePeriod.StartDate, true));
                    }
                });
            });

            return BusyEvents;
        }

        public List<CalendarEvent> GetReservationsEvents()
        {
            List<CalendarEvent> reservationEvents = this.Reservations.Select(reservation => reservation.GetReservationEvent()).ToList();
            return reservationEvents;
        }

    }
}
