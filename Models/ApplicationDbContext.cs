using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRUD_1st.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PK__Roles__CAFF40D29310A27F");

            entity.Property(e => e.Rid).HasColumnName("RId");
            entity.Property(e => e.Rname)
                .HasMaxLength(100)
                .HasColumnName("RName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PK__Users__C5B19662D70211E3");

            entity.Property(e => e.Uid).HasColumnName("UId");
            entity.Property(e => e.Uname)
                .HasMaxLength(100)
                .HasColumnName("UName");
            entity.Property(e => e.Upass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPass");

            entity.HasMany(d => d.Rids).WithMany(p => p.Uids)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("Rid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersRoles__RId__3C69FB99"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Uid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersRoles__UId__3B75D760"),
                    j =>
                    {
                        j.HasKey("Uid", "Rid").HasName("PK__UsersRol__F91E626FAC2023FC");
                        j.ToTable("UsersRoles");
                        j.IndexerProperty<int>("Uid").HasColumnName("UId");
                        j.IndexerProperty<int>("Rid").HasColumnName("RId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
