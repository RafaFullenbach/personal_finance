using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType("TEXT"); // SQLite: decimal -> TEXT geralmente evita problemas de precisão

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CompetenceYear)
                .IsRequired();

            builder.Property(x => x.CompetenceMonth)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.AccountId)
                .IsRequired(); // você tornou obrigatório

            builder.Property(x => x.TransferId)
                .IsRequired(false);

            builder.HasIndex(x => x.AccountId);
            builder.HasIndex(x => x.TransferId);
            builder.HasIndex(x => x.TransactionDate);

            builder.Property(x => x.CategoryId).IsRequired(false);
            builder.HasIndex(x => x.CategoryId);
        }
    }
}
