using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    /// <summary>
    /// Interface for classes that can get national holidays for a certain locale.
    /// </summary>
    public interface INationalHolidayProvider
    {
        /// <summary>
        /// Locale of the implementing class.
        /// </summary>
        string Locale { get; }

        /// <summary>
        /// Gets a list of national holidays between <paramref name="startDate"/> and 
        /// <paramref name="endDate"/> for a particular locale.
        /// </summary>
        /// <param name="startDate">Start date of the period.</param>
        /// <param name="endDate">End date of the period.</param>
        /// <returns>
        /// List of national holidays in period between 
        /// <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if parameters <paramref name="startDate"/> and/or <paramref name="endDate"/> are invalid.
        /// </exception>
        List<DateTime> GetNationalHolidaysBetween(DateTime startDate, DateTime endDate);
    }
}
