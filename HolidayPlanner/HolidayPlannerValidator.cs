using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public class HolidayPlannerValidator : IHolidayPlannerValidator
    {
        private const int MAX_PERIOD_LENGTH = 50;
        private const int HOLIDAY_PERIOD_START_MONTH = 4;
        private const int HOLIDAY_PERIOD_START_DAY = 1;

        private const string LENGTH_ERROR = "Period length is invalid";
        private const string CHORONOLOGY_ERROR = "End date < start date";
        private const string OVERLAP_ERROR = "Input period is on more than one holiday periods";



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
            if (endDate - startDate <= TimeSpan.FromDays(MAX_PERIOD_LENGTH))
                return null;
            return new ValidationError(LENGTH_ERROR);
        }

        private ValidationError? ValidatePeriodChronology(DateTime startDate, DateTime endDate)
        {
            if (endDate >= startDate)
                return null;
            return new ValidationError(CHORONOLOGY_ERROR);
        }

        private ValidationError? ValidateHolidayPeridOverlap(DateTime startDate, DateTime endDate)
        {
            // Should never happen if ValidatePeriodLength is called before this with the current requirements
            if (endDate.Year - startDate.Year > 1)
                return new ValidationError(OVERLAP_ERROR);

            // Should never happen if ValidatePeriodChronology is called before this
            if (startDate > endDate)
                throw new ArgumentException($"{nameof(startDate)} is greater than {nameof(endDate)}");

            List<DateTime> holidayPeriodDates = new() { new DateTime(startDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY) };
            if (endDate.Year != startDate.Year)
                holidayPeriodDates.Add(new DateTime(endDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY));

            if (holidayPeriodDates.Any(holidayPeriodDate => IsDateBetween(holidayPeriodDate, startDate, endDate)))
                return new ValidationError(OVERLAP_ERROR);

            return null;
        }

        private bool IsDateBetween(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck > startDate && dateToCheck < endDate;
        }
    }
}
