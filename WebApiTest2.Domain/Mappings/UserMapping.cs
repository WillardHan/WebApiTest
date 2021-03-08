using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebApiTest2.Domain.Models;

namespace WebApiTest2.Domain.Mappings
{
    public class ComputerMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "dbo");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnType("uniqueidentifier");

            builder.Property(m => m.Code)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(m => m.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(m => m.CreateTime)
                //.HasDefaultValueSql("getdate()")
                .IsRequired();
        }
    }
}
