# Expense Tracker API

A comprehensive REST API for managing personal finances, built with ASP.NET Core. Track your income, expenses, categories, and get real-time balance calculations.

## 📋 Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Models](#models)
- [Configuration](#configuration)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

- **Income Management**: Add, update, delete, and retrieve income records
- **Expense Tracking**: Comprehensive expense management with categorization
- **Category System**: Organize expenses with custom categories
- **User Management**: User registration and profile management
- **Balance Calculation**: Real-time balance tracking across all accounts
- **RESTful API**: Clean, intuitive API design following REST principles
- **Swagger Documentation**: Interactive API documentation and testing
- **Data Validation**: Robust input validation and error handling

## 🚀 Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Language**: C#
- **Documentation**: Swagger/OpenAPI
- **Architecture**: RESTful API with MVC pattern
- **Data Storage**: In-memory collections (development)

## 🏁 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- IDE: Visual Studio, Visual Studio Code, or JetBrains Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/expense-tracker-api.git
   cd expense-tracker-api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

### Running the Application

1. **Start the development server**
   ```bash
   dotnet run
   ```

2. **Access the application**
   - API Base URL: `http://localhost:5039`
   - Swagger UI: `http://localhost:5039/swagger`
   - HTTPS: `https://localhost:7089`

## 📖 API Documentation

The API includes comprehensive Swagger documentation. Once the application is running, visit:
- **Swagger UI**: `http://localhost:5039/swagger`
- **OpenAPI Spec**: `http://localhost:5039/swagger/v1/swagger.json`

## 🔗 API Endpoints

### Balance
- `GET /Balance/total` - Get total balance

### Categories
- `GET /Category` - Get all categories
- `GET /Category/{id}` - Get category by ID
- `POST /Category` - Create new category
- `PUT /Category/{id}` - Update category
- `DELETE /Category/{id}` - Delete category

### Expenses
- `GET /Expense` - Get all expenses
- `GET /Expense/{id}` - Get expense by ID
- `POST /Expense` - Create new expense
- `PUT /Expense/{id}` - Update expense
- `DELETE /Expense/{id}` - Delete expense

### Income
- `GET /Income` - Get all income records
- `GET /Income/{id}` - Get income by ID
- `POST /Income` - Create new income
- `PUT /Income/{id}` - Update income
- `DELETE /Income/{id}` - Delete income

### Users
- `GET /User` - Get all users
- `GET /User/{id}` - Get user by ID
- `POST /User` - Create new user
- `PUT /User/{id}` - Update user
- `DELETE /User/{id}` - Delete user

## 📊 Models

### Expense
```csharp
{
  "id": "guid",
  "amount": "decimal",
  "date": "datetime",
  "note": "string",
  "category": "Category"
}
```

### Income
```csharp
{
  "id": "guid",
  "amount": "decimal",
  "date": "datetime",
  "note": "string"
}
```

### Category
```csharp
{
  "id": "guid",
  "name": "string"
}
```

### User
```csharp
{
  "id": "guid",
  "username": "string",
  "email": "string",
  "passwordHash": "string"
}
```

## ⚙️ Configuration

The application uses standard ASP.NET Core configuration:

- `appsettings.json` - Production settings
- `appsettings.Development.json` - Development settings
- `launchSettings.json` - Launch profiles

## 🛠️ Development

### Building
```bash
dotnet build
```

### Code Formatting
The project follows standard C# coding conventions.

## 📋 Future Enhancements

- [ ] Database integration (Entity Framework Core)
- [ ] Authentication & Authorization (JWT)
- [ ] Data persistence
- [ ] Unit and integration tests

---

