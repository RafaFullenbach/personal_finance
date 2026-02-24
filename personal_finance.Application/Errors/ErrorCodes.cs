using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Errors
{
    public class ErrorCodes
    {
        //Transactions
        public const string TransactionNotFound = "TRANSACTION_NOT_FOUND";
        public const string TransactionInvalidType = "TRANSACTION_INVALID_TYPE";
        public const string TransactionInvalidAmount = "TRANSACTION_INVALID_AMOUNT";
        public const string TransactionInvalidCompetence = "TRANSACTION_INVALID_COMPETENCE";
        public const string TransactionInvalidStatus = "TRANSACTION_INVALID_STATUS";
        public const string QueryInvalidPagination = "QUERY_INVALID_PAGINATION";
        public const string QueryInvalidSort = "QUERY_INVALID_SORT";
        public const string TransactionInvalidUpdate = "TRANSACTION_INVALID_UPDATE";
        public const string TransactionInvalidCategory = "TRANSACTION_INVALID_CATEGORY";

        //Reports
        public const string ReportInvalidPeriod = "REPORT_INVALID_PERIOD";

        //Accounts
        public const string AccountInvalidType = "ACCOUNT_INVALID_TYPE";
        public const string TransferInvalidAccounts = "TRANSFER_INVALID_ACCOUNTS";
        public const string AccountInvalidId = "ACCOUNT_INVALID_ID";
        public const string AccountInvalidName = "ACCOUNT_INVALID_NAME";
        
        //Categories
        public const string CategoryInvalidName = "CATEGORY_INVALID_NAME";
        public const string CategoryInvalidType = "CATEGORY_INVALID_TYPE";
        public const string CategoryInvalidId = "CATEGORY_INVALID_ID";

        //Budgets
        public const string BudgetInvalidCategory = "BUDGET_CATEGORY_INVALID";
        public const string BudgetInvalidPeriod = "BUDGET_INVALID_PERIOD";
        public const string BudgetInvalidLimit = "BUDGET_INVALID_LIMIT";
        public const string BudgetInvalidCategoryType = "BUDGET_INVALID_CATEGORY_TYPE";
        public const string BudgetInvalidId = "BUDGET_INVALID_ID";
        

        //Recurring
        public const string RecurringInvalidAmount = "RECURRING_INVALID_AMOUNT";
        public const string RecurringInvalidType = "RECURRING_INVALID_TYPE";
        public const string RecurringInvalidPeriod = "RECURRING_INVALID_PERIOD";
        public const string RecurringInvalidId = "RECURRING_INVALID_ID";
        public const string MonthCloseInvalidPeriod = "MONTH_CLOSE_INVALID_PERIOD";
        public const string MonthClosed = "MONTH_CLOSED";
   
    }
}


