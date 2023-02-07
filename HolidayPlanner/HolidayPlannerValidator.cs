using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public class HolidayPlannerValidator : IHolidayPlannerValidator
    {
        // These values could be public properties in a more configurable implementation
        private const int MAX_PERIOD_LENGTH_DAYS = 50;
        private const int HOLIDAY_PERIOD_START_MONTH = 4;
        private const int HOLIDAY_PERIOD_START_DAY = 1;

        private const string LENGTH_ERROR = "Period length of {0} days greater than the maximum allowed {1} days";
        private const string CHORONOLOGY_ERROR = "Start date is greater than end date ({0} < {1})";
        private const string OVERLAP_ERROR = "Input period from {0} to {1} is on more than one holiday periods";


        /// <summary>
        /// Validates the holiday period from <paramref name="startDate"/> to <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">Start date of the period.</param>
        /// <param name="endDate">End date of the period.</param>
        /// <remarks>
        /// Parameters <paramref name="startDate"/> and <paramref name="endDate"/> are valid if
        /// <list type="bullet">
        /// <item>The timespan from <paramref name="startDate"/> to <paramref name="endDate"/> is at most 50 days.</item>
        /// <item><paramref name="startDate"/> is not greater than <paramref name="endDate"/>.</item>
        /// <item>The period from <paramref name="startDate"/> to <paramref name="endDate"/> does not overlap 
        /// mulitple holiday periods; holiday periods are one year long from 1st of April to 31st of March next year.</item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <see cref="ValidationResult"/> object where <see cref="ValidationResult.Status"/> 
        /// is <see cref="ValidationStatus.Success"/> if there were no validation errors.
        /// </returns>
        public ValidationResult ValidateHolidayPeriod(DateTime startDate, DateTime endDate)
        {
            ValidationResult result = new();

            var periodLengthValidationError = ValidatePeriodLength(startDate, endDate);
            if (periodLengthValidationError != null)
                result.AddErrors(periodLengthValidationError);

            var chronologyValidationError = ValidatePeriodChronology(startDate, endDate);
            if (chronologyValidationError != null)
                result.AddErrors(chronologyValidationError);

            var overlapValidationError = ValidateHolidayPeridOverlap(startDate, endDate);
            if (overlapValidationError != null)
                result.AddErrors(overlapValidationError);

            return result;
        }



        private ValidationError? ValidatePeriodLength(DateTime startDate, DateTime endDate)
        {
            var periodLength = endDate - startDate;
            var maxPeriod = TimeSpan.FromDays(MAX_PERIOD_LENGTH_DAYS);

            if (periodLength <= maxPeriod)
                return null;
            return new ValidationError(string.Format(LENGTH_ERROR, periodLength.TotalDays, MAX_PERIOD_LENGTH_DAYS));
        }

        private ValidationError? ValidatePeriodChronology(DateTime startDate, DateTime endDate)
        {
            if (endDate >= startDate)
                return null;
            return new ValidationError(string.Format(CHORONOLOGY_ERROR, startDate, endDate));
        }

        private ValidationError? ValidateHolidayPeridOverlap(DateTime startDate, DateTime endDate)
        {
            // Should never happen if ValidatePeriodLength is called before this with the current requirements
            if (endDate.Year - startDate.Year > 1)
                return new ValidationError(string.Format(OVERLAP_ERROR, startDate, endDate));

            // Should never happen if ValidatePeriodChronology is called before this
            if (startDate > endDate)
                throw new ArgumentException($"{nameof(startDate)} is greater than {nameof(endDate)}");

            List<DateTime> holidayPeriodDates = new() { new DateTime(startDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY) };
            if (endDate.Year != startDate.Year)
                holidayPeriodDates.Add(new DateTime(endDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY));

            if (holidayPeriodDates.Any(holidayPeriodDate => DoesDateOverlapRange(holidayPeriodDate, startDate, endDate)))
                return new ValidationError(string.Format(OVERLAP_ERROR, startDate, endDate));

            return null;
        }

        private bool DoesDateOverlapRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            // Note: startDate is not inclusive here but endDate is.
            // This is because this is used to check if date range 
            // from startDate to endDate goes over dateToCheck. 
            // Therefore, we return true only if startDate < endDate
            // and dateToCheck is between them. I.e. return false 
            // if all three DateTime objects have the same date value.
            return dateToCheck > startDate && dateToCheck <= endDate;
        }
    }
}
