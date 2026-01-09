using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public AccountType Type { get; private set; }
        public bool IsActive { get; private set; }

        private Account() { } // para ORMs no futuro

        public Account(string name, AccountType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Account name is required.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Type = type;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
