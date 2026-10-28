# Migración de CrudWindowsFormsLinq a Arquitectura en Capas

> 📖 **¿Buscas cómo ejecutar el proyecto paso a paso?**  
> Consulta la [**Guía Paso a Paso para Ejecutar el Proyecto (GUIA_PASO_A_PASO.md)**](file:///c:/Users/VICTUS15FB/Downloads/CrudWindowsFormsLinq/CrudWindowsFormsLinq/GUIA_PASO_A_PASO.md) para un tutorial detallado y amigable desde cero.

Este repositorio contiene la transformación y migración completa de la solución original **CrudWindowsFormsLinq** (CRUD monolítico acoplado a SQL Server mediante LINQ to SQL) hacia una moderna **arquitectura desacoplada en cuatro capas**.

---

## 1. Arquitectura del Sistema

```
+---------------------------------------------+
|        Cliente Windows Forms (C#)           |
|            .NET Framework 4.8               |
|      (HttpClient asíncrono + JSON)          |
+----------------------+----------------------+
                       | HTTP REST (/api/people)
                       v
+---------------------------------------------+
|        ASP.NET Core Web API (.NET 8)        |
|    Controlador REST + DTOs + ProblemDetails |
+----------------------+----------------------+
                       | LINQ (EF.Functions.ILike)
                       v
+---------------------------------------------+
|        Entity Framework Core 8              |
|        Proveedor Npgsql PostgreSQL          |
+----------------------+----------------------+
                       | TCP Localhost (5432)
                       v
+---------------------------------------------+
|        PostgreSQL 16 en Ubuntu 24.04        |
|     Base: crud_windows_forms | Usuario: crud_app |
+---------------------------------------------+
```

### Reglas de Diseño Implementadas:
- **Aislamiento de red**: PostgreSQL escucha únicamente en `127.0.0.1` dentro del servidor Ubuntu; nunca se expone a redes externas.
- **Cliente desacoplado**: El cliente WinForms solo habla HTTP REST con la API y desconoce por completo la base de datos subyacente. Se eliminaron todas las referencias a `System.Data.Linq` y LINQ to SQL.
- **Validación bicapa**: Los formularios validan formato y rangos inmediatamente; la API replica y refuerza todas las reglas mediante DataAnnotations y ProblemDetails RFC 7807.
- **Búsqueda insensible a mayúsculas**: Traducida nativamente a la instrucción `ILIKE` de PostgreSQL con `EF.Functions.ILike`.
- **Cero secretos en el repositorio**: Cadenas de conexión reales y contraseñas inyectadas por variables de entorno del sistema (`ConnectionStrings__Default`). En el repositorio solo existe `appsettings.Example.json`.

---

## 2. Estructura del Proyecto

```
CrudWindowsFormsLinq/
├─ src/
│  ├─ Client.WinForms/        # Cliente Windows Forms (.NET Framework 4.8)
│  │  ├─ PeopleApiClient.cs   # Cliente HTTP asíncrono reutilizando HttpClient
│  │  ├─ Person.cs            # POCO desacoplado
│  │  ├─ Form1.cs             # Listado, búsqueda y acciones principales
│  │  ├─ FrmPersona.cs        # Formulario de alta y edición
│  │  └─ App.config           # Clave ApiBaseUrl (sin connectionStrings)
│  └─ Api/                    # ASP.NET Core Web API (.NET 8 LTS)
│     ├─ Controllers/         # PeopleController con endpoints REST normalizados
│     ├─ Data/                # AppDbContext con mapeo explícito para PostgreSQL
│     ├─ DTOs/                # DTOs de petición y respuesta con validaciones
│     ├─ Migrations/          # Migraciones EF Core Code-First (InitialCreate)
│     └─ Program.cs           # Configuración de pipeline, Swagger y DbContext
├─ database/
│  └─ postgres/
│     ├─ 01_schema.sql        # Script SQL de referencia con esquema y seeds
│     ├─ init-local-db.sh     # Inicialización local automatizada
│     └─ configure-auth.sh    # Configuración de autenticación pg_hba.conf
├─ deploy/
│  ├─ crudapi.service         # Unidad systemd para ejecución daemon en Linux
│  ├─ provision-ubuntu.sh     # Script idempotente de aprovisionamiento en Ubuntu 24.04
│  └─ ufw-y-red.md            # Guía detallada de firewall UFW y túnel SSH
├─ docs/
│  ├─ REPORTE_POR_CAPAS.md    # Reporte técnico detallado con evidencias por capa
│  ├─ LOOP_STATE.md           # Estado de tareas y compuertas del loop
│  ├─ HUMAN_ACTIONS.md        # Acciones para el operador y checklist manual de UI
│  ├─ DECISIONES.md           # Registro de decisiones técnicas arquitectónicas
│  ├─ AUTO_APROBACIONES.md    # Bitácora de aprobaciones autónomas
│  └─ LOOP_DONE.md            # Resumen final de cierre y compuertas
├─ tests/
│  ├─ verify.ps1              # Verificador integral de compuertas G1 a G8
│  ├─ api-smoke.ps1           # Smoke tests de endpoints desde PowerShell
│  └─ api-smoke               # Smoke tests de endpoints desde Bash/curl
└─ README.md
```

---

## 3. Requisitos Previos

- **Entorno Windows (Cliente)**:
  - Windows 10/11
  - .NET Framework 4.8 Runtime / Developer Pack
  - Visual Studio 2022 o MSBuild Build Tools
- **Entorno Linux / VM (Servidor)**:
  - Ubuntu 24.04 LTS (nativo, VM en Hyper-V/VirtualBox o WSL)
  - PostgreSQL 16
  - ASP.NET Core Runtime 8 (`aspnetcore-runtime-8.0`)

---

## 4. Puesta en Marcha

### 4.1 Servidor Ubuntu 24.04 (API + PostgreSQL)
1. Ejecutar el aprovisionamiento automatizado:
   ```bash
   sudo chmod +x deploy/provision-ubuntu.sh
   sudo ./deploy/provision-ubuntu.sh
   ```
2. Publicar e instalar la API en `/opt/crudapi`:
   ```bash
   dotnet publish src/Api/Api.csproj -c Release -r linux-x64 --self-contained false -o ./publish
   sudo cp -r ./publish/* /opt/crudapi/
   sudo chown -R www-data:www-data /opt/crudapi
   sudo systemctl restart crudapi
   sudo systemctl status crudapi
   ```
3. Probar respuesta local del servicio:
   ```bash
   curl http://localhost:5000/api/people
   ```

### 4.2 Cliente Windows Forms
1. Configurar la URL de la API en `src/Client.WinForms/App.config`:
   ```xml
   <appSettings>
       <add key="ApiBaseUrl" value="http://localhost:5000" />
   </appSettings>
   ```
   *(Reemplazar `localhost` por la IP de la VM si corre en una máquina virtual externa).*
2. Compilar y ejecutar desde Visual Studio o consola:
   ```powershell
   & "MSBuild.exe" src/Client.WinForms/CrudWindowsFormsLinq.csproj -v:m
   & "src\Client.WinForms\bin\Debug\CrudWindowsFormsLinq.exe"
   ```

---

## 5. Ejecución de Pruebas Automatizadas

Para validar las 8 compuertas de aceptación de forma integral:
```powershell
powershell -ExecutionPolicy Bypass -File tests/verify.ps1
```

Para ejecutar los smoke tests de la API REST contra una base de datos real:
```powershell
powershell -ExecutionPolicy Bypass -File tests/api-smoke.ps1 -BaseUrl "http://localhost:5000"
```
O en entorno Linux:
```bash
./tests/api-smoke "http://localhost:5000"
```
