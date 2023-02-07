using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    /// <summary>
    /// Interface for holiday planner implementations.
    /// </summary>
    public interface IHolidayPlanner
    {
        /// <summary>
        /// Calculate the number of vacation that are consumed if one is on vacation in the 
        /// date period defined in <paramref name="dateRangeString"/>.
        /// </summary>
        /// <param name="dateRangeString"></param>
        /// <returns>The number of vacation days that have to be consumed.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="dateRangeString"/> is invalid.
        /// </exception>
        int CalculateSpentHolidays(string dateRangeString);

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
        int CalculateSpentHolidays(DateTime startDate, DateTime endDate);
    }
}
