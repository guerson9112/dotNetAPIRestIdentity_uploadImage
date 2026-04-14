# E-Commerce API Pro (.NET 10 + ASP.NET Core Identity)

A high-performance and secure RESTful API built with **ASP.NET Core 10**. This project simulates a real-world E-commerce backend, managing a catalog of products organized by categories, protected by an enterprise-grade security layer.

## 📦 Business Logic & Data Model

The API is designed to manage a structured catalog where:
- **Products**: Detailed items containing names, descriptions, pricing, and dynamic image management.
- **Categories**: Classification system to organize the inventory.
- **Entity Relationships**: Implements a **One-to-Many** relationship (1:N) ensuring every product belongs to a specific category for optimized search and filtering.

## 🚀 Pro Features

- **Product & Category Management**: Full CRUD operations with related data handling (Eager Loading).
- **Enterprise Security (Identity)**: Robust user management using **ASP.NET Core Identity**, including automatic password hashing, role-based access control (RBAC), and security claims.
- **Dynamic Image Upload System**: 
  - **Automated Processing**: Handles file streams using `IFormFile`.
  - **Collision Prevention**: Generates unique filenames using **GUIDs**.
  - **Directory Management**: Auto-creates infrastructure in `wwwroot/ProductsImages`.
  - **Dual-Path Persistence**: Stores both the local physical path (`ImgUrlLocal`) and the public access URL (`ImgUrl`).
- **Interactive Documentation**: **Swagger UI (Swashbuckle v7)** configured for OpenAPI v1.6 with full Bearer Token support.
- **Advanced Performance**: Centralized **Response Caching** strategy (10s and 20s profiles) for high-traffic endpoints.

## 🛠️ Tech Stack

* **Runtime**: .NET 10.0
* **Security**: ASP.NET Core Identity & JWT Bearer Authentication
* **Database**: Microsoft SQL Server 2022
* **ORM**: Entity Framework Core 10
* **Mapping**: AutoMapper 14.0.0
* **Storage**: Local File System (`wwwroot`)

## ⚡ Performance & Optimization (Response Caching)

The API utilizes centralized **Cache Profiles** to ensure high availability:
- **Profiles**: `Default10` for dynamic products and `Default20` for categories.
- **Implementation**: Configured globally in `Program.cs` and managed via a `Constants/CacheProfiles.cs` class.

## ⚙️ Getting Started

### Prerequisites
- .NET 10 SDK
- Docker Desktop

### Installation & DB Setup

1. **Navigate to the Project Folder**:
   cd ApiEcommerce

2. **Spin up the Database ("The Recipe")**:
- Run the following command where the docker-compose.yaml is located:
    docker-compose up -d

3. **Apply Migrations**:
    dotnet ef database update

4. **Run Application**:
    dotnet run

## 📁 Project Structure Highlights

- **`Controllers/V1 & V2`**: Versioned API endpoints.
- **`wwwroot/ProductsImages`**: Local storage for uploaded product media (GUID-named files).
- **`Constants`**: Centralized configuration for Cache Profiles and Policy Names.
- **`docker-compose.yaml`**: Containerized SQL Server 2022 configuration.