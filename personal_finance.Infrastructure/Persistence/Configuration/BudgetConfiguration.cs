using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Configuration
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.Month).IsRequired();
            builder.Property(x => x.LimitAmount).IsRequired().HasColumnType("TEXT");
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasIndex(x => new { x.CategoryId, x.Year, x.Month }).IsUnique();
        }
    }
}
