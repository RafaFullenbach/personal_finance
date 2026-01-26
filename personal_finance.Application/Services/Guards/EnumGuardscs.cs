using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;

namespace personal_finance.Application.Services.Guards
{
    public static class EnumGuards
    {
        public static T ParseOrThrow<T>(string? value, string fieldName, string errorCode) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw ValidationException.Invalid($"{fieldName} é obrigatório.", errorCode);

            if (!Enum.TryParse<T>(value, ignoreCase: true, out var parsed))
                throw ValidationException.Invalid($"{fieldName} '{value}' é inválido.", errorCode);

            return parsed;
        }

        public static T? TryParseNullable<T>(string? value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return Enum.TryParse<T>(value, ignoreCase: true, out var parsed) ? parsed : null;
        }
    }
}
