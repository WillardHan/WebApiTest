using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.Mappings
{
    public class DepartmentMapping : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department", "dbo");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnType("uniqueidentifier");

            builder.Property<Guid>("CompanyId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

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
