using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transactions.ConfirmTransaction
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
                throw NotFoundException.For("Lançamento", command.TransactionId);

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
