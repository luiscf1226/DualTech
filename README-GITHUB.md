# PruebaTecnica - .NET Web API with SQL Server

A .NET Web API project with SQL Server database, following Clean Architecture principles and SOLID design patterns.

## Project Structure

- **PruebaTecnica.API**: Web API controllers and presentation layer
- **PruebaTecnica.Application**: Application services, interfaces, and DTOs
- **PruebaTecnica.Domain**: Domain entities and business logic
- **PruebaTecnica.Infrastructure**: Data access, repositories, and external services
- **PruebaTecnica.Shared**: Shared components and utilities

## Features

- Clean Architecture with proper separation of concerns
- Entity Framework Core for data access
- Repository pattern for data abstraction
- Automatic database migrations and seeding
- Docker support for both the API and SQL Server
- Swagger UI for API documentation

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) installed on your machine
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) installed for local development (optional)

### Running with Docker

1. Clone the repository
2. Generate HTTPS development certificate:
   ```bash
   dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
   dotnet dev-certs https --trust
   ```
3. Run the application:
   ```bash
   docker-compose up -d --build
   ```
4. Access the API at https://localhost:8081/swagger

### Running Locally

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your local SQL Server
3. Run the application:
   ```bash
   dotnet run --project PruebaTecnica.API
   ```

## Database

The application automatically creates and initializes the database on startup using Entity Framework Core migrations. The database includes the following tables:

- **Cliente**: Customer information
- **Producto**: Product information
- **Orden**: Order information
- **DetalleOrden**: Order detail information

## License

This project is licensed under the MIT License - see the LICENSE file for details. 