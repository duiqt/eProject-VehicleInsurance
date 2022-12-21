using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class VipDbContext : DbContext
{
    public VipDbContext()
    {
    }

    public VipDbContext(DbContextOptions<VipDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<CompanyExpense> CompanyExpenses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerBill> CustomerBills { get; set; }

    public virtual DbSet<Estimate> Estimates { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<OptionDetail> OptionDetails { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Database=VIP_DB;Persist Security Info=True;Trusted_Connection=true;TrustServerCertificate=Yes;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE488968417BB");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserName, "UQ__Admin__C9F28456393AC753").IsUnique();

            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC27973BB7EF");

            entity.ToTable("Certificate");

            entity.HasIndex(e => e.PolicyNo, "UQ__Certific__2E132196817A8D3A").IsUnique();

            entity.HasIndex(e => e.EstimateId, "UQ__Certific__ABEBF4D4D893A36E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstimateId).HasColumnName("EstimateID");
            entity.Property(e => e.Prove)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.VehicleBodyNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleEngineNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleWarranty)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Not Available')");

            entity.HasOne(d => d.Estimate).WithOne(p => p.Certificate)
                .HasForeignKey<Certificate>(d => d.EstimateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimate_Certificate");
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Claim__3214EC275883A5CA");

            entity.ToTable("Claim");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
            entity.Property(e => e.ClaimableAmount).HasColumnType("money");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateOfAccident)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsuredAmount).HasColumnType("money");
            entity.Property(e => e.PlaceOfAccident)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Certificate).WithMany(p => p.Claims)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Claim_Certificate");
        });

        modelBuilder.Entity<CompanyExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company___3214EC275B1E8CFD");

            entity.ToTable("Company_Expense");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmountOfExpense).HasColumnType("money");
            entity.Property(e => e.DateOfExpense)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfExpense)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC2791CDC70F");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.CustomerPhone, "UQ__Customer__390618B376A4234E").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E41840B500").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27A0819DEE");

            entity.ToTable("CustomerBill");

            entity.HasIndex(e => e.BillNo, "UQ__Customer__11F284183DBB6AFF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
            entity.Property(e => e.CustomerAddProve)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Certificate).WithMany(p => p.CustomerBills)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificate_Bill");
        });

        modelBuilder.Entity<Estimate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Estimate__3214EC27326682F6");

            entity.ToTable("Estimate");

            entity.HasIndex(e => e.EstimateNo, "UQ__Estimate__ABEB835BA241940B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OptionDetailsId).HasColumnName("OptionDetailsID");
            entity.Property(e => e.PolicyDate).HasColumnType("date");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.Premium).HasColumnType("money");
            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleVersion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Estimate_Customer");

            entity.HasOne(d => d.OptionDetails).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.OptionDetailsId)
                .HasConstraintName("FK_Estimate_OptionDetails");

            entity.HasOne(d => d.Policy).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimate_Policy");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Option__3214EC270A080CE5");

            entity.ToTable("Option");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Annually).HasColumnType("money");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.OptionType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OptionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OptionDe__3214EC27B757CCA9");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Option1Navigation).WithMany(p => p.OptionDetailOption1Navigations)
                .HasForeignKey(d => d.Option1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OptionDetails_OptionID1");

            entity.HasOne(d => d.Option2Navigation).WithMany(p => p.OptionDetailOption2Navigations)
                .HasForeignKey(d => d.Option2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OptionDetails_OptionID2");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Policy__3214EC278310F190");

            entity.ToTable("Policy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Annually).HasColumnType("money");
            entity.Property(e => e.Coverage)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PolicyType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicle__3214EC27CAFD0295");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.VehicleNumber, "UQ__Vehicle__ABAD8859D72C09B0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VehicleBodyNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleEngineNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleOwnerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleRate).HasColumnType("money");
            entity.Property(e => e.VehicleVersion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
