using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public class HolidayPlannerValidator : IHolidayPlannerValidator
    {
        private const int MAX_PERIOD_LENGTH_DAYS = 50;
        private const int HOLIDAY_PERIOD_START_MONTH = 4;
        private const int HOLIDAY_PERIOD_START_DAY = 1;

        private const string LENGTH_ERROR = "Period length of {0} days greater than the maximum allowed {1} days";
        private const string CHORONOLOGY_ERROR = "Start date is greater than end date ({0} < {1})";
        private const string OVERLAP_ERROR = "Input period from {0} to {1} is on more than one holiday periods";



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

            if (holidayPeriodDates.Any(holidayPeriodDate => IsDateBetween(holidayPeriodDate, startDate, endDate)))
                return new ValidationError(string.Format(OVERLAP_ERROR, startDate, endDate));

            return null;
        }

        private bool IsDateBetween(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck > startDate && dateToCheck < endDate;
        }
    }
}
