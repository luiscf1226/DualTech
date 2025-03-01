# PruebaTecnica Docker Setup

This repository contains a Docker Compose configuration to run the .NET Web API with SQL Server.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) installed on your machine
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) installed for local development (optional)

## Step-by-Step Instructions

### 1. Generate HTTPS Development Certificate (Required for HTTPS)

Run the following commands to generate a development certificate for HTTPS:

```bash
# For Windows PowerShell
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p password
dotnet dev-certs https --trust

# For macOS/Linux
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
dotnet dev-certs https --trust
```

### 2. Make the setup script executable (Optional)

This step is only needed if you want to use the SQL initialization script approach. The application now supports automatic Entity Framework migrations.

```bash
# For macOS/Linux
chmod +x setup-db.sh
```

### 3. Build and Run the Docker Containers

```bash
# Build and start the containers
docker-compose up -d --build

# To view logs
docker-compose logs -f
```

### 4. Access the Application

- API: https://localhost:8081/swagger
- SQL Server:
  - Server: localhost,1433
  - Username: sa
  - Password: YourStrong!Passw0rd
  - Database: PruebaTecnicaDb

### 5. Stop the Containers

```bash
docker-compose down
```

To remove volumes as well:

```bash
docker-compose down -v
```

## Database Initialization

The database is automatically initialized in two ways:

1. **Entity Framework Migrations (Primary Method)**
   - The application uses Entity Framework Core to automatically create and apply migrations
   - On startup, the application checks if the database exists and creates it if needed
   - It also seeds initial data if the database is empty

2. **SQL Script (Fallback Method)**
   - If the Entity Framework migrations fail, the SQL script can be used as a fallback
   - The SQL Server container includes an initialization script that creates the tables and adds sample data

The database includes the following tables:

1. **Cliente** - Customer information
   - ClienteId (PK)
   - Nombre (Name)
   - Identidad (Identity)

2. **Producto** - Product information
   - ProductoId (PK)
   - Nombre (Name)
   - Descripcion (Description, optional)
   - Precio (Price)
   - Existencia (Stock)

3. **Orden** - Order information
   - OrdenId (PK)
   - ClienteId (FK to Cliente)
   - Impuesto (Tax)
   - Subtotal
   - Total
   - FechaCreacion (Creation Date)

4. **DetalleOrden** - Order detail information
   - DetalleOrdenId (PK)
   - OrdenId (FK to Orden)
   - ProductoId (FK to Producto)
   - Cantidad (Quantity)
   - Impuesto (Tax)
   - Subtotal
   - Total

## Database Connection String

The application is configured to use the following connection string:

```
Server=db;Database=PruebaTecnicaDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
```

## Troubleshooting

### Certificate Issues

If you encounter certificate issues, make sure you've generated the development certificate correctly and that the path in the docker-compose.yml file matches your local path.

### SQL Server Connection Issues

If the API cannot connect to SQL Server, ensure that:
1. SQL Server container is running (`docker ps`)
2. The connection string in the environment variables is correct
3. SQL Server has fully started before the API attempts to connect

### Database Initialization Issues

If the database is not initialized properly:
1. Check the logs for any errors: `docker-compose logs db`
2. Check the API logs for migration errors: `docker-compose logs api`
3. The application will retry connecting to the database if it's not available initially
4. You can manually run the SQL script by connecting to the SQL Server container:
   ```bash
   docker exec -it <container_id> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -i /docker-entrypoint-initdb.d/init-db.sql
   ```

## Additional Information

- The SQL Server data is persisted in a Docker volume named `sqlserver-data`
- The API is configured to run in Development mode
- Both HTTP (8080) and HTTPS (8081) endpoints are exposed
- The API container will restart automatically if it fails to connect to the database initially
