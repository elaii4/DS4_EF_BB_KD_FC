# LOOP_STATE.md - Estado de Ejecución Autónoma

## Estado General
- **Rama Git**: `feature/arquitectura-capas`
- **Iteración actual**: 1
- **Límite de seguridad**: 60 iteraciones
- **Estado general**: EN CURSO

---

## Matriz de Tareas (Backlog)

| ID | Tarea | Fase | Estado | Dependencias | Intentos | Evidencia |
|---|---|---|---|---|---|---|
| T-01 | Auditoría inicial y archivos de control | Fase 0 | HECHA | Ninguna | 1 | Archivos creados, git inicializado en branch `feature/arquitectura-capas`. |
| T-02 | Reorganización de carpetas (`src/Client.WinForms`, `src/Api`, `database/postgres`) | Fase 0 | PENDIENTE | T-01 | 0 | - |
| T-03 | Base de datos PostgreSQL: script `database/postgres/01_schema.sql` | Fase 2.1 / 3 | PENDIENTE | T-02 | 0 | - |
| T-04 | Proyecto API REST (.NET 8) y EF Core con Npgsql, AppDbContext y Migraciones | Fase 2.2 / 3 | PENDIENTE | T-03 | 0 | - |
| T-05 | Implementación de Endpoints REST, LINQ, ILike, DTOs y validaciones | Fase 2.3 / 3 | PENDIENTE | T-04 | 0 | - |
| T-06 | Pruebas de API (smoke tests con matriz de casos) `tests/api-smoke.ps1` | Fase 4.1 | PENDIENTE | T-05 | 0 | - |
| T-07 | Cliente WinForms: Implementar `PeopleApiClient`, POCO `Person`, async/await, `ApiBaseUrl` | Fase 2.4 / 3 | PENDIENTE | T-06 | 0 | - |
| T-08 | Cliente WinForms: Remover LINQ to SQL, `.dbml`, `System.Data.Linq`, connection strings | Fase 2.4 / 3 | PENDIENTE | T-07 | 0 | - |
| T-09 | Infraestructura Ubuntu 24.04: `deploy/provision-ubuntu.sh`, `crudapi.service`, `ufw-y-red.md` | Fase 2.5 / 5 | PENDIENTE | T-05 | 0 | - |
| T-10 | Despliegue en Ubuntu 24.04 (WSL/VM) y verificación de servicio systemd | Fase 5 | PENDIENTE | T-09 | 0 | - |
| T-11 | Script de compuertas `tests/verify.ps1` (G1 a G8) | Fase 4 | PENDIENTE | T-08, T-10 | 0 | - |
| T-12 | Cierre y documentación: `README.md`, `REPORTE_POR_CAPAS.md`, `LOOP_DONE.md` | Fase 6 | PENDIENTE | T-11 | 0 | - |

---

## Registro de Iteraciones
- **Iteración 1**: Inicialización de repositorio Git, rama `feature/arquitectura-capas`, detección de herramientas (.NET SDK 10, MSBuild VS2022, Ubuntu 24.04 WSL disponible). Creación de archivos de control del loop.
