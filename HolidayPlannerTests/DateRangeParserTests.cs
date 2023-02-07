using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HolidayPlanner.Tests
{
    [TestClass()]
    public class DateRangeParserTests
    {
        #region Parse valid

        [TestMethod()]
        public void Parse_ValidDatesSingleCharacterSeparator()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesWithWhitespaceSingleCharacterSeparator()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $" {Environment.NewLine} \t {startDate} {separator} \t{endDate}  ";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesWithMultiCharacterSeparator()
        {
            var separator = ";|_";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesWithMultiCharacterWhitespaceSeparator()
        {
            var separator = " | ";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesWithMultipleSingleCharacterSeparators()
        {
            var separators = new[] { "|", ";" };
            var parser = new DateRangeParser(separators);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            foreach (var separator in separators)
            {
                var dateRangeStr = $"{startDate}{separator}{endDate}";
                var result = parser.Parse(dateRangeStr);
                Assert.AreEqual(startDate, result.Item1);
                Assert.AreEqual(endDate, result.Item2);
            }
        }

        [TestMethod()]
        public void Parse_ValidDatesWithMultipleMultiCharacterSeparators()
        {
            var separators = new[] { "|_+", "^¤#", "!()()" };
            var parser = new DateRangeParser(separators);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            foreach (var separator in separators)
            {
                var dateRangeStr = $"{startDate}{separator}{endDate}";
                var result = parser.Parse(dateRangeStr);
                Assert.AreEqual(startDate, result.Item1);
                Assert.AreEqual(endDate, result.Item2);
            }
        }

        [TestMethod()]
        public void Parse_ValidDatesCustomDateFormat()
        {
            var separator = "|";
            var format = "yyyy_MM_dd";
            var parser = new DateRangeParser(separator) { DateFormat = format };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate.ToString(format)}{separator}{endDate.ToString(format)}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesSpecifiedDateAndTimeFormat()
        {
            var separator = "|";
            var format = "yyyy_MM_dd HH~mm!ss¤ffffff";
            var parser = new DateRangeParser(separator) { DateFormat = format };
            var startDate = new DateTime(2023, 2, 11, 20, 40, 50, 123, 456);
            var endDate = new DateTime(2023, 2, 15, 15, 30, 55, 987, 654);

            var dateRangeStr = $"{startDate.ToString(format)}{separator}{endDate.ToString(format)}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesCustomFormatProvider()
        {
            var separator = "|";
            var fiFormatProvider = new CultureInfo("FI");
            var parser = new DateRangeParser(separator) { FormatProvider = fiFormatProvider };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate.ToString(fiFormatProvider)}{separator}{endDate.ToString(fiFormatProvider)}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesCustomFormatProviderAndDateFormat()
        {
            var separator = "|";
            var fiFormatProvider = new CultureInfo("FI");
            var format = "MMMM d yyyy";
            var parser = new DateRangeParser(separator) { FormatProvider = fiFormatProvider, DateFormat = format };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate.ToString(format, fiFormatProvider)}{separator}{endDate.ToString(format, fiFormatProvider)}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate, result.Item1);
            Assert.AreEqual(endDate, result.Item2);
        }

        [TestMethod()]
        public void Parse_ValidDatesTimeZoneInFormatAdjustToUniversalDateTimeStyles()
        {
            var separator = "|";
            var styles = DateTimeStyles.AdjustToUniversal;
            var format = "yyyy-MM-dd HH:mm:ssK";
            var parser = new DateRangeParser(separator) { DateStyles = styles, DateFormat = format, };

            var startDate = new DateTime(2023, 2, 11, 13, 23, 43, DateTimeKind.Local);
            var endDate = new DateTime(2023, 2, 15, 14, 33, 53, DateTimeKind.Local);

            var dateRangeStr = $"{startDate.ToString(format)}{separator}{endDate.ToString(format)}";
            var result = parser.Parse(dateRangeStr);
            Assert.AreEqual(startDate.ToUniversalTime(), result.Item1);
            Assert.AreEqual(endDate.ToUniversalTime(), result.Item2);
        }

        #endregion


        #region Parse invalid

        [TestMethod()]
        public void Parse_InvalidNonsenseDateString()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);

            var dateRangeStr = $"complete nonsense string slajdsjadlkaj";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidDateString()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);

            var dateRangeStr = $"2023-02-29{separator}2023-02-30";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidFormatProvider()
        {
            var separator = "|";
            var dateFormat = "MMM dd yyyy";
            var fiFormatProvider = new CultureInfo("FI");
            var usFormatProvider = new CultureInfo("en-US");
            var parser = new DateRangeParser(separator) { DateFormat = dateFormat, FormatProvider = fiFormatProvider };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);


            var dateRangeStr = $"{startDate.ToString(usFormatProvider)}{separator}{endDate.ToString(usFormatProvider)}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidDateStyles()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator) { DateStyles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AssumeLocal };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidDateFormat()
        {
            var separator = "|";
            var requiredFormat = "dd.MM.yyyy";
            var actualFormat = "MM/dd/yyyy";
            var parser = new DateRangeParser(separator) { DateFormat = requiredFormat };
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);


            var dateRangeStr = $"{startDate.ToString(actualFormat)}{separator}{endDate.ToString(actualFormat)}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidSingleCharacterSeparatorNotFound()
        {
            var separator = "|";
            var parser = new DateRangeParser(";");
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidMultiCharacterSeparatorNotFound()
        {
            var separator = "|+;#";
            var parser = new DateRangeParser(";");
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);

            var dateRangeStr = $"{startDate}{separator}{endDate}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidTooManyDates()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);
            var endDate = new DateTime(2023, 2, 15);
            var thirdDate = new DateTime(2023, 2, 25);

            var dateRangeStr = $"{startDate}{separator}{endDate}{separator}{thirdDate}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidTooFewDates()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);
            var startDate = new DateTime(2023, 2, 11);

            var dateRangeStr = $"{startDate}{separator}";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidEmptyString()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);

            var dateRangeStr = $"";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        [TestMethod()]
        public void Parse_InvalidWhitespaceOnlyString()
        {
            var separator = "|";
            var parser = new DateRangeParser(separator);

            var dateRangeStr = $"    ";
            Assert.ThrowsException<FormatException>(() => parser.Parse(dateRangeStr));
        }

        #endregion


        #region Constructor errors

        [TestMethod()]
        public void Constructor_EmptySeparator()
        {
            Assert.ThrowsException<ArgumentException>(() => new DateRangeParser(""));
        }

        [TestMethod()]
        public void Constructor_EmptyListSeparator()
        {
            Assert.ThrowsException<ArgumentException>(() => new DateRangeParser(Array.Empty<string>()));
        }

        [TestMethod()]
        public void Constructor_ListWithEmptySeparator()
        {
            Assert.ThrowsException<ArgumentException>(() => new DateRangeParser("-", "", ";"));
        }

        [TestMethod()]
        public void Constructor_ListWithNullSeparator()
        {
            Assert.ThrowsException<ArgumentException>(() => new DateRangeParser("-", null, ";"));
        }

        #endregion
    }
}