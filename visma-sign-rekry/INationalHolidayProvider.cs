using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visma_sign_rekry
{
    public interface INationalHolidayProvider
    {
        string Locale { get; }

        List<DateTime> GetNationalHolidays(DateTime startDate, DateTime endDate);
    }
}
