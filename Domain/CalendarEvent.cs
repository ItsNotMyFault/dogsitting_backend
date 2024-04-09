namespace dogsitting_backend.Domain
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public DateTimePeriod DateTimePeriod { get; set; }

        public string EventSubject { get; set; }
        public string EventLocation { get; set; }
        public bool IsAllDayEvent { get; set; }
        public CalendarEvent()
        {
            this.Id = Guid.NewGuid();
        }
        public CalendarEvent(DateTimePeriod DateTimePeriod, FreeBusyStatus EventStatus)
        {
            this.Id = Guid.NewGuid();
            SetIsPeriodAllDay();
        }

        private void SetIsPeriodAllDay()
        {
            this.IsAllDayEvent = this.DateTimePeriod.IsASingleFullDay() || this.DateTimePeriod.IsMultipleDays();
        }


        public bool IsMomentInPeriod(DateTimePeriod PeriodToCheck)
        {
            bool isStartMomentInPeriod = false;
            bool isEndMomentInPeriod = false;
            bool isOverlapping = false;
            try
            {
                isStartMomentInPeriod = this.DateTimePeriod.IsDateInBetweenDates(PeriodToCheck.StartDate);
                isEndMomentInPeriod = this.DateTimePeriod.IsDateInBetweenDates(PeriodToCheck.EndDate);
                isOverlapping = this.DateTimePeriod.IsPeriodOverlappedByPeriod(PeriodToCheck);
            }
            catch (Exception)
            {
            }
            return (isStartMomentInPeriod || isEndMomentInPeriod || isOverlapping);
        }
    }
}
