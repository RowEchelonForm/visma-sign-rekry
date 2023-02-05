using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visma_sign_rekry
{
    /// <summary>
    /// Class for getting Finnish national holidays.
    /// </summary>
    public class FinnishNationalHolidayProvider : INationalHolidayProvider
    {
        /// <summary>
        /// Current locale.
        /// </summary>
        public string Locale { get => "FI"; }

        private static readonly Dictionary<int, List<DateTime>> _holidaysPerYear = new();


        // Note: A proper implementation would either have a longer listing of all Good Fridays for like 100 years 
        // or a proper calculation. In order to keep this implementation simple, that calculation isn't done.
        private readonly Dictionary<int, Tuple<int, int>> _yearlyGoodFridays = new()
        {
            { 2020, new(4, 10) },
            { 2021, new(4, 2) },
            { 2022, new(4, 15) },
            { 2023, new(4, 7) },
            { 2024, new(3, 29) },
        };


        /// <summary>
        /// Gets a list of Finnish national holidays between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">Start date of the period.</param>
        /// <param name="endDate">End date of the period.</param>
        /// <returns>List of national holidays in period between <paramref name="startDate"/> and <paramref name="endDate"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="startDate"/> is greater than <paramref name="endDate"/> OR 
        /// the period between <paramref name="startDate"/> and <paramref name="endDate"/> is in a range that is not supported.
        /// </exception>
        public List<DateTime> GetNationalHolidaysBetween(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException($"{nameof(startDate)} is greater than {nameof(endDate)}");

            var holidays = new List<DateTime>();
            var yearsToCheck = GetYearsBetween(startDate, endDate);
            foreach (int year in yearsToCheck)
            {
                var yearlyHolidays = GetHolidaysForYear(year)
                    .Where(holiday => (holiday >= startDate) && (holiday <= endDate));
                holidays.AddRange(yearlyHolidays);
            }
            return holidays;
        }


        private List<int> GetYearsBetween(DateTime startDate, DateTime endDate)
        {
            int year = startDate.Year;
            List<int> years = new() { year };
            while (year < endDate.Year)
            {
                year++;
                years.Add(year);
            }
            return years;
        }

        private List<DateTime> GetHolidaysForYear(int year)
        {
            if (_holidaysPerYear.TryGetValue(year, out var holidays))
                return holidays;


            holidays = CalculateHolidaysForYear(year);
            _holidaysPerYear[year] = holidays;
            return holidays.ToList();
        }

        private List<DateTime> CalculateHolidaysForYear(int year)
        {
            List<DateTime> holidays = new()
            {
                new(year, 1, 1),    // New Year's
                new(year, 1, 6),    // Epiphany
                new(year, 5, 1),    // 1st of May
                new(year, 12, 6),   // Independance Day
                new(year, 12, 24),  // Christmas Eve
                new(year, 12, 25),  // Christmas Day
                new(year, 12, 26),  // Boxing Day
            };

            holidays.AddRange(GetEasterRelatedHolidays(year));
            holidays.AddRange(GetMidsummerRelatedHolidays(year));
            holidays.Add(GetAllSaintsDay(year));

            holidays.Sort();
            return holidays;
        }

        private List<DateTime> GetEasterRelatedHolidays(int year)
        {
            if (!_yearlyGoodFridays.TryGetValue(year, out var goodFridayMonthAndDay))
                throw new ArgumentException($"Year {year} is not supported for easter holiday calculation");

            DateTime goodFriday = new(year, goodFridayMonthAndDay.Item1, goodFridayMonthAndDay.Item2);
            DateTime easterSunday = goodFriday.AddDays(2); // Always Sunday so technically unnecessary to add
            DateTime easterMonday = goodFriday.AddDays(3);
            DateTime feastOfAscension = goodFriday.AddDays(41);
            DateTime pentecost = goodFriday.AddDays(51); // Always Sunday so technically unnecessary to add

            return new List<DateTime>() { goodFriday, easterSunday, easterMonday, feastOfAscension, pentecost };
        }

        private List<DateTime> GetMidsummerRelatedHolidays(int year)
        {
            // Midsummer eve is the Friday between 19th and 25th of June
            // Midsummer day is the Saturday after that
            
            var dateToCheck = new DateTime(year, 6, 19);
            while (dateToCheck.DayOfWeek != DayOfWeek.Friday)
                dateToCheck = dateToCheck.AddDays(1);
            return new List<DateTime>() { dateToCheck, dateToCheck.AddDays(1), };
        }

        private DateTime GetAllSaintsDay(int year)
        {
            // All Saints' Day is the Saturday between October 31st and November 6th

            var dateToCheck = new DateTime(year, 10, 31);
            while (dateToCheck.DayOfWeek != DayOfWeek.Saturday)
                dateToCheck = dateToCheck.AddDays(1);
            return dateToCheck;
        }
    }
}
