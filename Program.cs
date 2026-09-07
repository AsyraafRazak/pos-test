using System;
using System.Linq;
using DesktopPos.Data;
using DesktopPos.Models;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
await db.Database.EnsureCreatedAsync();

while (true)
{
    Console.Clear();
    Console.WriteLine("========================================");
    Console.WriteLine("        POS INVENTORY MANAGEMENT        ");
    Console.WriteLine("========================================");
    Console.WriteLine("1. View All Inventory Items");
    Console.WriteLine("2. Add New Inventory Item");
    Console.WriteLine("3. Update Item Stock Quantity");
    Console.WriteLine("4. Check Low Stock Alerts");
    Console.WriteLine("5. Seed Sample Data");
    Console.WriteLine("6. Exit");
    Console.WriteLine("========================================");
    Console.Write("Select an option (1-6): ");

    var choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            await ListInventoryItemsAsync(db);
            break;
        case "2":
            await AddInventoryItemAsync(db);
            break;
        case "3":
            await UpdateStockQuantityAsync(db);
            break;
        case "4":
            await CheckLowStockAlertsAsync(db);
            break;
        case "5":
            await SeedSampleDataAsync(db);
            break;
        case "6":
            Console.WriteLine("\nGoodbye!");
            return;
        default:
            Console.WriteLine("\nInvalid option. Press any key to continue...");
            Console.ReadKey();
            break;
    }
}

static async Task ListInventoryItemsAsync(AppDbContext db)
{
    Console.WriteLine("\n--- Current Inventory ---");
    var items = await db.InventoryItems.OrderBy(i => i.SKU).ToListAsync();

    if (items.Count == 0)
    {
        Console.WriteLine("No items found in inventory.");
    }
    else
    {
        Console.WriteLine($"{"ID",-5} {"SKU",-20} {"Stock",-10} {"Threshold",-12} {"Status",-12} {"Last Updated"}");
        Console.WriteLine(new string('-', 80));
        foreach (var item in items)
        {
            var status = item.StockQuantity <= item.ReorderThreshold ? "[LOW STOCK]" : "OK";
            Console.WriteLine($"{item.Id,-5} {item.SKU,-20} {item.StockQuantity,-10} {item.ReorderThreshold,-12} {status,-12} {item.LastUpdated:yyyy-MM-dd HH:mm:ss}");
        }
    }

    Console.WriteLine("\nPress any key to return to menu...");
    Console.ReadKey();
}

static async Task AddInventoryItemAsync(AppDbContext db)
{
    Console.WriteLine("\n--- Add New Inventory Item ---");
    Console.Write("Enter SKU (e.g. PROD-001): ");
    var sku = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(sku))
    {
        Console.WriteLine("SKU cannot be empty.");
        Wait();
        return;
    }

    if (await db.InventoryItems.AnyAsync(i => i.SKU.ToLower() == sku.ToLower()))
    {
        Console.WriteLine($"An item with SKU '{sku}' already exists.");
        Wait();
        return;
    }

    Console.Write("Enter Initial Stock Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
    {
        Console.WriteLine("Invalid quantity.");
        Wait();
        return;
    }

    Console.Write("Enter Reorder Threshold: ");
    if (!int.TryParse(Console.ReadLine(), out int threshold) || threshold < 0)
    {
        Console.WriteLine("Invalid reorder threshold.");
        Wait();
        return;
    }

    var newItem = new InventoryItem
    {
        SKU = sku,
        StockQuantity = quantity,
        ReorderThreshold = threshold,
        LastUpdated = DateTime.UtcNow
    };

    db.InventoryItems.Add(newItem);
    await db.SaveChangesAsync();

    Console.WriteLine($"\nSuccessfully added item '{sku}' (ID: {newItem.Id}).");
    Wait();
}

static async Task UpdateStockQuantityAsync(AppDbContext db)
{
    Console.WriteLine("\n--- Update Stock Quantity ---");
    Console.Write("Enter SKU: ");
    var sku = Console.ReadLine()?.Trim();

    var item = await db.InventoryItems.FirstOrDefaultAsync(i => i.SKU.ToLower() == (sku ?? "").ToLower());
    if (item == null)
    {
        Console.WriteLine($"Item with SKU '{sku}' not found.");
        Wait();
        return;
    }

    Console.WriteLine($"Current Stock for {item.SKU}: {item.StockQuantity} (Reorder Threshold: {item.ReorderThreshold})");
    Console.Write("Enter New Stock Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int newQuantity) || newQuantity < 0)
    {
        Console.WriteLine("Invalid quantity.");
        Wait();
        return;
    }

    item.StockQuantity = newQuantity;
    item.LastUpdated = DateTime.UtcNow;

    await db.SaveChangesAsync();
    Console.WriteLine($"\nStock for '{item.SKU}' updated to {item.StockQuantity}.");
    Wait();
}

static async Task CheckLowStockAlertsAsync(AppDbContext db)
{
    Console.WriteLine("\n--- Low Stock Alerts ---");
    var lowStockItems = await db.InventoryItems
        .Where(i => i.StockQuantity <= i.ReorderThreshold)
        .OrderBy(i => i.StockQuantity)
        .ToListAsync();

    if (lowStockItems.Count == 0)
    {
        Console.WriteLine("All stock levels are above their reorder thresholds.");
    }
    else
    {
        Console.WriteLine($"{"ID",-5} {"SKU",-20} {"Stock",-10} {"Threshold",-12} {"Needs Reorder"}");
        Console.WriteLine(new string('-', 60));
        foreach (var item in lowStockItems)
        {
            var deficit = item.ReorderThreshold - item.StockQuantity;
            Console.WriteLine($"{item.Id,-5} {item.SKU,-20} {item.StockQuantity,-10} {item.ReorderThreshold,-12} +{deficit} units");
        }
    }

    Wait();
}

static async Task SeedSampleDataAsync(AppDbContext db)
{
    Console.WriteLine("\n--- Seeding Sample Inventory Items ---");
    if (await db.InventoryItems.AnyAsync())
    {
        Console.Write("Inventory already has items. Do you want to add more sample items? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y")
        {
            Wait();
            return;
        }
    }

    var sampleItems = new[]
    {
        new InventoryItem { SKU = "BARCODE-SCAN-01", StockQuantity = 15, ReorderThreshold = 5, LastUpdated = DateTime.UtcNow },
        new InventoryItem { SKU = "THERMAL-ROLL-80MM", StockQuantity = 4, ReorderThreshold = 10, LastUpdated = DateTime.UtcNow },
        new InventoryItem { SKU = "CASH-DRAWER-RJ11", StockQuantity = 2, ReorderThreshold = 3, LastUpdated = DateTime.UtcNow },
        new InventoryItem { SKU = "CARD-READER-USB", StockQuantity = 25, ReorderThreshold = 5, LastUpdated = DateTime.UtcNow }
    };

    foreach (var item in sampleItems)
    {
        if (!await db.InventoryItems.AnyAsync(i => i.SKU == item.SKU))
        {
            db.InventoryItems.Add(item);
        }
    }

    await db.SaveChangesAsync();
    Console.WriteLine("Sample data added successfully!");
    Wait();
}

static void Wait()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}
