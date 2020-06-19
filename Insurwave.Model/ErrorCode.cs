using System.Collections.Generic;
using System.Linq;

namespace Insurwave.Model
{
    public enum ErrorCode
    {
        InvalidLocation = 1006,
        InvalidTemperature = 1007,
        MissingLocation = 9997,
        ServiceUnavailable = 9998,
        ApplicationError = 9999
    }

    public static class Errors
    {
        private static List<ErrorMessage> _errorList;

        public static ErrorMessage GetErrorMessage(ErrorCode code)
        {
            if (_errorList == null)
            {
                _errorList = new List<ErrorMessage>
                {
                    new ErrorMessage { code = 1006, message = "Invalid location has been supplied." },
                    new ErrorMessage { code = 1007, message = $"Invalid temperature has been supplied. Valid values are '{Temperature.c}': celcius, '{Temperature.f}': fahrenheit, '{Temperature.k}': kelvin, '': celcius" },
                    new ErrorMessage { code = 9997, message = "No location specified." },
                    new ErrorMessage { code = 9998, message = "Partial service unavailable, please try again in a few moments." },
                    new ErrorMessage { code = 9999, message = "Application error, please try again later." }
                };
            }

            return _errorList.First(e => e.code == (int)code);
        }
    }
}