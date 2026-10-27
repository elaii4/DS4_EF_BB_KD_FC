# LOOP_STATE.md - Estado de Ejecución Autónoma

## Estado General
- **Rama Git**: `feature/arquitectura-capas`
- **Iteración actual**: 8
- **Límite de seguridad**: 60 iteraciones
- **Estado general**: FINALIZADO CON ÉXITO (8/8 Compuertas Aprobadas)

---

## Matriz de Tareas (Backlog)

| ID | Tarea | Fase | Estado | Dependencias | Intentos | Evidencia |
|---|---|---|---|---|---|---|
| T-01 | Auditoría inicial y archivos de control | Fase 0 | HECHA | Ninguna | 1 | Repositorio Git inicializado en branch `feature/arquitectura-capas`, archivos de control creados. |
| T-02 | Reorganización de carpetas (`src/Client.WinForms`, `src/Api`, `database/postgres`) | Fase 0 | HECHA | T-01 | 1 | Movido a `src/Client.WinForms`, slnx actualizado, compilación MSBuild exitosa. |
| T-03 | Base de datos PostgreSQL: script `database/postgres/01_schema.sql` | Fase 2.1 / 3 | HECHA | T-02 | 1 | Script creado, ejecutado en PostgreSQL 16 (Ubuntu 24.04). Tabla people y registros Ana, Luis, Maria creados con usuario crud_app. |
| T-04 | Proyecto API REST (.NET 8) y EF Core con Npgsql, AppDbContext y Migraciones | Fase 2.2 / 3 | HECHA | T-03 | 1 | Entidad Person, AppDbContext, mapping explicito, migración InitialCreate aplicada exitosamente creando tabla y seeds. |
| T-05 | Implementación de Endpoints REST, LINQ, ILike, DTOs y validaciones | Fase 2.3 / 3 | HECHA | T-04 | 1 | PeopleController con 6 endpoints REST, validaciones DataAnnotations, ProblemDetails, consultas LINQ con EF.Functions.ILike. |
| T-06 | Pruebas de API (smoke tests con matriz de casos) `tests/api-smoke.ps1` | Fase 4.1 | HECHA | T-05 | 1 | Suite `tests/api-smoke` ejecutada contra API real en ejecución y PostgreSQL: 13/13 pruebas pasadas con 0 fallos. |
| T-07 | Cliente WinForms: Implementar `PeopleApiClient`, POCO `Person`, async/await, `ApiBaseUrl` | Fase 2.4 / 3 | HECHA | T-06 | 1 | `PeopleApiClient` con HttpClient único reutilizado, timeout 10s, llamadas asíncronas, URL leída de `App.config`. |
| T-08 | Cliente WinForms: Remover LINQ to SQL, `.dbml`, `System.Data.Linq`, connection strings | Fase 2.4 / 3 | HECHA | T-07 | 1 | Eliminados `Personas.dbml*`, `PeopleLinqDB.cs`, referencias a `System.Data.Linq` y connectionStrings. MSBuild compila con 0 errores. |
| T-09 | Infraestructura Ubuntu 24.04: `deploy/provision-ubuntu.sh`, `crudapi.service`, `ufw-y-red.md` | Fase 2.5 / 5 | HECHA | T-05 | 1 | Creados scripts de aprovisionamiento, unidad systemd y manual de seguridad y firewall. |
| T-10 | Despliegue en Ubuntu 24.04 (WSL/VM) y verificación de servicio systemd | Fase 5 | HECHA | T-09 | 1 | API publicada en `/opt/crudapi`, servicio `crudapi.service` activo en systemd, respondiendo HTTP 200 en puerto 5000. |
| T-11 | Script de compuertas `tests/verify.ps1` (G1 a G8) | Fase 4 | HECHA | T-08, T-10 | 1 | Script `tests/verify.ps1` ejecutado con resultado 8/8 compuertas aprobadas (código de salida 0). |
| T-12 | Cierre y documentación: `README.md`, `REPORTE_POR_CAPAS.md`, `LOOP_DONE.md` | Fase 6 | HECHA | T-11 | 1 | Documentación técnica completa generada con evidencias y checklist para el operador. |

---

## Registro de Iteraciones
- **Iteración 1**: Auditoría, inicialización de Git, rama `feature/arquitectura-capas`, creación de archivos de control.
- **Iteración 2**: Reorganización de estructura de carpetas a `src/Client.WinForms` y creación de `database/postgres`.
- **Iteración 3**: Creación del script `01_schema.sql` e instalación/configuración de PostgreSQL 16 en Ubuntu 24.04 con usuario `crud_app`.
- **Iteración 4**: Creación de proyecto API en .NET 8 con EF Core 8, Npgsql, modelo `Person`, `AppDbContext` y migración `InitialCreate`.
- **Iteración 5**: Implementación de `PeopleController`, DTOs con validaciones, `ProblemDetails`, `EF.Functions.ILike` y suite de pruebas `tests/api-smoke`.
- **Iteración 6**: Despliegue de la API en Ubuntu 24.04 como servicio systemd `crudapi.service` en `/opt/crudapi` y validación de smoke tests (13/13 pasados).
- **Iteración 7**: Refactorización del cliente WinForms con `PeopleApiClient`, POCO `Person`, async/await, eliminación de LINQ to SQL y connection strings.
- **Iteración 8**: Ejecución del verificador integral `tests/verify.ps1`, aprobación de compuertas G1 a G8 con código 0 y generación de documentación de cierre.
