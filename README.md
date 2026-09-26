# Sistema de Catálogo de Biblioteca - Seguimiento 1

Proyecto desarrollado para el **Seguimiento 1** aplicando **Clean Architecture (CA)**, principios de **Domain-Driven Design (DDD)**, arquitectura **CQRS (Command Query Responsibility Segregation)** y persistencia en **SQL Server** mediante **Entity Framework Core (EF Core 10)**.

La estructura de la solución, convenciones de nombres, separación por capas y utilidades desacopladas (`SimpleMediator`, `Pagination`, `DataBaseSeeder`, `ef.cmd`) replican fielmente la arquitectura del proyecto de referencia **Inmoby**.

---

## 🏛️ Arquitectura del Sistema

La solución está implementada en **.NET 10.0** y estructurada mediante el archivo de solución moderno `Books.slnx` organizado en carpetas virtuales:

```text
Entrega1-Libreria/
├── docker-compose.yml                      # Contenedor SQL Server 2022
├── .gitignore
├── README.md
└── Microservices/
    └── Books/
        ├── Books.slnx                      # Solución con carpetas de solución
        ├── ef.cmd                          # Automatización de migraciones EF Core
        │
        ├── Books.Domain/                   # [/Core/] Núcleo de Dominio (DDD)
        │   ├── Common/ValueObjects/        # Value Object Isbn (validación de formato)
        │   ├── Entities/
        │   │   ├── Authors/                # Entidad Author
        │   │   ├── Categories/             # Entidad Category
        │   │   └── Books/                  # Agregado Book
        │   └── Exceptions/                 # BussinesRuleException
        │
        ├── Books.Application/              # [/Core/] Casos de Uso (CQRS)
        │   ├── Contracts/                  # IUnitOfWork, IRepository<T>, IBooksRepository
        │   ├── Utilities/                  # SimpleMediator desacoplado y Pagination
        │   ├── Exceptions/                 # MediatorException
        │   ├── UseCases/Books/Queries/
        │   │   ├── GetBooksList/           # Query 1: Listado general de libros
        │   │   ├── GetBookById/            # Query 2: Detalle de libro por ID
        │   │   └── GetBooksByCategory/     # Query 3: Libros por categoría
        │   └── ApplicationServicesRegistry.cs
        │
        ├── Books.Persistence/              # [/Infrastructure/] Persistencia (EF Core)
        │   ├── Configurations/             # Fluent API (BookConfig, AuthorConfig, CategoryConfig)
        │   ├── Extensions/                 # Paginación asíncrona para IQueryable
        │   ├── Repositories/               # Repository<T> genérico y BooksRepository
        │   ├── UnitOfWorks/                # EfCoreUnitOfWork
        │   ├── Seeds/                      # IDataSeeder y DataBaseSeeder (catálogo inicial)
        │   ├── Migrations/                 # Migraciones generadas con ef.cmd
        │   ├── DataContext.cs              # DbContext de EF Core
        │   └── PersistenceServicesRegistry.cs
        │
        ├── Books.Api/                      # [/Presenters/] Capa de Presentación (REST API)
        │   ├── Controllers/                # BooksController (exposición de los 3 casos de uso)
        │   ├── Properties/launchSettings.json
        │   ├── appsettings.json            # Cadena de conexión a SQL Server
        │   ├── Books.Api.http              # Archivo de pruebas HTTP listo para ejecutar
        │   └── Program.cs                  # Pipeline HTTP, OpenAPI, Swagger y Scalar
        │
        └── Books.Tests/                    # [/Tests/] Pruebas Unitarias
            ├── DomainTests.cs              # Pruebas de reglas de negocio e invariantes
            ├── ApplicationTests.cs         # Pruebas de CQRS, Mediador y Casos de Uso
            ├── PersistenceTests.cs         # Pruebas de DataContext, Repositorios y Seeders
            └── ControllerTests.cs          # Pruebas de BooksController
```

---

## 📋 Casos de Uso Implementados (Seguimiento 1)

El sistema implementa tres casos de uso de solo lectura bajo el patrón CQRS:

### 1. **Query 1 – Consultar todos los libros**
- **Endpoint**: `GET /api/books`
- **Descripción**: Obtiene el catálogo de libros registrados con paginación y búsqueda opcional.
- **Campos retornados**: `Id`, `Título`, `ISBN`, `Año de publicación`, `Autor` y `Categoría`.
- **Filtros opcionales**: `pageNumber`, `pageSize`, `categoryId`, `searchTerm`.

### 2. **Query 2 – Consultar un libro por ID**
- **Endpoint**: `GET /api/books/{id}`
- **Descripción**: Consulta la información detallada de un libro a partir de su identificador GUID.
- **Campos retornados**: Información relevante del libro, sinopsis, datos completos del autor (nombre, biografía) y de la categoría (nombre, descripción).
- **Manejo de errores**: Retorna `404 Not Found` con mensaje descriptivo si el libro no existe.

### 3. **Query 3 – Consultar libros por categoría**
- **Endpoint**: `GET /api/books/category/{categoryId}`
- **Descripción**: Consulta todos los libros pertenecientes a una categoría específica con soporte de paginación.

---

## 🚀 Guía de Puesta en Marcha

### Prerrequisitos
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o una instancia local de SQL Server)

### Paso 1: Levantar el contenedor de SQL Server
En la raíz del proyecto (`Entrega1-Libreria/`), ejecuta:
```bash
docker compose up -d
```
Esto iniciará un contenedor SQL Server 2022 en el puerto `1433` con credenciales:
- **Usuario**: `sa`
- **Contraseña**: `Library@2026!`
- **Base de Datos**: `LibraryDb`

### Paso 2: Aplicar migraciones con `ef.cmd`
Navega a la carpeta del microservicio y ejecuta el script de migraciones:
```cmd
cd Microservices\Books
ef.cmd update
```

### Paso 3: Iniciar la API
Ejecuta la API desde la carpeta del microservicio:
```bash
dotnet run --project Books.Api\Books.Api.csproj
```
Al arrancar, el sistema ejecutará automáticamente el semillero (`DataBaseSeeder`) que precarga:
- **5 Autores**: Gabriel García Márquez, George Orwell, Jane Austen, Isaac Asimov y Miguel de Cervantes.
- **5 Categorías**: Novela, Ciencia Ficción, Clásicos, Realismo Mágico y Ensayo.
- **10 Libros**: Obras reconocidas con ISBNs válidos, años de publicación y sinopsis.

---

## 🎨 Interfaces Visuales de Documentación

Una vez iniciada la API en `http://localhost:5000`:

1. **Scalar API Reference (Moderna y Elegante)**:
   - **URL**: [http://localhost:5000/](http://localhost:5000/) (o `/scalar/v1`)
   - Incluye tema *DeepSpace*, generador de snippets en cURL/C#/JS/Python, modo oscuro y consola interactiva de pruebas.

2. **Swagger UI (Clásica)**:
   - **URL**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
   - Especificación OpenAPI JSON: [http://localhost:5000/swagger/v1/swagger.json](http://localhost:5000/swagger/v1/swagger.json)

---

## 🧪 Pruebas Automatizadas

La solución cuenta con **21 pruebas unitarias** que cubren el Dominio, la Capa de Aplicación, la Persistencia y los Controladores:

Para ejecutar todas las pruebas:
```bash
dotnet test Microservices\Books\Books.slnx
```
**Resultado**: `Correctas! - Con error: 0, Superado: 21, Omitido: 0, Total: 21`

---

## 🌿 Flujo de Ramas Git (Git Flow)

El desarrollo del proyecto se gestionó mediante fases y ramas locales:
- `main`: Rama principal de entrega final etiquetada con `v1.0.0-entrega`.
- `develop`: Rama base de integración continua.
- `feature/fase-1-setup`: Inicialización de proyectos y estructura espejo de Inmoby.
- `feature/fase-2-dominio`: Entidades DDD, Value Object `Isbn`, reglas de negocio y excepciones.
- `feature/fase-3-aplicacion`: `SimpleMediator`, `Pagination`, DTOs y casos de uso CQRS.
- `feature/fase-4-persistencia`: `DataContext`, configuraciones Fluent API, repositorios y seeders.
- `feature/fase-5-api`: `BooksController`, OpenAPI, Swagger UI y Scalar API Reference.
