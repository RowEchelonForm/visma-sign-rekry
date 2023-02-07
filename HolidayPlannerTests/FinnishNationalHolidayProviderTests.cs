using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner.Tests
{
    [TestClass()]
    public class FinnishNationalHolidayProviderTests
    {
        [TestMethod()]
        public void GetNationalHolidaysBetween_SameNonHolidayDate()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 2, 6);
            var endDate = new DateTime(2023, 2, 6);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 0);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_NonHolidayRange()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 9, 30);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 0);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_NewYear()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 1);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_Epiphany()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 1, 6);
            var endDate = new DateTime(2023, 1, 6);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_Easter2023()
        {
            // Good friday is 2023-04-07
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 4, 7);
            var endDate = new DateTime(2023, 4, 10);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 3);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_1stOfMay()
        {
            // Good friday is 2023-04-07
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 5, 1);
            var endDate = new DateTime(2023, 5, 1);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_FeastOfAscension2023()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 5, 18);
            var endDate = new DateTime(2023, 5, 18);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_Pentecost2023()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 5, 28);
            var endDate = new DateTime(2023, 5, 28);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_Midsummer2023()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 6, 23);
            var endDate = new DateTime(2023, 6, 24);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 2);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_AllSaintsDay2023()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 11, 4);
            var endDate = new DateTime(2023, 11, 4);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_IndependenceDay()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 12, 6);
            var endDate = new DateTime(2023, 12, 6);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 1);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_Christmas()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 12, 24);
            var endDate = new DateTime(2023, 12, 26);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 3);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_FullYear2023()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 12, 31);
            var holidays = provider.GetNationalHolidaysBetween(startDate, endDate);
            Assert.AreEqual(holidays.Count, 15);
        }

        [TestMethod()]
        public void GetNationalHolidaysBetween_ChronologicalError()
        {
            var provider = new FinnishNationalHolidayProvider();
            var startDate = new DateTime(2023, 2, 6);
            var endDate = new DateTime(2023, 2, 5);
            Assert.ThrowsException<ArgumentException>(() => provider.GetNationalHolidaysBetween(startDate, endDate));
        }
    }
}