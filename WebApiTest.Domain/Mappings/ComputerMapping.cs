using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.Mappings
{
    public class ComputerMapping : IEntityTypeConfiguration<Computer>
    {
        public void Configure(EntityTypeBuilder<Computer> builder)
        {
            builder.ToTable("Computer", "dbo");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnType("uniqueidentifier");

            builder.Property(m => m.Code)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(m => m.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(m => m.DepartmentId)
                .HasColumnType("uniqueidentifier")
                .IsRequired(false);

            builder.Property(m => m.CreateTime)
                //.HasDefaultValueSql("getdate()")
                .IsRequired();
        }
    }
}
