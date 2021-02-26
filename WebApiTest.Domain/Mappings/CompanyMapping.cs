using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.Mappings
{
    public class CompanyMapping : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company", "dbo");
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

            builder.HasMany(m => m.Departments)
                .WithOne()
                .HasForeignKey("CompanyId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
