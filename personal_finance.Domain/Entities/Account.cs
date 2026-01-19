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
            UpdateName(name);
            Id = Guid.NewGuid();
            Name = name.Trim();
            Type = type;
            IsActive = true;
        }

        public void Update(string name, AccountType type)
        {
            UpdateName(name);
            UpdateType(type);
        }


        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Account name is required.");

            var trimmed = name.Trim();

            if (trimmed.Length > 100)
                throw new BusinessRuleException("Name must be 100 characters or less.");

            Name = trimmed;
        }

        public void UpdateType(AccountType type)
        {
            Type = type;
        }

        public void Deactivate()
        {
            if (!IsActive) throw new BusinessRuleException("The account is already deactivated."); // idempotente (ou lançar regra se preferir)
            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive) throw new BusinessRuleException("The account is already active.");
            IsActive = true;
        }
    }
}
