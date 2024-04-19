using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace dogsitting_backend.Domain.calendar
{
    [NotMapped]
    public class BusyCalendarEvent : CalendarEvent
    {
        private Guid _id;
        public int LodgerCount { get; set; }
        public bool IsBusy { get; set; }
        public bool IsFree { get; set; }
        public bool IsFull { get; set; }
        public BusyCalendarEvent(Reservation reservation, DateTime dateTime) : base(dateTime)
        {
            Id = reservation.Id;
            LodgerCount = reservation.LodgerCount;
            EventSubject = reservation.ReservationTitle;
        }

        /// <summary>
        /// To increment the amount of present lodgers to an existing event.
        /// </summary>
        /// <param name="count"></param>
        public void AddLodger(int count = 1)
        {
            LodgerCount += count;
            EventSubject = $"Occupé {LodgerCount}";

        }

        public void ComputeBusyness(Calendar calendar)
        {
            int maxLodgerCount = DateTimePeriod.IsWeekend() ? calendar.MaxWeekendDaysLodgerCount : calendar.MaxWeekDaysLodgerCount;
            SetBusyStatus(maxLodgerCount);
            //do other work with calendar
            if (calendar.UseUnavailabilities && calendar.UnavailablePeriods.Any(uPeriod => uPeriod.IsPeriodOverlappedByPeriod(DateTimePeriod)))
            {
                SetFull();
            }

            if (calendar.UseAvailabilities && calendar.AvailablePeriods.All(aPeriod => DateTimePeriod.IsPeriodOverlappedByPeriod(aPeriod)))
            {
                SetBusy();
            }

        }

        /// <summary>
        /// determine if lodger count is over, free or within margins
        /// </summary>
        /// <param name="maxLodgerCount"></param>
        private void SetBusyStatus(int maxLodgerCount)
        {
            if (LodgerCount == 0)
            {
                SetFree();
            }
            else if (LodgerCount > maxLodgerCount)
            {
                SetFull();
            }
            else
            {
                SetBusy();
            }
        }
        private void SetFree()
        {
            IsFree = true;
            IsBusy = false;
            IsFull = false;
        }

        private void SetBusy()
        {
            IsFree = false;
            IsBusy = true;
            IsFull = false;
        }

        private void SetFull()
        {
            IsFree = false;
            IsBusy = true;
            IsFull = true;
        }


    }
}
