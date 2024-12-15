using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace designclub.Models;

public partial class ComputerClubContext : DbContext
{
    public ComputerClubContext()
    {
    }

    public ComputerClubContext(DbContextOptions<ComputerClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Minutepackage> Minutepackages { get; set; }

    public virtual DbSet<Providedminutepackage> Providedminutepackages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(designclub.DatabaseConfiguration.GetConnectionString(), designclub.DatabaseConfiguration.ServerVersion);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.Phone, "Phone").IsUnique();

            entity.HasIndex(e => e.UserClient, "UserClient").IsUnique();

            entity.Property(e => e.BonusBalance).HasPrecision(10, 2);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Pass).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RoleClient)
                .HasMaxLength(50)
                .HasDefaultValueSql("'user'");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("games");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ExecutableFile).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Photo).HasMaxLength(255);
        });

        modelBuilder.Entity<Minutepackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("minutepackages");

            entity.Property(e => e.Cost).HasPrecision(10, 2);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Providedminutepackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("providedminutepackages");

            entity.HasIndex(e => e.ClientId, "ClientId");

            entity.HasIndex(e => e.PackageId, "PackageId");

            entity.Property(e => e.Cost).HasPrecision(10, 2);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Запланировано'");

            entity.HasOne(d => d.Client).WithMany(p => p.Providedminutepackages)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("providedminutepackages_ibfk_2");

            entity.HasOne(d => d.Package).WithMany(p => p.Providedminutepackages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("providedminutepackages_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "Login").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
