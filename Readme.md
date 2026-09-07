# Desktop POS System

A modern, fast, and lightweight Point of Sale (POS) desktop application built with C# and .NET, utilizing **SQLite** for local database management and optimized for development with **VS Code** and **Google Antigravity**.

---

## 📌 Project Overview

This desktop POS system is designed to handle core retail operations, product inventory management, sales processing, and receipt generation with offline capability powered by a local SQLite database.

---

## ✨ Features

- **Sales & Checkout Interface:** Fast order entry, item search, line-item quantity adjustments, discounts, and order summary calculation.
- **Inventory Management:** Full CRUD (Create, Read, Update, Delete) operations for products, stock level tracking, categories, and low-stock alerts.
- **SQLite Storage:** Lightweight, single-file local database requiring zero external database server setup.
- **Receipt & Invoice Output:** Transaction logging and printable digital receipt summaries.
- **Agentic AI Assisted:** Pre-configured for seamless workflow integration with **Google Antigravity** agents.

---

## 🛠️ Tech Stack & Prerequisites

### Tech Stack
- **Language:** C#
- **Framework:** .NET 8.0+ (WPF / .NET MAUI / WinForms)
- **Database:** SQLite
- **ORM:** Entity Framework Core (EF Core)

### Prerequisites
Before running the application, ensure you have the following installed on your machine:

1. **[.NET 8.0 SDK or higher](https://dotnet.microsoft.com/download)**
2. **[Visual Studio Code](https://code.visualstudio.com/)**
3. **VS Code Extensions:**
   - [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) / C# Dev Kit
   - [SQLite Viewer](https://marketplace.visualstudio.com/items?itemName=qwtel.sqlite-viewer) *(Optional, for inspecting `.db` files)*
   - [Google Antigravity Extension](https://marketplace.visualstudio.com/) *(Optional, for AI-assisted development)*

---

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/desktop-pos.git
cd desktop-pos
```

### 2. Restore Dependencies
Open the integrated terminal in VS Code (`Ctrl + ~` / `Cmd + ~`) and execute:
```bash
dotnet restore
```

### 3. Database Migration & Setup
Run Entity Framework Core migrations to initialize your local SQLite database:
```bash
# Install EF Core CLI tools if not already installed globally
dotnet tool install --global dotnet-ef

# Apply migrations and create SQLite database
dotnet ef database update
```

### 4. Build and Run
Execute the application from the VS Code terminal:
```bash
dotnet run
```

---

## 📁 Recommended Project Structure

```text
desktop-pos/
├── src/
│   ├── Assets/             # Icons, images, and UI styling assets
│   ├── Data/               # EF Core DbContext & SQLite migrations
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   ├── Models/             # Product, Order, Inventory, and User domain models
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   └── InventoryItem.cs
│   ├── Services/           # Business logic (Inventory management, Checkout process)
│   ├── Views/              # Desktop UI windows/pages
│   └── App.xaml / Program.cs
├── app.db                  # Local SQLite database file (generated at runtime)
├── .vscode/                # VS Code launch and task configurations
├── README.md
└── desktop-pos.sln
```

---

## 🤖 Development with Google Antigravity

If you are using **Google Antigravity** in VS Code:
- Open the **Antigravity Panel** inside VS Code.
- Use background agents to automatically create EF Core models, refactor UI components, write unit tests, or run terminal scripts (`dotnet ef migrations add`, `dotnet test`).
- Prompt example:
  > *"Create a new EF Core model for InventoryItem with fields for SKU, StockQuantity, ReorderThreshold, and LastUpdated, then generate a migration."*

---

## 🤝 Contributing

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.