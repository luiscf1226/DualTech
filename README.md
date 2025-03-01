# PruebaTecnica Docker Setup

This repository contains a Docker Compose configuration to run the .NET Web API with SQL Server.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) installed on your machine
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) installed for local development (optional)
- [Git](https://git-scm.com/downloads) for cloning the repository (optional)

## Detailed Setup Instructions

### Windows Setup

1. **Install Prerequisites**
   - Download and install [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/)
   - Download and install [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Ensure Docker Desktop is running (check the system tray icon)

2. **Generate HTTPS Development Certificate**
   - Open PowerShell as Administrator
   - Run the following commands:
     ```powershell
     dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p password
     dotnet dev-certs https --trust
     ```
   - Confirm any security prompts that appear

3. **Clone and Navigate to the Repository**
   - Open Command Prompt or PowerShell
   - Clone the repository (if not already done):
     ```powershell
     git clone <repository-url>
     cd PruebaTecnica
     ```

4. **Build and Run Docker Containers**
   - In the project directory, run:
     ```powershell
     docker-compose up -d --build
     ```
   - This will build and start the containers in detached mode
   - To view logs (optional):
     ```powershell
     docker-compose logs -f
     ```

5. **Access the Application**
   - API Swagger UI: https://localhost:8081/swagger
   - SQL Server connection details:
     - Server: localhost,1433
     - Username: sa
     - Password: YourStrong!Passw0rd
     - Database: PruebaTecnicaDb

6. **Stop the Application**
   - When finished, stop the containers:
     ```powershell
     docker-compose down
     ```
   - To remove volumes as well:
     ```powershell
     docker-compose down -v
     ```

### macOS Setup

1. **Install Prerequisites**
   - Download and install [Docker Desktop for Mac](https://www.docker.com/products/docker-desktop/)
   - Download and install [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Ensure Docker Desktop is running (check the menu bar icon)

2. **Generate HTTPS Development Certificate**
   - Open Terminal
   - Run the following commands:
     ```bash
     dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
     dotnet dev-certs https --trust
     ```
   - Enter your macOS password when prompted

3. **Clone and Navigate to the Repository**
   - Open Terminal
   - Clone the repository (if not already done):
     ```bash
     git clone <repository-url>
     cd PruebaTecnica
     ```

4. **Build and Run Docker Containers**
   - In the project directory, run:
     ```bash
     docker-compose up -d --build
     ```
   - This will build and start the containers in detached mode
   - To view logs (optional):
     ```bash
     docker-compose logs -f
     ```

5. **Access the Application**
   - API Swagger UI: https://localhost:8081/swagger
   - SQL Server connection details:
     - Server: localhost,1433
     - Username: sa
     - Password: YourStrong!Passw0rd
     - Database: PruebaTecnicaDb

6. **Stop the Application**
   - When finished, stop the containers:
     ```bash
     docker-compose down
     ```
   - To remove volumes as well:
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

If you encounter certificate issues:
- **Windows**: 
  - Ensure you ran the certificate commands as Administrator
  - Verify the certificate path in docker-compose.yml matches your user profile path
  - Try manually trusting the certificate: `dotnet dev-certs https --trust`

- **macOS**: 
  - Check if the certificate was properly added to the keychain
  - You may need to manually approve the certificate in Keychain Access
  - Try recreating the certificate: `dotnet dev-certs https --clean && dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password && dotnet dev-certs https --trust`

### SQL Server Connection Issues

If the API cannot connect to SQL Server, ensure that:
1. SQL Server container is running (`docker ps`)
2. The connection string in the environment variables is correct
3. SQL Server has fully started before the API attempts to connect
4. For Windows, ensure Docker has enough resources allocated (in Docker Desktop settings)
5. For Mac with Apple Silicon (M1/M2/M3), ensure you're using the ARM64 version of SQL Server image or enable Rosetta for x64 emulation

### Database Initialization Issues

If the database is not initialized properly:
1. Check the logs for any errors: `docker-compose logs db`
2. Check the API logs for migration errors: `docker-compose logs api`
3. The application will retry connecting to the database if it's not available initially
4. You can manually run the SQL script by connecting to the SQL Server container:
   ```bash
   docker exec -it <container_id> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -i /docker-entrypoint-initdb.d/init-db.sql
   ```

### Docker Issues

- **Windows**:
  - Ensure Hyper-V or WSL2 is enabled (required for Docker Desktop)
  - Check Windows Defender Firewall is not blocking Docker
  - Restart Docker Desktop if containers fail to start

- **macOS**:
  - Ensure Docker has sufficient resources in preferences
  - For Apple Silicon Macs, check container architecture compatibility
  - Restart Docker Desktop if containers fail to start

## Additional Information

- The SQL Server data is persisted in a Docker volume named `sqlserver-data`
- The API is configured to run in Development mode
- Both HTTP (8080) and HTTPS (8081) endpoints are exposed
- The API container will restart automatically if it fails to connect to the database initially
