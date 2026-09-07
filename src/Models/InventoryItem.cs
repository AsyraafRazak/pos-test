using System;
using System.ComponentModel.DataAnnotations;

namespace DesktopPos.Models;

public class InventoryItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string SKU { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public int ReorderThreshold { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

