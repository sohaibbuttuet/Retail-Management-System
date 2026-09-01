# RMS (Retail Management System)

A robust, secure Windows Forms (WinForms) desktop application built with C# and MySQL, designed to streamline retail store operations, inventory tracking, customer management, and sales invoicing.

---

## Tech Stack & Architecture

* **Language:** C# (.NET Framework)
* **UI Framework:** Windows Forms (WinForms)
* **Database:** MySQL (`retail_management_system`)
* **Data Access:** ADO.NET (`MySql.Data`) with parameterized queries to prevent SQL injection.
* **Configuration:** XML-based separation (`App.config` and `secrets.config` for sensitive credentials).
* **Design Patterns:** Thread-safe Singleton pattern (`DatabaseHelper`) using `Lazy<T>`.

---

## Project Structure

```text
RMS/
├── BL/               # Business Logic Layer
├── DL/               # Data Access Layer (AdminDL, CategoryDL, CustomerDL, InvoiceDL, InvoiceItemDL, ProductDL)
├── Models/           # Entity Classes (Admin, Category, Customer, Invoice, InvoiceItem, Product)
├── UI/               # Windows Forms User Interface
├── App.config        # Application Configuration
├── secrets.config    # Database Connection Strings (Ignored by Git)
└── Program.cs        # Entry Point

```

---

## Core Features

* **Admin Authentication:** Secure login system with parameter-bound validation to protect against SQL injection.
* **Inventory & Stock Management:** Complete CRUD operations for products and categories, including low-stock alerts (< 10 units) and relational safety checks preventing the deletion of active categories or products tied to existing invoices.
* **Customer Management:** Maintain customer contact details, city tracking, purchase history lookup, and invoice association tracking.
* **Invoicing & Sales:** Real-time generation of invoices and itemized receipt tracking with support for total calculations and historical date-range reporting.

---

## Getting Started & Installation

### Prerequisites

* Visual Studio (with .NET desktop development workload installed)
* MySQL Server (Local or Remote)

### Setup Steps

1. **Clone the Repository:**
```bash
git clone <repository-url>

```


2. **Configure the Database:**
* Import your MySQL schema into a database named `retail_management_system`.


3. **Configure Database Secrets:**
* Create a file named `secrets.config` in the root directory (matching the structure of `App.config`).
* Add your connection string:
```xml
<connectionStrings>
    <add name="DefaultConnection" connectionString="Server=127.0.0.1;Database=retail_management_system;Uid=root;Pwd=your_password;" providerName="MySql.Data.MySqlClient" />
</connectionStrings>

```




4. **Open and Run:**
* Open `RMS.sln` in Visual Studio.
* Restore NuGet packages if prompted.
* Ensure `secrets.config` has its **Copy to Output Directory** property set to **Copy if newer**.
* Press **F5** to build and run the application.