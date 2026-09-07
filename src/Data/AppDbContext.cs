using System;
using DesktopPos.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopPos.Data;

public class AppDbContext : DbContext
{
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SKU).IsUnique();
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StockQuantity).IsRequired();
            entity.Property(e => e.ReorderThreshold).IsRequired();
            entity.Property(e => e.LastUpdated).IsRequired();
        });
    }
}

