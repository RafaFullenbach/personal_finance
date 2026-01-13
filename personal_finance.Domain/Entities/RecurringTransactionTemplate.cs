using personal_finance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class RecurringTransactionTemplate
    {
        public Guid Id { get; private set; }

        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public int DayOfMonth { get; private set; } // 1..28 (MVP seguro)
        public int CompetenceOffsetMonths { get; private set; } // 0 = mesmo mês; opcional pra salário cair dia 5 mas competência 1

        public string Description { get; private set; }

        public Guid AccountId { get; private set; }
        public Guid CategoryId { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private RecurringTransactionTemplate() { } // EF

        public RecurringTransactionTemplate(
            decimal amount,
            TransactionType type,
            Guid accountId,
            Guid categoryId,
            string description,
            int dayOfMonth,
            DateTime startDate,
            DateTime? endDate = null,
            int competenceOffsetMonths = 0)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (accountId == Guid.Empty) throw new ArgumentException("AccountId is required.");
            if (categoryId == Guid.Empty) throw new ArgumentException("CategoryId is required.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.");
            if (description.Length > 200) throw new ArgumentException("Description must be 200 characters or less.");

            // MVP: 1..28 para não ter dor com mês curto (fev)
            if (dayOfMonth < 1 || dayOfMonth > 28) throw new ArgumentException("DayOfMonth must be between 1 and 28.");

            Id = Guid.NewGuid();
            Amount = amount;
            Type = type;
            AccountId = accountId;
            CategoryId = categoryId;
            Description = description.Trim();
            DayOfMonth = dayOfMonth;
            StartDate = startDate.Date;
            EndDate = endDate?.Date;
            CompetenceOffsetMonths = competenceOffsetMonths;

            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(
            decimal amount,
            TransactionType type,
            Guid accountId,
            Guid categoryId,
            string description,
            int dayOfMonth,
            DateTime startDate,
            DateTime? endDate,
            int competenceOffsetMonths)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (accountId == Guid.Empty) throw new ArgumentException("AccountId is required.");
            if (categoryId == Guid.Empty) throw new ArgumentException("CategoryId is required.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.");
            if (description.Length > 200) throw new ArgumentException("Description must be 200 characters or less.");
            if (dayOfMonth < 1 || dayOfMonth > 28) throw new ArgumentException("DayOfMonth must be between 1 and 28.");

            Amount = amount;
            Type = type;
            AccountId = accountId;
            CategoryId = categoryId;
            Description = description.Trim();
            DayOfMonth = dayOfMonth;
            StartDate = startDate.Date;
            EndDate = endDate?.Date;
            CompetenceOffsetMonths = competenceOffsetMonths;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
