using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

using Type = WebAPI.Models.Type;

namespace WebAPI.Context;

public partial class AccessibilityDbContext : DbContext
{
    public AccessibilityDbContext()
    {
    }

    public AccessibilityDbContext(DbContextOptions<AccessibilityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityAccessibility> ActivityAccessibilities { get; set; }

    public virtual DbSet<Accessibility> Accessibilities { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accessibility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accessib__3214EC2729895D37");

            entity.ToTable("Accessibility");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ActivityAccessibility>(entity =>
        {
            entity.HasKey(e => new { e.ActivityId, e.AccessibilityId });
            
            entity.ToTable("Activity_Accessibility");

            entity.HasOne(e => e.Activity)
                .WithMany(a => a.ActivityAccessibilities)
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Accessibility)
                .WithMany(a => a.ActivityAccessibilities)
                .HasForeignKey(e => e.AccessibilityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.AccessibilityId).HasColumnName("AccessibilityID");
        });


        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Activity__3214EC279DC9E491");

            entity.ToTable("Activity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Activities)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Activity__TypeID__3B75D760");            
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC27D8C1E683");

            entity.ToTable("Log");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Level).HasMaxLength(20);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC279E2E71F8");

            entity.ToTable("Review");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Activity).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK__Review__Activity__2180FB33");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__UserID__208CD6FA");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC27D8FBE94A");

            entity.ToTable("Type");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC278D5DBB28");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534AA76B35F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsAdmin)
                .HasMaxLength(10)
                .HasDefaultValue("false");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PasswordSalt).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
