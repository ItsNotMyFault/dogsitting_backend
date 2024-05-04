﻿using Microsoft.Extensions.Logging;
using System;
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

        //[Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [ForeignKey("Team")]
        public Guid TeamId { get; set; }
        public virtual Team Team { get; set; }

        public bool UseAvailabilities { get; set; }
        public bool UseUnavailabilities { get; set; }

        public int MaxWeekDaysLodgerCount = 1;
        public int MaxWeekendDaysLodgerCount = 3;

        private DateTimePeriod GeneralPeriod = new(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(12));

        public Calendar()
        {

            Reservations = [];
        }

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
                ArrivalEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (arrival)"));
            }
            return ArrivalEvents;
        }

        public List<CalendarEvent> GetDepartureEvents()
        {
            List<DateTime> arrivalDates = Reservations.Select(reservation => reservation.DateFrom).Distinct().ToList();
            List<DateTime> departureDates = Reservations.Select(reservation => reservation.DateTo).Distinct().ToList();

            foreach (Reservation reservation in Reservations.Where(r => departureDates.Contains(r.DateTo)))
            {
                DepartureEvents.Add(new CalendarEvent($"{reservation.ReservationTitle} (departure)"));
            }

            return DepartureEvents;

        }



        public List<BusyCalendarEvent> BusyEvents { get; set; } = new List<BusyCalendarEvent>();
        public List<CalendarEvent> DepartureEvents { get; set; } = new List<CalendarEvent>();
        public List<CalendarEvent> ArrivalEvents { get; set; } = new List<CalendarEvent>();

        public List<BusyCalendarEvent> GetBusyEvents()
        {

            List<BusyCalendarEvent> BusyEvents = [];

            Reservations.ToList().ForEach(reservation =>
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
            this.BusyEvents = BusyEvents;
            return BusyEvents;
            //create a list of fulltime day events with a condition on the calendar setting to determine
            //if each day is full / busy / free
        }

        public List<CalendarEvent> GetReservationsEvents()
        {
            List<CalendarEvent> reservationEvents = Reservations.Select(reservation => reservation.GetDateRangeEvent()).ToList();
            return reservationEvents;
        }

    }
}
