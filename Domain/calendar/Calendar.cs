using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.calendar
{
    public class Calendar : DBModel
    {
        /// <summary>
        /// Allow the user to configure if he wants to enable periods or block them.
        /// Can only choose once at a time.
        /// </summary>
        [NotMapped]
        public List<DateTimePeriod> UnavailablePeriods
        {
            get
            {
                return this.Availabilities.Where(availability => !availability.IsAvailable).Select(availability => availability.Period).ToList();
            }
        }

        [NotMapped]
        public List<DateTimePeriod> AvailablePeriods
        {
            get
            {
                return this.Availabilities.Where(availability => availability.IsAvailable).Select(availability => availability.Period).ToList();
            }
        }
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
            }
            ;
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



        public List<ReservationEvent> GetComputedBusyEvents()
        {
            List<ReservationEvent> BusyEvents = [];

            this.Reservations.ToList().ForEach(reservation =>
            {

                var original = reservation.DateFrom;
                var test1 = reservation.DateFrom.ToUniversalTime();
                var test2 = reservation.DateFrom.ToLocalTime();

                reservation.GetDailyEvents().ForEach(ev =>
                {
                    bool busyDayAlreadyExists = BusyEvents.Any(busyEve =>
                    {
                        return ev.DateTimePeriod.IsPeriodOverlappedByPeriod(busyEve.DateTimePeriod);
                    });
                    if (!busyDayAlreadyExists)
                    {
                        //adds a new date as "busy"
                        BusyEvents.Add(new ReservationEvent(reservation, ev.DateTimePeriod));
                    }
                    else
                    {
                        //add coresponding amount of lodger to existing
                        ReservationEvent busyEvent = BusyEvents.Where(busyEvent => busyEvent.DateTimePeriod.StartDate == ev.DateTimePeriod.StartDate).First();
                        busyEvent.AddLodger(reservation.LodgerCount);
                    }
                });
            });
            BusyEvents.ForEach(ev => { ev.ComputeBusyness(this); });


            this.Availabilities.ToList().ForEach(availability =>
            {
                bool busyDayAlreadyExists = BusyEvents.Any(busyEve =>
                {
                    return availability.Period.ToLocalTimezone().IsPeriodOverlappedByPeriod(busyEve.DateTimePeriod.ToLocalTimezone());
                });
                if (!busyDayAlreadyExists)
                {
                    BusyEvents.Add(new ReservationEvent(availability));
                }
            });

            return BusyEvents;
        }

        public void ValidateReservation(Reservation reservation)
        {
            //validate count.
            int currentCount = this.GetLodgerCountForPeriod(reservation.Period);
            bool isNewCountValid = (currentCount + reservation.LodgerCount) <= this.MaxWeekDaysLodgerCount;
            if (!isNewCountValid)
            {
                throw new Exception("Reservation has too big of a lodgerCount for current calendar team's settings.");
            }

            //validate availabilities
            //TODO: add validation according to settings => if(this.UseUnavailabilities)

            this.UnavailablePeriods.ForEach(unavPeriod =>
            {
                if (unavPeriod.IsPeriodOverlappedByPeriod(reservation.Period))
                {
                    throw new Exception("Reservation can't be created on an Unavailable date");
                }
            });
        }

        public int GetLodgerCountForPeriod(DateTimePeriod period)
        {
            //this.Reservations.ev
            List<Reservation> reservationsInPeriod = this.Reservations.ToList().Where(resrv => resrv.Period.IsPeriodOverlappedByPeriod(period)).ToList();
            return reservationsInPeriod.Sum(resrv => resrv.LodgerCount);
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

        public List<ReservationEvent> GetBusyEvents()
        {

            List<ReservationEvent> BusyEvents = [];
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
                        BusyEvents.Add(new ReservationEvent(reservation, ev.DateTimePeriod, true));
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
