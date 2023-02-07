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
    public class ValidationResultTests
    {
        #region Constructors

        [TestMethod()]
        public void Constructor_NoParams()
        {
            var valResult = new ValidationResult();
            Assert.AreEqual(ValidationStatus.Success, valResult.Status);
            Assert.AreEqual(0, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_EmptyStringList()
        {
            var valResult = new ValidationResult(Array.Empty<string>());
            Assert.AreEqual(ValidationStatus.Success, valResult.Status);
            Assert.AreEqual(0, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_EmptyValidationErrorList()
        {
            var valResult = new ValidationResult(Array.Empty<ValidationError>());
            Assert.AreEqual(ValidationStatus.Success, valResult.Status);
            Assert.AreEqual(0, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_SingleString()
        {
            var valResult = new ValidationResult("error!");
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(1, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_SingleValidationError()
        {
            var valResult = new ValidationResult(new ValidationError("error str"));
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(1, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_MultipleStrings()
        {
            var valResult = new ValidationResult("error!", "more errors!!", "so many errors?", "even more");
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(4, valResult.ErrorCount);
        }

        [TestMethod()]
        public void Constructor_MultipleValidationErrors()
        {
            var valResult = new ValidationResult(new ValidationError("error str"), new ValidationError("second error"));
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(2, valResult.ErrorCount);
        }

        #endregion

        #region AddErrors

        [TestMethod()]
        public void AddErrors_ZeroStrings()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors(Array.Empty<string>());
            Assert.AreEqual(ValidationStatus.Success, valResult.Status);
            Assert.AreEqual(0, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_SingleString()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors("some error");
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(1, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_MultipleStrings()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors("some error", "another error", "more errors");
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(3, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_MultipleStringsTwice()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors("some error", "another error", "more errors");
            valResult.AddErrors("more errors", "more!");
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(5, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_ZeroValidationErrors()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors(Array.Empty<ValidationError>());
            Assert.AreEqual(ValidationStatus.Success, valResult.Status);
            Assert.AreEqual(0, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_SingleValidationError()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors(new ValidationError("some error"));
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(1, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_MultipleValidationErrors()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors(new ValidationError("some error"), new ValidationError("another error"), new ValidationError("more errors"));
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(3, valResult.ErrorCount);
        }

        [TestMethod()]
        public void AddErrors_MultipleValidationErrorsTwice()
        {
            var valResult = new ValidationResult();
            valResult.AddErrors(new ValidationError("some error"), new ValidationError("another error"), new ValidationError("more errors"));
            valResult.AddErrors(new ValidationError("more errors"), new ValidationError("more!"));
            Assert.AreEqual(ValidationStatus.Error, valResult.Status);
            Assert.AreEqual(5, valResult.ErrorCount);
        }

        #endregion

        #region GetErrorMessages

        [TestMethod()]
        public void GetErrorMessages_Zero()
        {
            var valResult = new ValidationResult("test");
            var errorMessages = valResult.GetErrorMessages();
            Assert.AreEqual(1, errorMessages.Count);
        }

        [TestMethod()]
        public void GetErrorMessages_SingleString()
        {
            var valResult = new ValidationResult();
            var errorMessages = valResult.GetErrorMessages();
            Assert.AreEqual(0, errorMessages.Count);
        }

        [TestMethod()]
        public void GetErrorMessages_MultipleStrings()
        {
            var valResult = new ValidationResult("test");
            valResult.AddErrors("testing");
            var errorMessages = valResult.GetErrorMessages();
            Assert.AreEqual(2, errorMessages.Count);
        }

        #endregion
    }
}