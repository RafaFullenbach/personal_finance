using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Configuration
{
    public class RecurringTransactionTemplateConfiguration : IEntityTypeConfiguration<RecurringTransactionTemplate>
    {
        public void Configure(EntityTypeBuilder<RecurringTransactionTemplate> builder)
        {
            builder.ToTable("RecurringTransactionTemplates");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount).IsRequired().HasColumnType("TEXT");
            builder.Property(x => x.Type).IsRequired().HasConversion<int>();

            builder.Property(x => x.Description).IsRequired().HasMaxLength(200);

            builder.Property(x => x.DayOfMonth).IsRequired();
            builder.Property(x => x.CompetenceOffsetMonths).IsRequired();

            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();

            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired(false);

            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasIndex(x => new { x.AccountId, x.CategoryId });
            builder.HasIndex(x => new { x.IsActive, x.StartDate });
        }
    }
}
