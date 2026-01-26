using personal_finance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public CategoryType Type { get; private set; }
        public bool IsActive { get; private set; }

        private Category() { } // EF

        public Category(string name, CategoryType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome da categoria é obrigatório.");

            if (name.Length > 100)
                throw new ArgumentException("O nome da categoria deve ter no máximo 100 caracteres.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Type = type;
            IsActive = true;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome da categoria é obrigatório.");

            if (name.Length > 100)
                throw new ArgumentException("O nome da categoria deve ter no máximo 100 caracteres.");

            Name = name.Trim();
        }

        public void ChangeType(CategoryType type)
        {
            Type = type;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
