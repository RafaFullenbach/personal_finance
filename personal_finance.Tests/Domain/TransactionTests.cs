using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;

namespace personal_finance.Tests.Domain
{
    public class TransactionTests
    {
        [Fact]
        public void Constructor_ShouldCreatePendingTransaction()
        {
            var tx = new Transaction(
                amount: 100m,
                type: TransactionType.Debit,
                transactionDate: DateTime.Today,
                competenceYear: 2026,
                competenceMonth: 1,
                description: "Groceries");

            Assert.NotEqual(Guid.Empty, tx.Id);
            Assert.Equal(TransactionStatus.Pending, tx.Status);
            Assert.Equal(100m, tx.Amount);
            Assert.Equal(TransactionType.Debit, tx.Type);
        }

        [Fact]
        public void Confirm_ShouldChangeStatusToConfirmed_WhenPending()
        {
            var tx = new Transaction(100m, TransactionType.Debit, DateTime.Today, 2026, 1, "Test");

            tx.Confirm();

            Assert.Equal(TransactionStatus.Confirmed, tx.Status);
        }

        [Fact]
        public void Cancel_ShouldChangeStatusToCancelled_WhenPending()
        {
            var tx = new Transaction(100m, TransactionType.Debit, DateTime.Today, 2026, 1, "Test");

            tx.Cancel();

            Assert.Equal(TransactionStatus.Cancelled, tx.Status);
        }

        [Fact]
        public void Confirm_ShouldThrow_WhenNotPending()
        {
            var tx = new Transaction(100m, TransactionType.Debit, DateTime.Today, 2026, 1, "Test");
            tx.Cancel();

            Assert.Throws<InvalidOperationException>(() => tx.Confirm());
        }
    }
}
