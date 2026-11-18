# Proyecto5_SW4_CRUD_Blazor_WinLinqEfPsqlUbu_

Sistema CRUD desacoplado de 4 capas con frontend moderno en **Blazor WebAssembly (.NET 8)**, backend en **ASP.NET Core 8 Web API**, ORM con **Entity Framework Core 8** y persistencia dual en **PostgreSQL 16** (Windows Local y Ubuntu AWS EC2 en la nube).

---

## 🏛️ Arquitectura del Sistema (4 Capas Desacopladas)

```
┌─────────────────────────────────────────────────────────────┐
│  1. CAPA DE PRESENTACIÓN (Frontend Web SPA)                 │
│     Blazor WebAssembly (.NET 8) • C# ejecutado en Browser   │
│     Puerto local: 5292                                      │
└──────────────────────────────┬──────────────────────────────┘
                               │ HTTP REST (JSON) + CORS
┌──────────────────────────────▼──────────────────────────────┐
│  2. CAPA DE SERVICIO / CONTROLADORES (Backend REST)         │
│     ASP.NET Core 8 Web API • Swagger / OpenAPI              │
│     - Windows Server (Local): Puerto 5144                   │
│     - Ubuntu Server (AWS EC2): Docker Compose + Cloudflare  │
└──────────────────────────────┬──────────────────────────────┘
                               │ LINQ queries
┌──────────────────────────────▼──────────────────────────────┐
│  3. CAPA DE ACCESO A DATOS (ORM)                            │
│     Entity Framework Core 8 (Npgsql.EntityFrameworkCore)    │
│     Mapeo relacional objeto-entidad PersonDbContext         │
└──────────────────────────────┬──────────────────────────────┘
                               │ SQL / TCP 5432
┌──────────────────────────────▼──────────────────────────────┐
│  4. CAPA DE BASE DE DATOS (Persistencia PostgreSQL 16)      │
│     - Windows: crud_windows_forms_win (Puerto 5434)         │
│     - Ubuntu:  crud_windows_forms     (Puerto 5432)         │
└─────────────────────────────────────────────────────────────┘
```

---

## ✨ Características Principales

1. **Frontend Moderno en Blazor WebAssembly:**
   - SPA 100% reactiva escrita en C# ejecutada en el navegador vía WebAssembly.
   - Diseño estilo Dribbble / SaaS con tarjetas pastel y tipografía Plus Jakarta Sans.
   - **Pantalla de carga 3D:** Animación isométrica de cubos negros deslizantes (`LoadingBoxes.razor`).
   - **Conmutador de servidor en caliente (Hot Switch):** Alterna entre la base de datos de Windows y la de Ubuntu en tiempo real sin recargar la página.
   - **Doble vista:** Conmutador interactivo entre vista en **Tarjetas** y vista en **Lista / Tabla**.
   - **Búsqueda en tiempo real:** Barra de búsqueda integrada conectada al backend (`GET /api/people?search=...`).
   - **Filtros por edad:** Filtros rápidos en píldora (`Todos`, `Jóvenes`, `Adultos`, `Senior`).
   - **Cápsula modal centrada:** Ventanas emergentes modernas con efecto de desenfoque (`backdrop-filter`) para crear, editar y confirmar eliminación.

2. **Backend ASP.NET Core 8 Web API:**
   - Endpoints RESTful completos (`GET`, `POST`, `PUT`, `DELETE`).
   - Middleware CORS habilitado para soportar clientes WebAssembly y de escritorio.
   - Documentación interactiva con Swagger UI (`/swagger`).

3. **Bases de Datos Duales (PostgreSQL 16):**
   - **Windows:** Contenedor Docker local `crud_postgres` en puerto `5434`.
   - **Ubuntu:** AWS EC2 con Docker Compose y túnel Cloudflare seguro para acceso global.

---

## 🚀 Instrucciones de Ejecución

### 1. Iniciar la Base de Datos y Servidor Windows
Ejecuta el script:
```cmd
iniciar-servidor-windows.bat
```
*Inicia la API en `http://localhost:5144` y Swagger en `http://localhost:5144/swagger`.*

### 2. Iniciar el Cliente Web Blazor WebAssembly
Ejecuta el script:
```cmd
iniciar-cliente-blazor.bat
```
*Compila los cambios y levanta la aplicación en `http://localhost:5292`.*

---

## 📂 Estructura del Proyecto

```
Proyecto5_SW4_CRUD_Blazor_WinLinqEfPsqlUbu_/
├── Database/
│   └── postgres/
│       ├── 01_schema.sql                  # Script DDL/DML Ubuntu
│       └── 02_schema_windows.sql          # Script DDL/DML Windows
├── deploy/
│   └── docker-compose.yml                 # Orquestación Docker (API + Postgres)
├── src/
│   ├── Api/                               # ASP.NET Core 8 Web API
│   │   ├── Controllers/PeopleController.cs
│   │   ├── Data/PersonDbContext.cs
│   │   ├── DTOs/
│   │   ├── Models/Person.cs
│   │   ├── Dockerfile
│   │   └── Program.cs
│   └── Client.Blazor/                     # Frontend Blazor WebAssembly (.NET 8)
│       ├── Layout/
│       │   ├── MainLayout.razor
│       │   └── LoadingBoxes.razor         # Pantalla de carga 3D
│       ├── Models/Person.cs
│       ├── Pages/Home.razor               # Dashboard reactivo
│       ├── Services/PeopleApiService.cs   # Cliente HTTP y Hot Switch
│       └── wwwroot/
│           ├── appsettings.json           # URLs de APIs Windows y Ubuntu
│           └── index.html
├── iniciar-cliente-blazor.bat             # Lanzador del cliente web
├── iniciar-servidor-windows.bat           # Lanzador del servidor Windows
├── Proyecto5_SW4_CRUD_Blazor_WinLinqEfPsqlUbu_.sln
└── README.md
```
