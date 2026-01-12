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
                throw new ArgumentException("Category name is required.");

            if (name.Length > 100)
                throw new ArgumentException("Category name must be 100 characters or less.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Type = type;
            IsActive = true;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            if (name.Length > 100)
                throw new ArgumentException("Category name must be 100 characters or less.");

            Name = name.Trim();
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
