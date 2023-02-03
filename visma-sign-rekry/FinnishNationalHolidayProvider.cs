using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visma_sign_rekry
{
    public class FinnishNationalHolidayProvider : INationalHolidayProvider
    {
        public string Locale { get => "FI"; }


        public List<DateTime> GetNationalHolidays(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
