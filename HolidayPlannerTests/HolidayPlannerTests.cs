using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HolidayPlanner.Tests
{
    [TestClass()]
    public class HolidayPlannerTests
    {
        // TODO: write readme

        // TODO: make repo public

        // TODO: send email

        #region CalculateSpentHolidays DateTime params

        #region Ok without national holidays

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkOneMonday()
        {
            var expectedResult = 1;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = new DateTime(startDate.Ticks);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkOneSunday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = new DateTime(startDate.Ticks);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkMondayToWednesday()
        {
            var dayAddition = 2;
            var expectedResult = 3;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkMondayToSaturday()
        {
            var dayAddition = 5;
            var expectedResult = 6;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkMondayToNextMonday()
        {
            var dayAddition = 7;
            var expectedResult = 7;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkSundayToMonday()
        {
            var dayAddition = 1;
            var expectedResult = 1;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkSundayToSunday()
        {
            var dayAddition = 7;
            var expectedResult = 6;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region Ok with national holidays

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkOneHolidayMonday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = new DateTime(startDate.Ticks);
            var holidays = new List<DateTime>() { startDate };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkOneHolidaySunday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = new DateTime(startDate.Ticks);
            var holidays = new List<DateTime>() { startDate };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_OkOneWeekWithHolidayFridayToSunday()
        {
            var expectedResult = 4;
            var daysToAdd = 6;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(daysToAdd);
            //                                        Friday                Saturday                Sunday
            var holidays = new List<DateTime>() { startDate.AddDays(4), startDate.AddDays(5), startDate.AddDays(6) };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_Ok5DaysOutsideOfHolidays()
        {
            var expectedResult = 4;
            var daysToAdd = 4;
            var startDate = new DateTime(2023, 2, 16); // Thursday
            var endDate = startDate.AddDays(daysToAdd);
            var holidays = new List<DateTime>() { startDate.AddDays(-1), startDate.AddDays(5), };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays(startDate, endDate);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region Exceptions

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_ValidationFailOneError()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            validatorMock.Setup(v => v.ValidateHolidayPeriod(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new ValidationResult("Some error"));

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of validation failure"));
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_ValidationFailMultipleErrors()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            validatorMock.Setup(v => v.ValidateHolidayPeriod(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new ValidationResult("Some error", "more errors", "this is real bad"));

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of validation failure"));
        }

        [TestMethod()]
        public void CalculateSpentHolidays_DateTimeParams_HolidayProviderArgumentException()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws<ArgumentException>();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of holiday provider exception"));
        }

        #endregion

        #endregion


        #region CalculateSpentHolidays string param

        #region Ok without national holidays

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkOneMonday()
        {
            var expectedResult = 1;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = new DateTime(startDate.Ticks);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkOneSunday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = new DateTime(startDate.Ticks);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkMondayToWednesday()
        {
            var dayAddition = 2;
            var expectedResult = 3;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkMondayToSaturday()
        {
            var dayAddition = 5;
            var expectedResult = 6;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkMondayToNextMonday()
        {
            var dayAddition = 7;
            var expectedResult = 7;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkSundayToMonday()
        {
            var dayAddition = 1;
            var expectedResult = 1;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkSundayToSunday()
        {
            var dayAddition = 7;
            var expectedResult = 6;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = startDate.AddDays(dayAddition);

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region Ok with national holidays

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkOneHolidayMonday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = new DateTime(startDate.Ticks);
            var holidays = new List<DateTime>() { startDate };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkOneHolidaySunday()
        {
            var expectedResult = 0;
            var startDate = new DateTime(2023, 2, 12); // Sunday
            var endDate = new DateTime(startDate.Ticks);
            var holidays = new List<DateTime>() { startDate };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_OkOneWeekWithHolidayFridayToSunday()
        {
            var expectedResult = 4;
            var daysToAdd = 6;
            var startDate = new DateTime(2023, 2, 13); // Monday
            var endDate = startDate.AddDays(daysToAdd);
            //                                        Friday                Saturday                Sunday
            var holidays = new List<DateTime>() { startDate.AddDays(4), startDate.AddDays(5), startDate.AddDays(6) };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_Ok5DaysOutsideOfHolidays()
        {
            var expectedResult = 4;
            var daysToAdd = 4;
            var startDate = new DateTime(2023, 2, 16); // Thursday
            var endDate = startDate.AddDays(daysToAdd);
            var holidays = new List<DateTime>() { startDate.AddDays(-1), startDate.AddDays(5), };

            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock(startDate, endDate);

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime start, DateTime end) => holidays.Where(d => IsDateTimeInRange(d, start, end)).ToList());

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);
            var result = holidayPlanner.CalculateSpentHolidays("It's just a mock, so whatever");

            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region Exceptions

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_ValidationFailOneError()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            validatorMock.Setup(v => v.ValidateHolidayPeriod(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new ValidationResult("Some error"));

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of validation failure"));
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_ValidationFailMultipleErrors()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            validatorMock.Setup(v => v.ValidateHolidayPeriod(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new ValidationResult("Some error", "more errors", "this is real bad"));

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of validation failure"));
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_ParserFormatException()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            parserMock.Setup(parser => parser.Parse(It.IsAny<string>())).Throws<FormatException>();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of parser exception"));
        }

        [TestMethod()]
        public void CalculateSpentHolidays_StringParams_HolidayProviderArgumentException()
        {
            var holidayProviderMock = GetDefaultNationalHolidayProviderMock();
            var validatorMock = GetDefaultValidatorMock();
            var parserMock = GetDefaultParserMock();

            holidayProviderMock.Setup(provider => provider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws<ArgumentException>();

            var holidayPlanner = new HolidayPlanner(holidayProviderMock.Object, validatorMock.Object, parserMock.Object);

            Assert.ThrowsException<ArgumentException>(() => holidayPlanner.CalculateSpentHolidays("Throwing because of holiday provider exception"));
        }

        #endregion

        #endregion


        #region Private methods

        #region Private mock creation

        private Mock<INationalHolidayProvider> GetDefaultNationalHolidayProviderMock()
        {
            var holidayProviderMock = new Mock<INationalHolidayProvider>();
            holidayProviderMock.SetupAllProperties();
            holidayProviderMock.Setup((holidayProvider) => holidayProvider.GetNationalHolidaysBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<DateTime>());
            return holidayProviderMock;
        }

        private Mock<IHolidayPlannerValidator> GetDefaultValidatorMock()
        {
            var validatorMock = new Mock<IHolidayPlannerValidator>();
            validatorMock.SetupAllProperties();
            validatorMock.Setup((validator) => validator.ValidateHolidayPeriod(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new ValidationResult());
            return validatorMock;
        }

        private Mock<DateRangeParser> GetDefaultParserMock()
        {
            return GetDefaultParserMock(DateTime.Now, DateTime.Now);
        }

        private Mock<DateRangeParser> GetDefaultParserMock(DateTime firstDateTimeToReturn, DateTime secondDateTimeToReturn)
        {
            var parserMock = new Mock<DateRangeParser>(new[] { "|" });
            parserMock.SetupAllProperties();
            parserMock.Setup((parser) => parser.Parse(It.IsAny<string>()))
                .Returns((firstDateTimeToReturn, secondDateTimeToReturn));
            return parserMock;
        }

        #endregion

        private bool IsDateTimeInRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
            => dateToCheck >= startDate && dateToCheck <= endDate;

        #endregion
    }
}