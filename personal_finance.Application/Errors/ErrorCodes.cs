using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Errors
{
    public class ErrorCodes
    {
        public const string TransactionNotFound = "TRANSACTION_NOT_FOUND";
        public const string TransactionInvalidType = "TRANSACTION_INVALID_TYPE";
        public const string TransactionInvalidAmount = "TRANSACTION_INVALID_AMOUNT";
        public const string TransactionInvalidCompetence = "TRANSACTION_INVALID_COMPETENCE";
    }
}
