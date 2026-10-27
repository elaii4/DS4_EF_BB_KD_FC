# REPORTE_POR_CAPAS.md - Reporte Técnico Integral de Migración por Capas

## 1. Resumen Ejecutivo
Se transformó exitosamente el proyecto **CrudWindowsFormsLinq** (originalmente una aplicación monolítica acoplada directamente a SQL Server mediante LINQ to SQL) en una moderna **arquitectura desacoplada en cuatro capas**. El cliente corre en Windows Forms (.NET Framework 4.8) y se comunica exclusivamente vía HTTP REST con una Web API en ASP.NET Core (.NET 8 LTS), la cual interactúa a través de Entity Framework Core 8 con una base de datos PostgreSQL 16 alojada en Ubuntu 24.04 LTS.

---

## 2. Reporte Detallado por Capa

### Capa 1: Base de Datos (PostgreSQL 16 en Ubuntu 24.04)
- **Objetivo y responsabilidad**: Almacenamiento persistente, integridad relacional mediante constraints (`CHECK (age BETWEEN 0 AND 120)`), secuencia auto-incremental (`IDENTITY`) y control de acceso por usuario sin privilegios administrativos.
- **Estado inicial**: Base de datos SQL Server (`dbo.People`, script `CrudWindowsForms.sql`) con autenticación integrada o SA.
- **Estado final**: PostgreSQL 16 con base de datos `crud_windows_forms`, tabla `people`, usuario de aplicación `crud_app` con permisos restringidos exclusivamente a su esquema. Escucha únicamente en `127.0.0.1` (localhost) de la VM.
- **Archivos creados/modificados**:
  - `database/postgres/01_schema.sql`: Script SQL de referencia, creación de tabla, constraint y semillas de datos.
  - `database/postgres/init-local-db.sh`: Script idempotente de inicialización de base de datos local.
  - `database/postgres/configure-auth.sh`: Configuración de pg_hba.conf para permitir acceso localhost con scram-sha-256.
- **Decisiones técnicas**:
  - Convención de nombres en minúsculas snake_case (`people`, `id`, `name`, `age`).
  - No exponer el puerto 5432 a la red externa para mitigar ataques directos contra el motor de base de datos.
- **Comandos ejecutados**:
  ```bash
  sudo apt-get install -y postgresql postgresql-contrib
  sudo -u postgres psql -c "CREATE ROLE crud_app LOGIN PASSWORD '***';"
  sudo -u postgres createdb -O crud_app crud_windows_forms
  PGPASSWORD='***' psql -h 127.0.0.1 -U crud_app -d crud_windows_forms -f 01_schema.sql
  ```
- **Pruebas y evidencias**:
  - Consulta: `SELECT id, name, age FROM people ORDER BY id;`
  - Resultado:
    ```
     id | name  | age 
    ----+-------+-----
      1 | Ana   |  20
      2 | Luis  |  25
      3 | Maria |  22
    (3 rows)
    ```

---

### Capa 2: Acceso a Datos (EF Core + Npgsql)
- **Objetivo y responsabilidad**: Mapeo objeto-relacional entre la entidad `Person` y la tabla `people`, ejecución de consultas LINQ traducidas a SQL nativo y versionado Code-First.
- **Estado inicial**: LINQ to SQL (.dbml generado para SQL Server), acoplamiento a `System.Data.Linq`.
- **Estado final**: Entity Framework Core 8 con proveedor `Npgsql.EntityFrameworkCore.PostgreSQL`, contexto `AppDbContext`, mapeo explícito en `OnModelCreating`, lecturas `AsNoTracking()` y búsqueda case-insensitive con `EF.Functions.ILike`.
- **Archivos creados/modificados**:
  - `src/Api/Models/Person.cs`: Entidad POCO.
  - `src/Api/Data/AppDbContext.cs`: DbContext con mapeo de tabla, columnas, constraint y HasData.
  - `src/Api/Migrations/20261027201349_InitialCreate.cs`: Migración Code-First.
  - `src/Api/Migrations/AppDbContextModelSnapshot.cs`: Snapshot del modelo.
- **Decisiones técnicas**:
  - Uso de `EF.Functions.ILike(p.Name, $"%{search}%")` para garantizar que la búsqueda sea insensible a mayúsculas y se ejecute directamente en PostgreSQL (SQL `ILIKE`), replicando el comportamiento original de SQL Server.
- **Comandos ejecutados**:
  ```bash
  dotnet dotnet-ef migrations add InitialCreate --project src/Api --startup-project src/Api
  dotnet dotnet-ef database update --project src/Api --startup-project src/Api
  ```
- **Pruebas y evidencias**:
  - Aplicación de migración en base vacía creó exitosamente la tabla `people`, el constraint `CK_people_age` y sembró a Ana, Luis y Maria (Compuerta G6 aprobada).

---

### Capa 3: API REST (ASP.NET Core Web API .NET 8)
- **Objetivo y responsabilidad**: Exposición de endpoints HTTP RESTful, validación server-side estricta de DTOs, manejo uniforme de errores RFC 7807 (ProblemDetails) y documentación OpenAPI Swagger.
- **Estado inicial**: Inexistente (el cliente se conectaba directo a SQL Server).
- **Estado final**: Controlador `PeopleController` con endpoints normalizados:
  - `GET /api/people`: Listado ordenado por Id.
  - `GET /api/people?search={texto}`: Búsqueda parcial insensible a mayúsculas ordenada por nombre.
  - `GET /api/people/{id}`: Detalle de persona (200 o 404).
  - `POST /api/people`: Creación validada (201 CreatedAtAction con nuevo Id, o 400).
  - `PUT /api/people/{id}`: Actualización completa (204 No Content, 400 o 404).
  - `DELETE /api/people/{id}`: Eliminación (204 No Content o 404).
- **Archivos creados/modificados**:
  - `src/Api/Controllers/PeopleController.cs`
  - `src/Api/DTOs/PersonDtos.cs` (`PersonResponseDto`, `PersonCreateDto`, `PersonUpdateDto`)
  - `src/Api/Program.cs`
  - `src/Api/appsettings.Example.json` (plantilla sin secretos)
- **Decisiones técnicas**:
  - DTOs separados para desacoplar el modelo interno de la interfaz pública.
  - Formato ProblemDetails en respuestas de error para cumplimiento de estándares.
  - Inyección de cadena de conexión mediante variable de entorno `ConnectionStrings__Default`.
- **Pruebas y evidencias**:
  - Ejecución de suite automatizada `tests/api-smoke` (13 casos pasados, 0 fallados).

---

### Capa 4: Cliente Windows Forms (.NET Framework 4.8)
- **Objetivo y responsabilidad**: Interfaz gráfica de usuario con DataGridView, captura de eventos, validación inmediata del formulario y consumo HTTP asíncrono.
- **Estado inicial**: Formularios acoplados a `PeopleLinqDB`, llamadas síncronas bloqueantes, referencias a `System.Data.Linq` y cadenas de conexión a SQL Server en `App.config` y `Settings.settings`.
- **Estado final**: Cliente limpio usando `PeopleApiClient`, llamadas no bloqueantes con `async/await`, POCO `Person` y URL configurable en `App.config` (`ApiBaseUrl`).
- **Archivos creados/modificados/eliminados**:
  - Creados: `src/Client.WinForms/PeopleApiClient.cs`, `src/Client.WinForms/Person.cs`, `packages.config`
  - Modificados: `src/Client.WinForms/Form1.cs`, `src/Client.WinForms/FrmPersona.cs`, `src/Client.WinForms/App.config`, `src/Client.WinForms/CrudWindowsFormsLinq.csproj`
  - Eliminados: `PeopleLinqDB.cs`, `Personas.dbml`, `Personas.designer.cs`, `Personas.dbml.layout`
- **Decisiones técnicas**:
  - Se mantuvo la firma de métodos idéntica (`Get`, `GetById`, `Add`, `Update`, `Delete`, `Search`) con soporte dual sincrónico/asincrónico.
  - Reutilización de una única instancia estática de `HttpClient` con `Timeout = 10s` para prevenir agotamiento de sockets.
  - Preservación íntegra de eventos y diseñadores visuales (`Form1.Designer.cs`, `FrmPersona.Designer.cs`).
- **Pruebas y evidencias**:
  - Compilación limpia con MSBuild Release sin errores ni advertencias (Compuerta G2).
  - Escaneo de código: 0 referencias a LINQ to SQL o SQL Server (Compuerta G3).

---

### Capa 5: Infraestructura y Despliegue (Ubuntu 24.04 LTS)
- **Objetivo y responsabilidad**: Entorno de ejecución productivo, seguro y resiliente para la API y la base de datos.
- **Estado inicial**: Sin soporte Linux; solución diseñada solo para Windows local.
- **Estado final**: Publicación linux-x64 de la API en `/opt/crudapi`, servicio systemd `crudapi.service` ejecutado con usuario sin privilegios `www-data`, reinicio automático ante caídas y reglas de firewall UFW.
- **Archivos creados**:
  - `deploy/crudapi.service`: Unidad systemd.
  - `deploy/provision-ubuntu.sh`: Script idempotente de instalación y configuración.
  - `deploy/ufw-y-red.md`: Manual de firewall y acceso seguro por túnel SSH.
- **Pruebas y evidencias**:
  - Estado del servicio: `systemctl status crudapi` -> `active (running)`.
  - Sobrevivencia a reinicio: El servicio arranca automáticamente tras iniciar el sistema.

---

## 3. Matriz de Pruebas de la API REST

| # | Endpoint / Caso de Prueba | Entrada / Condición | Resultado Esperado | Resultado Obtenido | Estado |
|---|---|---|---|---|---|
| 1 | `GET /api/people` | Sin parámetros | 200 OK, lista con >= 3 registros ordenados por Id | 200 OK, 3 registros ([Ana, Luis, Maria]) | **PASS** |
| 2 | `GET /api/people?search=an` | Parámetro `search=an` | 200 OK, devuelve Ana (case-insensitive) | 200 OK, contiene "Ana" | **PASS** |
| 3 | `GET /api/people?search=a` | Parámetro `search=a` | 200 OK, devuelve Ana y Maria | 200 OK, contiene "Ana" y "Maria" | **PASS** |
| 4 | `GET /api/people/1` | Id existente (1) | 200 OK, persona con Id 1 (Ana) | 200 OK, Ana (id=1, age=20) | **PASS** |
| 5 | `GET /api/people/999999` | Id inexistente | 404 Not Found (ProblemDetails) | 404 Not Found | **PASS** |
| 6 | `POST /api/people` | `{"name":"Carlos","age":28}` | 201 Created con Id generado | 201 Created con Id nuevo | **PASS** |
| 7 | `POST /api/people` | `{"name":"","age":28}` | 400 Bad Request (validación de nombre) | 400 Bad Request | **PASS** |
| 8 | `POST /api/people` | `{"name":"Test","age":121}` | 400 Bad Request (edad fuera de rango 0-120) | 400 Bad Request | **PASS** |
| 9 | `POST /api/people` | `{"name":"Test","age":-1}` | 400 Bad Request (edad negativa) | 400 Bad Request | **PASS** |
| 10 | `PUT /api/people/{id}` | Id válido con datos nuevos | 204 No Content | 204 No Content | **PASS** |
| 11 | `PUT /api/people/999999` | Id inexistente | 404 Not Found | 404 Not Found | **PASS** |
| 12 | `DELETE /api/people/{id}` | Id válido | 204 No Content | 204 No Content | **PASS** |
| 13 | `DELETE /api/people/999999`| Id inexistente | 404 Not Found | 404 Not Found | **PASS** |

---

## 4. Diagrama de Flujo de Petición Completa ("Editar Persona")

```mermaid
sequenceDiagram
    autonumber
    actor U as Usuario (Operador)
    participant WF as FrmPersona (WinForms)
    participant AC as PeopleApiClient (HttpClient)
    participant API as PeopleController (ASP.NET Core)
    participant EF as AppDbContext (EF Core)
    participant PG as PostgreSQL 16 (people)

    U->>WF: Modifica Nombre a "Luis Gomez" y Edad a 26; clic en Guardar
    WF->>WF: Validación local inmediata (Nombre no vacío, Edad entre 0 y 120)
    WF->>AC: await UpdateAsync(2, "Luis Gomez", 26)
    AC->>API: HTTP PUT /api/people/2 {"name":"Luis Gomez","age":26}
    API->>API: Model Validation (DataAnnotations) -> IsValid == true
    API->>EF: FindAsync(2)
    EF->>PG: SELECT id, name, age FROM people WHERE id = 2
    PG-->>EF: Fila existente
    API->>EF: Modifica propiedades de entidad y SaveChangesAsync()
    EF->>PG: UPDATE people SET name = 'Luis Gomez', age = 26 WHERE id = 2
    PG-->>EF: 1 row affected
    EF-->>API: Persistencia confirmada
    API-->>AC: HTTP 204 No Content
    AC-->>WF: true
    WF->>U: Cierra diálogo con DialogResult.OK
    Note over WF,U: Form1 refresca automáticamente el DataGridView
```

---

## 5. Matriz de Riesgos y Recomendaciones Técnicas

| Área | Riesgo Identificado | Mitigación Implementada | Recomendación Futura |
|---|---|---|---|
| **Seguridad de Red** | Exposición de la base de datos a internet | PostgreSQL escucha únicamente en `127.0.0.1`. UFW bloquea puerto 5432. | Mantener acceso administrativo exclusivamente mediante túneles SSH. |
| **Cifrado de Tráfico** | Tráfico HTTP plano en la red local/VM | Separación de capas y configuración en red privada. | Implementar reverse proxy Nginx con certificado TLS/HTTPS (Let's Encrypt o autofirmado). |
| **Autenticación** | Endpoints de la API abiertos sin credenciales | Uso en red interna controlada. | Implementar autenticación basada en JWT o API Key para autorizar clientes WinForms. |
| **Rendimiento** | Consultas masivas si crece la tabla `people` | Uso de `AsNoTracking()` en todas las lecturas. | Incorporar paginación (`page`, `pageSize`) en el endpoint `GET /api/people`. |
| **Disponibilidad** | Caída inesperada del proceso de la API | Configuración systemd con `Restart=always` y `RestartSec=10`. | Incorporar Health Checks de ASP.NET Core (`/health`) y monitoreo automatizado. |

---

## 6. Problemas Encontrados y Soluciones Aplicadas

1. **Incompatibilidad de LINQ to SQL con PostgreSQL**:
   - *Causa*: LINQ to SQL solo funciona con Microsoft SQL Server.
   - *Solución*: Sustitución completa por EF Core 8 con `Npgsql.EntityFrameworkCore.PostgreSQL`.
2. **Búsqueda insensible a mayúsculas en PostgreSQL**:
   - *Causa*: El operador `LIKE` estándar en PostgreSQL es case-sensitive por defecto.
   - *Solución*: Uso de `EF.Functions.ILike(p.Name, $"%{texto}%")`, que compila a `ILIKE` en PostgreSQL sin cargar datos en memoria.
3. **Persistencia de sesión en WSL**:
   - *Causa*: Al cerrar todas las terminales de WSL, la máquina virtual entra en suspensión.
   - *Solución*: Se configuró `crudapi.service` como servicio habilitado en systemd para arranque automático y se documentó el script `deploy/provision-ubuntu.sh` para VMs dedicadas (Hyper-V/VirtualBox).
