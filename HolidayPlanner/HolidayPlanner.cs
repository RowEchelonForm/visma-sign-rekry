using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public class HolidayPlanner : IHolidayPlanner
    {
        private readonly INationalHolidayProvider _nationalHolidayProvider;


        public HolidayPlanner(INationalHolidayProvider nationalHolidayProvider)
        {
            _nationalHolidayProvider = nationalHolidayProvider;
        }


        public int CalculateSpentHolidays(DateTime startDate, DateTime endDate)
        {
            // validate

            // calculate

            throw new NotImplementedException();
        }
    }
}
