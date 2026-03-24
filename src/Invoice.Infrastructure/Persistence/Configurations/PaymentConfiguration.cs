// src/Invoice.Infrastructure/Persistence/Configurations/PaymentConfiguration.cs
using Invoice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoice.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration
    : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount).HasPrecision(18, 4);  // 18 digits total, 4 after the decimal point
        builder.Property(p => p.Reference).HasMaxLength(200);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.Method)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(p => p.InvoiceId);
        builder.HasIndex(p => p.PaidAt);  // Index on PaidAt for efficient querying of payments by date
    }
}