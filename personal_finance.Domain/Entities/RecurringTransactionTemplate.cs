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

        public int DayOfMonth { get; private set; }
        public int CompetenceOffsetMonths { get; private set; }

        public string Description { get; private set; }

        public Guid AccountId { get; private set; }
        public Guid CategoryId { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private RecurringTransactionTemplate() { }

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
            if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
            if (accountId == Guid.Empty) throw new ArgumentException("Id da conta é obrigatório.");
            if (categoryId == Guid.Empty) throw new ArgumentException("Id da categoria é obrigatório.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Descrição é obrigatória.");
            if (description.Length > 200) throw new ArgumentException("Descrição deve ter no máximo 200 caracteres.");

            if (dayOfMonth < 1 || dayOfMonth > 28) throw new ArgumentException("Dia do mês deve estar entre 1 e 28.");

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
            if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
            if (accountId == Guid.Empty) throw new ArgumentException("Id da conta é obrigatório.");
            if (categoryId == Guid.Empty) throw new ArgumentException("Id da categoria é obrigatório.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Descrição é obrigatória.");
            if (description.Length > 200) throw new ArgumentException("Descrição deve ter no máximo 200 caracteres.");
            if (dayOfMonth < 1 || dayOfMonth > 28) throw new ArgumentException("Dia do mês deve estar entre 1 e 28.");

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
