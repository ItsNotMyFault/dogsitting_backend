using System.Globalization;

namespace dogsitting_backend.Domain
{
    public static class DateTimeUtils
    {
        //datetimenow
        public static string GetDateTimeNowAsString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateNowAsString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }

        //weeknumber
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
            int weekNumber = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }


        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        /// Get a range of hours in between 2 FULL dates (doesn't consider times)
        /// the month/year specified is important if it overlaps over months or years.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="thru"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachHour(DateTime from, DateTime thru)
        {
            DateTime dateTimeFrom = DateTimeUtils.StartOfDay(from);
            DateTime dateTimeTo = DateTimeUtils.EndOfDay(thru);
            for (DateTime day = dateTimeFrom; (day.Day <= dateTimeTo.Day || day.Month <= dateTimeTo.Month || day.Year <= dateTimeTo.Year) && day < dateTimeTo; day = day.AddHours(1))
                yield return day;
        }

        public static DateTime GetLastDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime GetFirstDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static void GetPreviousPerdiodDaterangeAsDateTime(DateTime dateFrom, DateTime dateTo, out DateTime previousDateFrom, out DateTime previousDateTo)
        {
            GetWeekNumberFromDaterange(dateFrom, dateTo, out int fromWeekNumber, out int toWeekNumber);
            int weekNumberDiff;
            if (fromWeekNumber > toWeekNumber)
            {
                //52 - 5
                int weekDiff = 52 - (fromWeekNumber - toWeekNumber);
                weekNumberDiff = weekDiff;
            }
            else
            {
                weekNumberDiff = (toWeekNumber - fromWeekNumber) + 1;
            }
            previousDateFrom = RemoveXWeeksToDateTime(dateFrom, weekNumberDiff);
            previousDateTo = RemoveXWeeksToDateTime(dateTo, weekNumberDiff);
        }

        /// <summary>
        /// Retrieve weeknumbers for dateFrom AND dateTo
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="fromWeekNumber"></param>
        /// <param name="toWeekNumber"></param>
        public static void GetWeekNumberFromDaterange(DateTime timeFrom, DateTime timeTo, out int fromWeekNumber, out int toWeekNumber)
        {
            int calculatedWeekNumberDateFrom = GetWeekNumber(timeFrom);
            int calculatedWeekNumberDateTo = GetWeekNumber(timeTo);
            if (calculatedWeekNumberDateTo - calculatedWeekNumberDateFrom == 0)
            {
                fromWeekNumber = calculatedWeekNumberDateFrom;
                toWeekNumber = calculatedWeekNumberDateFrom;
            }
            else if (calculatedWeekNumberDateTo - calculatedWeekNumberDateFrom > 0)
            {
                fromWeekNumber = calculatedWeekNumberDateFrom;
                toWeekNumber = calculatedWeekNumberDateTo;
            }
            else if (calculatedWeekNumberDateTo - calculatedWeekNumberDateFrom < 0)
            {
                fromWeekNumber = calculatedWeekNumberDateFrom;
                toWeekNumber = calculatedWeekNumberDateTo;
            }
            else { throw new ArgumentException("GetWeekNumberFromDaterange dates are inverted."); }//or crossing year....
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime firstThursday = GetFirstThursday(year);
            DateTime result = GetYearWeekNumber(firstThursday, weekOfYear);
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        public static DateTime LastDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime firstThursday = GetFirstThursday(year);
            DateTime result = GetYearWeekNumber(firstThursday, weekOfYear);
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(3);
        }

        /// <summary>
        /// Retrieve thursday date of the week number X.
        /// Found from the first thursday of the expected year.
        /// </summary>
        /// <param name="firstThursday"> first thursday of the expected year</param>
        /// <param name="weekOfYear">thursday date of expected week of the year (year is determined by firstthursday).</param>
        /// <returns></returns>
        private static DateTime GetYearWeekNumber(DateTime firstThursday, int weekOfYear)
        {
            System.Globalization.Calendar cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            DateTime result = firstThursday.AddDays(weekNum * 7);
            return result;
        }

        private static DateTime GetFirstThursday(int year)
        {
            DateTime jan1 = new(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            return firstThursday;
        }


        public static DateTime StartOfDay(this DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
            return dt;
        }

        public static string StartOfDayAsString(this DateTime dt)
        {
            dt = StartOfDay(dt);
            return ConvertDateTimeToString(dt);
        }

        public static string EndOfDayAsString(this DateTime dt)
        {
            dt = EndOfDay(dt);
            return ConvertDateTimeToString(dt);
        }
        public static DateTime EndOfDay(this DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
            return dt;
        }


        public static DateTime ConvertToDateTime(string date)
        {
            return Convert.ToDateTime(date);
        }

        public static string ConvertDateTimeToString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime LastMinOfHour(this DateTime dt)
        {
            return dt.AddHours(1).AddTicks(-1);
        }

        public static string ConvertDateToString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static DateTime RemoveXDaysToDatetTime(this DateTime dt, int dayCount)
        {
            return dt.AddDays(-dayCount);
        }
        public static DateTime RemoveXWeeksToDateTime(this DateTime dt, int weekCount)
        {
            return dt.AddDays(-(weekCount * 7));
        }

        public static double GetDayCountBetweenDateRange(DateTime dateFrom, DateTime dateTo)
        {
            double totalDaysBetweenDates = (dateTo - dateFrom).TotalDays;
            return totalDaysBetweenDates;
        }

        public static double GetDayCountBetweenDateRange(string dateFrom, string dateTo)
        {
            DateTime dateTimeFrom = Convert.ToDateTime(dateFrom);
            DateTime dateTimeTo = Convert.ToDateTime(dateTo);
            double totalDaysBetweenDates = (dateTimeTo - dateTimeFrom).TotalDays;
            return totalDaysBetweenDates;
        }






        /// <summary>
        /// Add X day(s) to date
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dayCount"></param>
        /// <returns></returns>
        public static DateTime AddXDaysToDatetTime(this DateTime dt, int dayCount)
        {
            return dt.AddDays(dayCount);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">date from where method operates</param>
        /// <param name="yearCount">minus X year</param>
        /// <returns>Beginning of year</returns>
        public static DateTime BegginningPastXYear(this DateTime dt, int yearCount)
        {
            DateTime dateTime = dt.AddYears(-yearCount);
            DateTime startDate = new(dateTime.Year, 1, 1);
            return startDate;
        }

        public static bool IsDayOfWeek(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.DayOfWeek.Equals(dayOfWeek);
        }
    }
}
