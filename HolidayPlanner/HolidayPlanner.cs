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


        public HolidayPlanner(INationalHolidayProvider nationalHolidayProvider, IHolidayPlannerValidator validator)
        {
            _nationalHolidayProvider = nationalHolidayProvider;
            _validator = validator;
        }

        /// <summary>
        /// Calculate the number of vacation that are consumed if one is on vacation in the 
        /// period between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">First date of the period (included as a vacation day).</param>
        /// <param name="endDate">Final date of the period (included as a vacation day).</param>
        /// <returns>The number of vacation days that have to be consumed.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="startDate"/> or <paramref name="endDate"/> is invalid.
        /// </exception>
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
