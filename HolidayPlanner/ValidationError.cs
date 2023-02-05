using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner
{
    /// <summary>
    /// Class for error objects.
    /// </summary>
    /// <remarks>
    /// Note: the current implementation is kind of a stub. 
    /// A proper application might have error codes, translations or something like that.
    /// </remarks>
    public class ValidationError
    {
        public string ErrorMessage { get; protected set; }

        public ValidationError(string errorMessage)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public override string ToString()
        {
            return ErrorMessage.ToString();
        }
    }
}
