using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    public enum ValidationStatus
    {
        Success,
        Error,
    }

    /// <summary>
    /// Result of validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Status of the validation. Default value is <see cref="ValidationStatus.Success"/>.
        /// </summary>
        public ValidationStatus Status { get; protected set; } = ValidationStatus.Success;

        /// <summary>
        /// The number of errors.
        /// </summary>
        public int ErrorCount { get => _errors.Count; }


        private readonly List<string> _errors = new();


        /// <summary>
        /// Initializes a new <see cref="ValidationResult"/> object where 
        /// <see cref="Status"/> is <see cref="ValidationStatus.Success"/>.
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ValidationResult"/> object with error messages
        /// and <see cref="Status"/> <see cref="ValidationStatus.Error"/>.
        /// </summary>
        public ValidationResult(params string[] errors)
        {
            AddErrors(errors);
        }


        /// <summary>
        /// Adds new error message(s) to this validation result. <see cref="Status"/> 
        /// will be <see cref="ValidationStatus.Error"/> after calling this.
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrors(params string[] errors)
        {
            if (errors.Any(error => error == null))
                throw new ArgumentException($"{nameof(errors)} array contains null error(s)");

            _errors.AddRange(errors);
            Status = ValidationStatus.Error;
        }

        /// <summary>
        /// Gets all error messages in this <see cref="ValidationResult"/>.
        /// </summary>
        /// <remarks>
        /// This method is not very useful ATM as <see cref="_errors"/> might as well be a 
        /// property with a public getter. However, a future implementation work with some 
        /// kinds of error codes in method <see cref="AddErrors(string[])"/>. Then, this 
        /// method might be used to get the actual (potentially translated) error messages.
        /// </remarks>
        /// <returns>A list of error messages from the validation.</returns>
        public List<string> GetErrorMessages()
            => _errors.ToList();  // create a copy so that the original list can't be modified by the caller later
    }
}
