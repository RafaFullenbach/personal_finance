using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class MonthClosing
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public DateTime ClosedAt { get; private set; }

        private MonthClosing() { } // EF

        public MonthClosing(int year, int month)
        {
            if (year < 2000 || year > 2100) throw new ArgumentException("Ano inválido.");
            if (month < 1 || month > 12) throw new ArgumentException("Mês inválido.");

            Id = Guid.NewGuid();
            Year = year;
            Month = month;
            ClosedAt = DateTime.UtcNow;
        }
    }
}
