using ContactManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.Mapping
{
    public class ContactInfoMapping : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.ToTable("ContactInfo", "dbo");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
               .HasColumnType("varchar(100)");
            builder.Property(t => t.Email)
               .HasColumnType("varchar(100)");
            builder.Property(t => t.Mobile)
               .HasColumnType("varchar(20)");
            builder.Property(t => t.Address)
               .HasColumnType("varchar(500)");
            builder.Property(t => t.ProfilePicture)
               .HasColumnType("varchar(100)");
            builder.Property(t => t.ContactCategoryId)
               .HasColumnType("Int");
        }
    }
}
