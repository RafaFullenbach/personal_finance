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
        public const string ReportInvalidPeriod = "REPORT_INVALID_PERIOD";
        public const string AccountInvalidType = "ACCOUNT_INVALID_TYPE";
        public const string TransferInvalidAccounts = "TRANSFER_INVALID_ACCOUNTS";
        public const string CategoryInvalidName = "CATEGORY_INVALID_NAME";
        public const string CategoryInvalidType = "CATEGORY_INVALID_TYPE";
    }
}
