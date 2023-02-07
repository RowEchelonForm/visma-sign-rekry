using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    /// <summary>
    /// Class for planning holidays and calculating how many vacation days get spent.
    /// </summary>
    public class HolidayPlanner : IHolidayPlanner
    {
        private readonly INationalHolidayProvider _nationalHolidayProvider;
        private readonly IHolidayPlannerValidator _validator;
        private readonly DateRangeParser _parser;


        public HolidayPlanner(
            INationalHolidayProvider nationalHolidayProvider, 
            IHolidayPlannerValidator validator,
            DateRangeParser parser)
        {
            _nationalHolidayProvider = nationalHolidayProvider;
            _validator = validator;
            _parser = parser;
        }

        public int CalculateSpentHolidays(string dateRangeString)
        {
            DateTime startDate, endDate;
            try
            {
                (startDate, endDate) = _parser.Parse(dateRangeString);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException($"{nameof(dateRangeString)} \"{dateRangeString}\" is invalid", ex);
            }

            return CalculateSpentHolidays(startDate, endDate);
        }

        public int CalculateSpentHolidays(DateTime startDate, DateTime endDate)
        {
            // Strip potential time data out because it's irrelevant and could might cause issues
            startDate = startDate.Date;
            endDate = endDate.Date;

            var validationResult = _validator.ValidateHolidayPeriod(startDate, endDate);
            if (validationResult.Status != ValidationStatus.Success)
            {
                // Note: in a proper appliction we would likely want to have custom exception. 
                // That is not done here for the sake of simplicity and time.
                throw new ArgumentException($"Validation failed for {nameof(startDate)} and/or {nameof(endDate)}:" +
                    $"{Environment.NewLine}{string.Join(Environment.NewLine, validationResult.GetErrorMessages())}");
            }

            // Note: +1 because both startDate and endDate are included in the period but time components are zero on both
            var dayCount = (int)(endDate - startDate).TotalDays + 1;

            var sundayCount = GetSundayCountInPeriod(startDate, endDate);
            var nationalHolidays = _nationalHolidayProvider.GetNationalHolidaysBetween(startDate, endDate);
            var nonSundayHolidayCount = nationalHolidays.Count(holiday => holiday.DayOfWeek != DayOfWeek.Sunday);

            return dayCount - sundayCount - nonSundayHolidayCount;
        }


        private int GetSundayCountInPeriod(DateTime startDate, DateTime endDate)
        {
            while (startDate.DayOfWeek != DayOfWeek.Sunday)
            {
                if (startDate >= endDate)
                    return 0;
                startDate = startDate.AddDays(1);
            }
            
            var sundaysBetween = (int)Math.Floor((endDate - startDate).TotalDays / 7) + 1;
            return sundaysBetween;
        }
    }
}
