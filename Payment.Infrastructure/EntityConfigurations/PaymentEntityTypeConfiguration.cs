using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Payment.Infrastructure.EntityConfigurations
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.PaymentAggregate.Payment>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.PaymentAggregate.Payment> builder)
        {
            builder.ToTable("pay", PaymentContext.DEFAULT_SCHEMA);

            builder.HasKey(b => b.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("PaymentSequenceHiLo", PaymentContext.DEFAULT_SCHEMA);

            builder.Property<Guid>("OrderId")
                .IsRequired();

            builder.Property<Guid>("StoreId")
                .IsRequired();

            builder.Property<string>("PayType")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property<string>("CardNumber")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property<string>("CardOwnerName")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property<char>("SecurityCode")
                .HasMaxLength(3)
                .IsRequired();

            builder.Property<DateTime>("ExpirationDate")
                .IsRequired();

            builder.Property<Guid>("RequesterId")
                .IsRequired();
        }
    }
}
