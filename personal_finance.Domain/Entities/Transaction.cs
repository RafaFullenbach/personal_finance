using personal_finance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace personal_finance.Domain.Entities
{
    internal class Transaction
    {
        public Guid Id { get; set; }

        public decimal Amount { get; private set; }

        public TransactionType Type { get; private set; }

        public TransactionStatus Status { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public int CompetenceYear { get; private set; }

        public int CompetenceMonth { get; private set; }

        public string Description { get; private set; }

        public Transaction(
        decimal amount,
        TransactionType type,
        DateTime transactionDate,
        int competenceYear,
        int competenceMonth,
        string description)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (competenceMonth < 1 || competenceMonth > 12)
                throw new ArgumentException("Invalid competence month.");

            Id = Guid.NewGuid();
            Amount = amount;
            Type = type;
            TransactionDate = transactionDate;
            CreatedAt = DateTime.UtcNow;
            CompetenceYear = competenceYear;
            CompetenceMonth = competenceMonth;
            Description = description;
            Status = TransactionStatus.Pending;
        }
        public void Confirm()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending transactions can be confirmed.");

            Status = TransactionStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending transactions can be cancelled.");

            Status = TransactionStatus.Cancelled;
        }
    }
}
