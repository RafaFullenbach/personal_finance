using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Configuration
{
    public class MonthClosingConfiguration : IEntityTypeConfiguration<MonthClosing>
    {
        public void Configure(EntityTypeBuilder<MonthClosing> builder)
        {
            builder.ToTable("MonthClosings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.Month).IsRequired();
            builder.Property(x => x.ClosedAt).IsRequired();

            builder.HasIndex(x => new { x.Year, x.Month }).IsUnique();
        }
    }
}
