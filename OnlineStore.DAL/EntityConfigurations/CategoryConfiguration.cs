using OnlineStore.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.EntityConfigurations
{
    public class CategoryConfiguration
        : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasAlternateKey(x => x.SeederId);

            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}
