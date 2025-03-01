# PruebaTecnica - API de Mantenimiento

Este repositorio contiene una API RESTful para el mantenimiento de Clientes, Productos y Órdenes, desarrollada con .NET 8.0 y SQL Server.

## Estructura del Proyecto

La solución sigue los principios de Clean Architecture y está organizada en las siguientes capas:

- **PruebaTecnica.Domain**: Contiene las entidades del dominio y lógica de negocio.
  - `Entities/`: Clases Cliente, Producto, Orden y DetalleOrden.

- **PruebaTecnica.Application**: Define interfaces y servicios de aplicación.
  - `Interfaces/`: Interfaces de repositorios y servicios.

- **PruebaTecnica.Infrastructure**: Implementa el acceso a datos.
  - `Persistence/`: Contexto de base de datos y configuraciones.
  - `Repositories/`: Implementaciones de repositorios.
  - `DependencyInjection/`: Configuración de inyección de dependencias.

- **PruebaTecnica.API**: Capa de presentación con controladores REST.
  - `Controllers/`: Controladores para Cliente, Producto y Orden.
  - `Middleware/`: Middleware personalizado para manejo de errores.

- **PruebaTecnica.Shared**: Modelos y utilidades compartidas.
  - `Models/`: DTOs y modelos de respuesta.

## Requisitos Previos

- [Docker](https://www.docker.com/products/docker-desktop/) para la base de datos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) para ejecutar la API
- [Git](https://git-scm.com/downloads) para clonar el repositorio

## Instrucciones de Ejecución

### Windows

1. **Iniciar la Base de Datos**
   - Abrir PowerShell o Command Prompt
   - Navegar al directorio del proyecto
   - Ejecutar:
     ```powershell
     docker-compose up -d db
     ```
   - Esto iniciará solo el contenedor de SQL Server

2. **Ejecutar la API Manualmente**
   - En el mismo directorio, navegar a la carpeta de la API:
     ```powershell
     cd PruebaTecnica.API
     ```
   - Ejecutar la API:
     ```powershell
     dotnet run
     ```

3. **Acceder a la API**
   - Swagger UI: http://localhost:5041/swagger
   - La API estará disponible en:
     - HTTP: http://localhost:5041

### macOS

1. **Iniciar la Base de Datos**
   - Abrir Terminal
   - Navegar al directorio del proyecto
   - Ejecutar:
     ```bash
     docker-compose up -d db
     ```
   - Esto iniciará solo el contenedor de SQL Server

2. **Ejecutar la API Manualmente**
   - En el mismo directorio, navegar a la carpeta de la API:
     ```bash
     cd PruebaTecnica.API
     ```
   - Ejecutar la API:
     ```bash
     dotnet run
     ```

3. **Acceder a la API**
   - Swagger UI: http://localhost:5041/swagger
   - La API estará disponible en:
     - HTTP: http://localhost:5041

## Detalles de la Base de Datos

- **Conexión SQL Server**:
  - Server: localhost,1433
  - Username: sa
  - Password: YourStrong!Passw0rd
  - Database: PruebaTecnicaDb

- **Tablas**:
  - Cliente: Información de clientes
  - Producto: Catálogo de productos
  - Orden: Órdenes de compra
  - DetalleOrden: Detalles de cada orden

## Endpoints de la API

### Clientes
- GET /api/clientes/getAll - Obtiene todos los clientes
- GET /api/clientes/getById/{id} - Obtiene un cliente por ID
- POST /api/clientes/create - Crea un nuevo cliente
- PUT /api/clientes/update - Actualiza un cliente existente

### Productos
- GET /api/productos/getAll - Obtiene todos los productos
- GET /api/productos/getById/{id} - Obtiene un producto por ID
- POST /api/productos/create - Crea un nuevo producto
- PUT /api/productos/update - Actualiza un producto existente

### Órdenes
- POST /api/ordenes/create - Crea una nueva orden con sus detalles

## Detener los Servicios

Para detener la base de datos:
```bash
docker-compose down
```

Para eliminar también los volúmenes:
```bash
docker-compose down -v
```
