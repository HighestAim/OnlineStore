using OnlineStore.Core;
using OnlineStore.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.EntityConfigurations
{
    public class OrderProductConfiguration 
        : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.HasOne(x => x.Product)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
