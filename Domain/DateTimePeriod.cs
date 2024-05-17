using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace dogsitting_backend.Domain
{
    [NotMapped]
    public class DateTimePeriod
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WeekNumber;

        public DateTimePeriod(string date)
        {
            var dateT = Convert.ToDateTime(date);
            this.StartDate = dateT;
            this.EndDate = dateT;
        }
        public DateTimePeriod(DateTime date)
        {
            var test1 = date.ToLocalTime();
            var test2 = date.ToUniversalTime();
            this.StartDate = date;
            this.EndDate = date;
        }
        public DateTimePeriod(DateTime StartDate, DateTime EndDate)
        {
            this.StartDate = StartDate;
            this.EndDate = EndDate;
        }
        public DateTimePeriod(string StartDate, string EndDate)
        {
            this.StartDate = Convert.ToDateTime(StartDate);
            this.EndDate = Convert.ToDateTime(EndDate);
        }

        public DateTimePeriod ToLocalTimezone()
        {
            return new DateTimePeriod(this.StartDate.ToLocalTime(), this.EndDate.ToLocalTime());
        }
        public override string ToString()
        {
            return this.StartDate.ToString("yyyy-MM-dd") + " au " + this.EndDate.ToString("yyyy-MM-dd");
        }
        public string StartDateToString()
        {
            return this.StartDate.ToString("yyyy-MM-dd");
        }
        public string EndDateToString()
        {
            return this.EndDate.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Should really only be used for single day purposes...
        /// startDate and endDate being the same value.
        /// </summary>
        public bool IsWeekend()
        {
            return this.StartDate.IsDayOfWeek(DayOfWeek.Saturday) ||
                this.StartDate.IsDayOfWeek(DayOfWeek.Sunday) ||
                this.EndDate.IsDayOfWeek(DayOfWeek.Saturday) ||
                this.EndDate.IsDayOfWeek(DayOfWeek.Saturday);
        }

        public IEnumerable<DateTimePeriod> GetPeriodsOf15Minutes()
        {
            //List<DateTimePeriod> asdf = new ();

            DateTime dateTimeTo = EndDate.AddTicks(-1);
            for (DateTime moment = this.StartDate; (moment.Minute < dateTimeTo.Minute) && moment <= dateTimeTo; moment = moment.AddMinutes(15))
                //asdf.Add(new DateTimePeriod(moment, moment.AddMinutes(15)));
                yield return new DateTimePeriod(moment, moment.AddMinutes(15).AddSeconds(-1));
        }


        public bool IsASingleFullDay()
        {
            bool startAndEndSameHour = this.StartDate.Hour == this.EndDate.Hour;
            bool endOnNextDay = this.StartDate.AddDays(1).Day == this.EndDate.Day;
            return (startAndEndSameHour && endOnNextDay);
        }
        public bool IsMultipleDays()
        {
            bool isfullday = this.IsASingleFullDay();
            bool doesntEndOnNextDay = this.StartDate.AddDays(1) < this.EndDate;
            return (!isfullday && doesntEndOnNextDay);
        }

        public IEnumerable<DateTime> EachDay()
        {
            for (DateTime day = this.StartDate.Date; day.Date <= this.EndDate.Date; day = day.AddDays(1))
                yield return day;
        }

        public int HourCount()
        {
            return this.EachHour().ToList().Count;
        }

        /// <summary>
        /// Get a range of hours in between 2 dates (does consider times)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="thru"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> EachHour()
        {
            DateTime dateTimeFrom = this.StartDate;
            DateTime dateTimeTo = DateTimeUtils.LastMinOfHour(this.EndDate);
            for (DateTime day = dateTimeFrom; (day.Hour < dateTimeTo.Hour) && day < dateTimeTo; day = day.AddHours(1))
                yield return day;
        }

        public bool IsDateInBetweenDates(DateTime dateToCheck)
        {
            bool isDateAfterStartDate = dateToCheck >= this.StartDate;
            bool isDateBeforeEndDate = dateToCheck <= this.EndDate;
            return isDateAfterStartDate && isDateBeforeEndDate;
        }

        public bool IsPeriodOverlappedByPeriod(DateTimePeriod checkedPeriod)
        {
            bool isCheckedPeriodStartingBefore = checkedPeriod.StartDate <= this.StartDate;
            bool isCheckedPeriodEndingLater = checkedPeriod.EndDate >= this.EndDate;
            return isCheckedPeriodStartingBefore && isCheckedPeriodEndingLater;
        }
        public bool IsSamePeriod(DateTimePeriod checkedPeriod)
        {
            bool isCheckedPeriodStartSame = checkedPeriod.StartDate == this.StartDate;
            bool isCheckedPeriodEndSame = checkedPeriod.EndDate == this.EndDate;
            return isCheckedPeriodStartSame && isCheckedPeriodEndSame;
        }

        public static int GetWeekNumber(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }


    }
}
