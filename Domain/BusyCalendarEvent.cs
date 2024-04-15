using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace dogsitting_backend.Domain
{
    [NotMapped]
    public class BusyCalendarEvent : CalendarEvent
    {
        private Guid _id;
        private int LodgerCount { get; set; }
        private bool IsBusy { get; set; }
        private bool IsFree { get; set; }
        private bool IsFull { get; set; }
        public BusyCalendarEvent(Reservation reservation, DateTime dateTime) : base(dateTime)
        {
            this.Id = reservation.Id;
            this.LodgerCount = reservation.LodgerCount;
            this.EventSubject = reservation.ReservationTitle;
        }

        /// <summary>
        /// To increment the amount of present lodgers to an existing event.
        /// </summary>
        /// <param name="count"></param>
        public void AddLodger(int count = 1)
        {
            this.LodgerCount += count;
            this.EventSubject = $"Occupé {this.LodgerCount}";

        }

        public void ComputeBusyness(Calendar calendar)
        {
            int maxLodgerCount = this.DateTimePeriod.IsWeekend() ? calendar.MaxWeekendDaysLodgerCount : calendar.MaxWeekDaysLodgerCount;
            this.SetBusyStatus(maxLodgerCount);
            //do other work with calendar
            if (calendar.UseUnavailabilities && calendar.UnavailablePeriods.Any(uPeriod => uPeriod.IsPeriodOverlappedByPeriod(this.DateTimePeriod)))
            {
                this.SetFull();
            }

            if (calendar.UseAvailabilities && calendar.AvailablePeriods.All(aPeriod => this.DateTimePeriod.IsPeriodOverlappedByPeriod(aPeriod)))
            {
                this.SetBusy();
            }

        }

        /// <summary>
        /// determine if lodger count is over, free or within margins
        /// </summary>
        /// <param name="maxLodgerCount"></param>
        private void SetBusyStatus(int maxLodgerCount)
        {
            if (this.LodgerCount == 0)
            {
                this.SetFree();
            }
            else if (this.LodgerCount > maxLodgerCount)
            {
                this.SetFull();
            }
            else
            {
                this.SetBusy();
            }
        }
        private void SetFree()
        {
            this.IsFree = true;
            this.IsBusy = false;
            this.IsFull = false;
        }

        private void SetBusy()
        {
            this.IsFree = false;
            this.IsBusy = true;
            this.IsFull = false;
        }

        private void SetFull()
        {
            this.IsFree = false;
            this.IsBusy = true;
            this.IsFull = true;
        }


    }
}
