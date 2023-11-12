using CacheSample.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CacheSample.Infra.DataAccess.EFCore.Mappings;
internal class StudentMap : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Student");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasColumnName("Cpf")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(12);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.BirthDay)
            .HasColumnName("BirthDay")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(10);

        builder.Property(x => x.CreateDate)
            .IsRequired()
            .HasColumnName("CreateDate")
            .HasColumnType("DateTime");

        builder.Property(x => x.UpdateDate)
            .HasColumnName("UpdateDate")
            .HasColumnType("DateTime");
    }
}


/*
 
drop table if exists  Student
go
CREATE TABLE [dbo].[Student]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Primary Key column
    [Name] NVARCHAR(100) NOT NULL,
    [Cpf] NVARCHAR(12) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Birthday] NVARCHAR(10) NULL,
    [CreateDate] DATETIME NOT NULL,
    [UpdateDate] DATETIME NULL
);
GO
select * from Student
 */