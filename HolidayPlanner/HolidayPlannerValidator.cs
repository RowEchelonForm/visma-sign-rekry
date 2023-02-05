using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public class HolidayPlannerValidator // should there be an interface?
    {
        private const int MAX_PERIOD_LENGTH = 50;
        private const int HOLIDAY_PERIOD_START_MONTH = 4;
        private const int HOLIDAY_PERIOD_START_DAY = 1;

        private const string LENGTH_ERROR = "Period length is invalid";
        private const string CHORONOLOGY_ERROR = "End date < start date";
        private const string OVERLAP_ERROR = "Input period is on more than one holiday periods";



        public List<ValidationResult> ValidateHolidayPeriod(DateTime startDate, DateTime endDate)
        {
            var result = new List<ValidationResult>();

            var periodLengthValidationResult = ValidatePeriodLength(startDate, endDate);
            if (periodLengthValidationResult.Status == ValidationStatus.Error)
                result.Add(periodLengthValidationResult);

            var chronologyValidationResult = ValidatePeriodChronology(startDate, endDate);
            if (chronologyValidationResult.Status == ValidationStatus.Error)
                result.Add(chronologyValidationResult);

            var overlapValidationResult = ValidateHolidayPeridOverlap(startDate, endDate);
            if (overlapValidationResult.Status == ValidationStatus.Error)
                result.Add(overlapValidationResult);

            return result;
        }


        private ValidationResult ValidatePeriodLength(DateTime startDate, DateTime endDate)
        {
            if (endDate - startDate <= TimeSpan.FromDays(MAX_PERIOD_LENGTH))
                return new ValidationResult();
            return new ValidationResult(LENGTH_ERROR);
        }

        private ValidationResult ValidatePeriodChronology(DateTime startDate, DateTime endDate)
        {
            if (endDate >= startDate)
                return new ValidationResult();
            return new ValidationResult(CHORONOLOGY_ERROR);
        }

        private ValidationResult ValidateHolidayPeridOverlap(DateTime startDate, DateTime endDate)
        {
            // Should never happen if ValidatePeriodLength is called before this with the current requirements
            if (endDate.Year - startDate.Year > 1)
                return new ValidationResult(OVERLAP_ERROR);

            // Should never happen if ValidatePeriodChronology is called before this
            if (startDate > endDate)
                throw new ArgumentException($"{nameof(startDate)} is greater than {nameof(endDate)}");

            List<DateTime> holidayPeriodDates = new() { new DateTime(startDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY) };
            if (endDate.Year != startDate.Year)
                holidayPeriodDates.Add(new DateTime(endDate.Year, HOLIDAY_PERIOD_START_MONTH, HOLIDAY_PERIOD_START_DAY));

            if (holidayPeriodDates.Any(date => IsDateBetween(date, startDate, endDate)))
                return new ValidationResult(OVERLAP_ERROR);
            return new ValidationResult();
        }

        private bool IsDateBetween(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck > startDate && dateToCheck < endDate;
        }
    }
}
