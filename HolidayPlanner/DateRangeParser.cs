using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    /// <summary>
    /// Class for parsing a date range, i.e. two dates.
    /// </summary>
    public class DateRangeParser
    {
        /// <summary>
        /// Exact format for the date time. Optional.
        /// </summary>
        public string? DateFormat { get; set; }

        /// <summary>
        /// Format provider used in parsing. Optional. 
        /// Default value is <see cref="DateTimeFormatInfo.CurrentInfo"/>.
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = DateTimeFormatInfo.CurrentInfo;

        /// <summary>
        /// Formatting options used in parsing. Optional. 
        /// Default value is <see cref="DateTimeStyles.None"/>.
        /// </summary>
        public DateTimeStyles DateStyles { get; set; } = DateTimeStyles.None;

        /// <summary>
        /// Separator string(s) for the separator of the two datetimes in the range.
        /// </summary>
        public IReadOnlyCollection<string> RangeSeparators { get => _rangeSeparators.ToList(); }
        private readonly List<string> _rangeSeparators = new();


        /// <summary>
        /// Initializes a new instance of <see cref="DateRangeParser"/>.
        /// </summary>
        /// <param name="rangeSeparators">
        /// Separator string(s). Must not be null/empty or contain null/empty strings. 
        /// For example "-".
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="rangeSeparators"/> is empty or contains null/empty strings.
        /// </exception>
        public DateRangeParser(params string[] rangeSeparators)
        {
            if (!rangeSeparators.Any())
                throw new ArgumentException($"{nameof(rangeSeparators)} is empty");
            if (rangeSeparators.Any(string.IsNullOrEmpty))
                throw new ArgumentException($"{nameof(rangeSeparators)} contains null or empty items");

            _rangeSeparators.AddRange(rangeSeparators);
        }

        /// <summary>
        /// Parses a date range (i.e. two dates) from <paramref name="input"/>.
        /// </summary>
        /// <param name="input">String that contains the date range to parse.</param>
        /// <returns>A tuple with the first and second date time parsed from <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">
        /// Thrown if <paramref name="input"/> is in an invalid format and cannot be parsed.
        /// </exception>
        public virtual (DateTime, DateTime) Parse(string input)
        {
            var splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var parts = input.Split(_rangeSeparators.ToArray(), splitOptions);
            var genericErrorStr = $"{nameof(input)} \"{input}\" is invalid";

            if (parts.Length != 2)
                throw new FormatException(genericErrorStr);

            try
            {
                return (ParseToDateTime(parts[0]), ParseToDateTime(parts[1]));
            }
            catch (ArgumentException ex) // Could be thrown by DateTime.TryParse or DateTime.TryParseExact
            {
                throw new FormatException(genericErrorStr, ex);
            }
            catch (NotSupportedException ex) // Could be thrown by DateTime.TryParse
            {
                throw new FormatException(genericErrorStr, ex);
            }
        }


        private DateTime ParseToDateTime(string dateStr)
        {
            bool success = string.IsNullOrEmpty(DateFormat)
                ? DateTime.TryParse(dateStr, FormatProvider, DateStyles, out DateTime result)
                : DateTime.TryParseExact(dateStr, DateFormat, FormatProvider, DateStyles, out result);

            return success
                ? result
                : throw new FormatException($"String \"{dateStr}\" cannot be parsed into a {nameof(DateTime)} object");
        }
    }
}
