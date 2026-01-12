using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class Budget
    {
        public Guid Id { get; private set; }
        public Guid CategoryId { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal LimitAmount { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Budget() { } // EF

        public Budget(Guid categoryId, int year, int month, decimal limitAmount)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentException("CategoryId is required.");

            if (year < 2000 || year > 2100)
                throw new ArgumentException("Invalid year.");

            if (month < 1 || month > 12)
                throw new ArgumentException("Invalid month.");

            if (limitAmount <= 0)
                throw new ArgumentException("LimitAmount must be greater than zero.");

            Id = Guid.NewGuid();
            CategoryId = categoryId;
            Year = year;
            Month = month;
            LimitAmount = limitAmount;
            IsActive = true;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLimit(decimal limitAmount)
        {
            if (limitAmount <= 0)
                throw new ArgumentException("LimitAmount must be greater than zero.");

            LimitAmount = limitAmount;
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
    }
}
