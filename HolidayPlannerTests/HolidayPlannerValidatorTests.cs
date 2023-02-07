using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace HolidayPlanner.Tests
{
    [TestClass()]
    public class HolidayPlannerValidatorTests
    {
        private const string VALIDATE_PERIOD_LENGTH_METHOD_NAME = "ValidatePeriodLength";
        private const string VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME = "ValidatePeriodChronology";
        private const string VALIDATE_PERIOD_OVERLAP_METHOD_NAME = "ValidateHolidayPeridOverlap";


        // Functionality is tested with tests of private methods.
        // There are just a few basic tests for this public method.

        [TestMethod()]
        public void ValidateHolidayPeriodTest_NoValidationErrors()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(1);
            var result = validator.ValidateHolidayPeriod(startDate, endDate);
            Assert.IsNotNull(result);
            Assert.AreEqual(ValidationStatus.Success, result.Status);
            Assert.AreEqual(0, result.ErrorCount);
        }

        [TestMethod()]
        public void ValidateHolidayPeriodTest_SingleError()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 5, 15);
            var endDate = startDate.AddDays(100);
            var result = validator.ValidateHolidayPeriod(startDate, endDate);
            Assert.IsNotNull(result);
            Assert.AreEqual(ValidationStatus.Error, result.Status);
            Assert.AreEqual(1, result.ErrorCount);
        }

        [TestMethod()]
        public void ValidateHolidayPeriodTest_MultipleError()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(100);
            var result = validator.ValidateHolidayPeriod(startDate, endDate);
            Assert.IsNotNull(result);
            Assert.AreEqual(ValidationStatus.Error, result.Status);
            Assert.IsTrue(result.ErrorCount > 1);
        }


        #region Private method testing

        // Testing private methods that do the actual validation checks. 
        // IMO unit tests on private methods are kind of ugly generally 
        // speaking since private methods are usually implementation details. 
        // However, IMO testing them is ok in this case since all 
        // validation checks are in these private methods, and these test 
        // can be used to check very "fine" details about the validation.


        #region ValidatePeriodLength

        [TestMethod()]
        public void Private_ValidatePeriodLength_0Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = new DateTime(startDate.Ticks);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_1Day()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_5Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(5);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_49Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(49);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_50Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(50);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_51Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(51);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodLength_100Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(100);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_LENGTH_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        #endregion

        #region ValidatePeriodChronology

        [TestMethod()]
        public void Private_ValidatePeriodChronology_Negative370Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(-370);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_Negative10Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(-10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_Negative1Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(-10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_0Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = new DateTime(startDate.Ticks);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_1Day()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_10Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodChronology_360Days()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(360);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_CHRONOLOGY_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        #endregion

        #region ValidatePeriodOverlap

        // Period threshold is 1st of April

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_0DaysNoOverlap()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = new DateTime(startDate.Ticks);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_1DayNoOverlap()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_30DaysNoOverlap()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 2, 15);
            var endDate = startDate.AddDays(30);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_1DayFromFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 4, 1);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_10DaysFromFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 4, 1);
            var endDate = startDate.AddDays(10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_364DaysFromFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2022, 4, 1);
            var endDate = startDate.AddDays(364);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_365DaysFromFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2022, 4, 1);
            var endDate = startDate.AddDays(365);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_370DaysFromFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2022, 4, 1);
            var endDate = startDate.AddDays(370);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_1DayFromLastPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 3, 31);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_10DaysFromLastPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 3, 31);
            var endDate = startDate.AddDays(10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValidationError));
            Assert.IsNotNull(((ValidationError)result).ErrorMessage);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_0DaysOnFirstPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 4, 1);
            var endDate = new DateTime(startDate.Ticks);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_0DaysOnLastPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 3, 31);
            var endDate = new DateTime(startDate.Ticks);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_1DayUntilLastPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 3, 30);
            var endDate = startDate.AddDays(1);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Private_ValidatePeriodOverlap_10DaysUntilLastPeriodDay()
        {
            var validator = new HolidayPlannerValidator();
            var startDate = new DateTime(2023, 3, 21);
            var endDate = startDate.AddDays(10);

            var result = InvokePrivate(validator, VALIDATE_PERIOD_OVERLAP_METHOD_NAME, startDate, endDate);
            Assert.IsNull(result);
        }

        #endregion

        #endregion


        private object? InvokePrivate(object obj, string method, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
                throw new ArgumentException($"No private method {method} found");

            var res = methodInfo.Invoke(obj, parameters);
            return res;
        }
    }
}