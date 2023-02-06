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


        private readonly List<ValidationError> _errors = new();


        /// <summary>
        /// Initializes a new <see cref="ValidationResult"/> object where 
        /// <see cref="Status"/> is <see cref="ValidationStatus.Success"/>.
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ValidationResult"/> object with error messages 
        /// and <see cref="Status"/> <see cref="ValidationStatus.Error"/> 
        /// if <paramref name="errors"/> contains any items.
        /// </summary>
        public ValidationResult(params string[] errors)
        {
            AddErrors(errors);
        }

        /// <summary>
        /// Initializes a new <see cref="ValidationResult"/> object with error messages 
        /// and <see cref="Status"/> <see cref="ValidationStatus.Error"/> 
        /// if <paramref name="errors"/> contains any items.
        /// </summary>
        public ValidationResult(params ValidationError[] errors)
        {
            AddErrors(errors);
        }


        /// <summary>
        /// Adds new error message(s) to this validation result. <see cref="Status"/> 
        /// will be <see cref="ValidationStatus.Error"/> after calling this 
        /// if <paramref name="errorMessages"/> contains items.
        /// </summary>
        /// <param name="errorMessages">One of more error messages.</param>
        /// <exception cref="ArgumentException"><paramref name="errorMessages"/> contains null items.</exception>
        public void AddErrors(params string[] errorMessages)
        {
            if (errorMessages.Any(error => error == null))
                throw new ArgumentException($"{nameof(errorMessages)} array contains null value(s)");

            AddErrors(errorMessages.Select(msg => new ValidationError(msg)).ToArray());
        }

        /// <summary>
        /// Adds new error(s) to this validation result. <see cref="Status"/> 
        /// will be <see cref="ValidationStatus.Error"/> after calling this 
        /// if <paramref name="errors"/> contains items.
        /// </summary>
        /// <param name="errors">One or more error objects.</param>
        /// <exception cref="ArgumentException"><paramref name="errors"/> contains null items.</exception>
        public void AddErrors(params ValidationError[] errors)
        {
            if (errors.Any(error => error == null))
                throw new ArgumentException($"{nameof(errors)} array contains null value(s)");

            _errors.AddRange(errors);

            if (errors.Length > 0)
                Status = ValidationStatus.Error;
        }

        /// <summary>
        /// Gets all error messages in this <see cref="ValidationResult"/>.
        /// </summary>
        /// <returns>A list of error messages from the validation.</returns>
        public List<string> GetErrorMessages()
            => _errors.Select(error => error.ToString()).ToList();
    }
}
