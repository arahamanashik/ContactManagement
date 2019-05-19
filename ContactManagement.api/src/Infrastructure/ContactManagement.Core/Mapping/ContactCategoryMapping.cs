using ContactManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.Mapping
{
    public class ContactCategoryMapping : IEntityTypeConfiguration<ContactCategory>
    {
        public void Configure(EntityTypeBuilder<ContactCategory> builder)
        {
            builder.ToTable("ContactCategory", "dbo");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
               .HasColumnType("varchar(100)");
            builder.Property(t => t.Description)
               .HasColumnType("varchar(500)");

        }
    }
}
