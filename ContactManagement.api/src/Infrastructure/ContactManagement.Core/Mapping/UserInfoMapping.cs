using ContactManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.Mapping
{
    public class UserInfoMapping : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo", "dbo");
            builder.HasKey(t => t.Id);


            builder.Property(t => t.Name)
               .IsRequired()
               .HasColumnType("varchar(80)");
            builder.Property(t => t.Email)
               .IsRequired()
               .HasColumnType("varchar(100)");
            builder.Property(t => t.Mobile)
               .HasColumnType("varchar(20)");
            builder.Property(t => t.Address)
               .HasColumnType("varchar(500)");
            builder.Property(t => t.PasswordHashed)
                .IsRequired()
               .HasColumnType("varchar(80)");
            builder.Property(t => t.PasswordSalt)
                .IsRequired()
               .HasColumnType("nvarchar(max)");
            builder.Property(t => t.ProviderName)
               .HasColumnType("varchar(20)");

        }

    }
}
