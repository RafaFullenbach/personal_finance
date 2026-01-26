using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class Transaction
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
        public Guid AccountId { get; private set; }
        public Guid? TransferId { get; private set; }
        public Guid? CategoryId { get; private set; }
        public Guid? RecurringTemplateId { get; private set; }

        public Transaction(
        decimal amount,
        TransactionType type,
        DateTime transactionDate,
        int competenceYear,
        int competenceMonth,
        string description,
        Guid accountId,
        Guid? transferId = null,
        Guid? categoryId = null,
        Guid? recurringTemplateId = null)
        {
            if (amount <= 0)
                throw new ArgumentException("O valor precisa ser maior que zero");

            if (competenceMonth < 1 || competenceMonth > 12)
                throw new ArgumentException("Mês de competência inválido.");

            Id = Guid.NewGuid();
            Amount = amount;
            Type = type;
            TransactionDate = transactionDate;
            CreatedAt = DateTime.UtcNow;
            CompetenceYear = competenceYear;
            CompetenceMonth = competenceMonth;
            Description = description;
            Status = TransactionStatus.Pending;
            AccountId = accountId;
            TransferId = transferId;
            CategoryId = categoryId;
            RecurringTemplateId = recurringTemplateId;
        }
        public void Confirm()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException(
                    "Apenas transações pendentes podem ser confirmadas.");

            Status = TransactionStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status != TransactionStatus.Pending)
                throw new BusinessRuleException("Apenas transações pendentes podem ser canceladas.");

            Status = TransactionStatus.Cancelled;
        }

        public void Update(
        decimal amount,
        TransactionType type,
        DateTime transactionDate,
        int competenceYear,
        int competenceMonth,
        string description,
        Guid accountId,
        Guid? categoryId)
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Apenas transações pendentes podem ser editadas.");

            if (amount <= 0)
                throw new ArgumentException("Valor precisa ser maior que zero.");

            if (competenceMonth < 1 || competenceMonth > 12)
                throw new ArgumentException("Mês de competência inválido.");

            if (competenceYear < 2000 || competenceYear > 2100)
                throw new ArgumentException("Ano de competência inválido.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição é obrigatória.");

            if (description.Length > 200)
                throw new ArgumentException("Descrição pode ter no máximo 200 caracteres.");

            Amount = amount;
            Type = type;
            TransactionDate = transactionDate;
            CompetenceYear = competenceYear;
            CompetenceMonth = competenceMonth;
            Description = description;
            AccountId = accountId;
            CategoryId = categoryId;
        }
    }
}
