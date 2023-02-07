using System.Globalization;

namespace HolidayPlanner
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string dateRangeSeparator = "-";
            string dateRangeStr = string.Join("", args);

            var validator = new HolidayPlannerValidator();
            var nationalHolidayProvider = new FinnishNationalHolidayProvider();
            var dtFormatProvider = new CultureInfo(nationalHolidayProvider.Locale);
            var parser = new DateRangeParser(dateRangeSeparator) { FormatProvider = dtFormatProvider };
            var holidayPlanner = new HolidayPlanner(nationalHolidayProvider, validator, parser);

            try
            {
                int consumedHolidays = holidayPlanner.CalculateSpentHolidays(dateRangeStr);
                Console.WriteLine($"Spending vacation during period {dateRangeStr} consumes {consumedHolidays} vacation days");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
            return 0;

            // TODO:
            // UNIT TESTS!
        }
    }
}