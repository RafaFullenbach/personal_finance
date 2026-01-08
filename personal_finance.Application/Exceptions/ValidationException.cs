using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public string Code { get; }

        public ValidationException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public static ValidationException Invalid(
            string message,
            string code)
            => new ValidationException(message, code);
    }
}
