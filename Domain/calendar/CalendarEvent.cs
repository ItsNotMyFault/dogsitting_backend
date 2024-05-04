﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.calendar
{
    [NotMapped]
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public DateTimePeriod DateTimePeriod { get; set; }

        public string EventSubject { get; set; }
        public string EventLocation { get; set; }
        public bool IsAllDayEvent { get; set; }
        public CalendarEvent(string eventSubject)
        {
            EventSubject = eventSubject;
            Id = Guid.NewGuid();
        }

        public CalendarEvent(DateTime dateTime)
        {
            DateTimePeriod = new DateTimePeriod(dateTime);
            Id = Guid.NewGuid();
            SetIsPeriodAllDay();
        }

        public CalendarEvent(Guid reservationId, DateTimePeriod DateTimePeriod, string eventSubject)
        {
            this.EventSubject = eventSubject;
            this.IsAllDayEvent = true;
            this.DateTimePeriod = DateTimePeriod;
            this.ReservationId = reservationId;
            this.Id = Guid.NewGuid();
        }

        private void SetIsPeriodAllDay()
        {
            IsAllDayEvent = DateTimePeriod.IsASingleFullDay() || DateTimePeriod.IsMultipleDays();
        }

        public override string ToString()
        {
            return $"{EventSubject} {DateTimePeriod.StartDate.ToString("yyyy-MM-dd")}";
        }


        public bool IsMomentInPeriod(DateTimePeriod PeriodToCheck)
        {
            bool isStartMomentInPeriod = false;
            bool isEndMomentInPeriod = false;
            bool isOverlapping = false;
            try
            {
                isStartMomentInPeriod = DateTimePeriod.IsDateInBetweenDates(PeriodToCheck.StartDate);
                isEndMomentInPeriod = DateTimePeriod.IsDateInBetweenDates(PeriodToCheck.EndDate);
                isOverlapping = DateTimePeriod.IsPeriodOverlappedByPeriod(PeriodToCheck);
            }
            catch (Exception)
            {
            }
            return isStartMomentInPeriod || isEndMomentInPeriod || isOverlapping;
        }
    }
}
