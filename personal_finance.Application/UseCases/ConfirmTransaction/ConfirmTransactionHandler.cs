using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ConfirmTransaction
{
    public class ConfirmTransactionHandler
    {
        private readonly ITransactionRepository _repository;

        public ConfirmTransactionHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ConfirmTransactionResult> HandleAsync(ConfirmTransactionCommand command)
        {
            var transaction = await _repository.GetByIdAsync(command.TransactionId);

            if (transaction is null)
                throw new InvalidOperationException("Transaction not found.");

            // Domain controla a transição de estado
            transaction.Confirm();

            await _repository.UpdateAsync(transaction);

            return new ConfirmTransactionResult
            {
                TransactionId = transaction.Id,
                Status = transaction.Status.ToString()
            };
        }
    }
}
