using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CancelTransaction
{
    public class CancelTransactionHandler
    {
        private readonly ITransactionRepository _repository;

        public CancelTransactionHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<CancelTransactionResult> HandleAsync(
            CancelTransactionCommand command)
        {
            var transaction = await _repository.GetByIdAsync(command.TransactionId);

            if (transaction is null)
                throw NotFoundException.For("Transaction", command.TransactionId);

            // Domínio decide se pode cancelar
            transaction.Cancel();

            await _repository.UpdateAsync(transaction);

            return new CancelTransactionResult
            {
                TransactionId = transaction.Id,
                Status = transaction.Status.ToString()
            };
        }
    }
}
