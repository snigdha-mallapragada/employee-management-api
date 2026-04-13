using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Models;

public partial class EmployeeManageDbContext : DbContext
{
    public EmployeeManageDbContext()
    {
    }

    public EmployeeManageDbContext(DbContextOptions<EmployeeManageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepartmentTbl> DepartmentTbls { get; set; }

    public virtual DbSet<DesignationTbl> DesignationTbls { get; set; }

    public virtual DbSet<EmployeeTbl> EmployeeTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=LIN76005526;Initial Catalog=employeeManageDb;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepartmentTbl>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);

            entity.ToTable("departmentTbl");

            entity.Property(e => e.DepartmentId).HasColumnName("departmentId");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("departmentName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<DesignationTbl>(entity =>
        {
            entity.HasKey(e => e.DesignationId);

            entity.ToTable("designationTbl");

            entity.Property(e => e.DesignationId).HasColumnName("designationId");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentId");
            entity.Property(e => e.DesignationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("designationName");

            entity.HasOne(d => d.Department).WithMany(p => p.DesignationTbls)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_designationTbl_departmentTbl");
        });

        modelBuilder.Entity<EmployeeTbl>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.ToTable("employeeTbl");

            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.AltContactNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("altContactNo");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("contactNo");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DesignationId).HasColumnName("designationId");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("email");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modifiedDate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Pincode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("pincode");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("role");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state");

            entity.HasOne(d => d.Designation).WithMany(p => p.EmployeeTbls)
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employeeTbl_designationTbl");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
