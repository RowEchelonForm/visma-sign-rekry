using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public interface IHolidayPlanner
    {
        // Should there be method(s) that take string arguments

        // Should there be a method that returns a list of dates that use up vacation days (instead of just the number of them)
        int CalculateSpentHolidays(DateTime startDate, DateTime endDate);

    }
}
