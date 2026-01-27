using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public static NotFoundException For(string resourceName, object key)
          => new NotFoundException($"{resourceName} '{key}' não foi encontrado.");
    }
}
