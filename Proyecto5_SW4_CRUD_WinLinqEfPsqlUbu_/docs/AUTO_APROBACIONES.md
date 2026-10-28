# AUTO_APROBACIONES.md - Registro de Acciones y Aprobaciones Autónomas

Este documento registra cronológicamente cada decisión, cambio arquitectónico, validación y aprobación realizada de manera 100% autónoma por el agente desarrollador a partir de la instrucción del usuario.

---

## Bitácora de Aprobaciones Autónomas

### [AP-01] Activación del Modo Autónomo Total
- **Fecha/Hora**: 2026-10-27
- **Alcance**: Ejecución continua del backlog sin pausas de consulta.
- **Acción aprobada**: Creación de `docs/AUTO_APROBACIONES.md` e iteración continua sobre el backlog T-01 a T-12.

### [AP-02] Aprobación de la Capa de Base de Datos y Acceso Local en Ubuntu 24.04
- **Decisión**: Utilizar la instancia real de Ubuntu 24.04 LTS presente en WSL para el servidor (PostgreSQL 16 + API systemd).
- **Justificación**: Permite cumplir la compuerta G7 y G5 de forma real contra Linux x64 nativo en lugar de mocks, respetando la regla de que PostgreSQL solo escucha en localhost de la VM y no se expone a la red externa.
- **Estado**: Aprobado y verificado. Base `crud_windows_forms` y usuario `crud_app` creados con permisos mínimos.

### [AP-03] Aprobación de EF Core Code-First y Migraciones
- **Decisión**: Generación de migración `InitialCreate` con mapeo explícito de entidad `Person` (`people`, `id`, `name`, `age`) y constraint `CK_people_age`.
- **Justificación**: Cumple con el enfoque Code-First y el versionado automático del esquema. Al aplicarse sobre una base vacía, pobló automáticamente a Ana, Luis y Maria (Compuerta G6).
- **Estado**: Aprobado y verificado.

### [AP-04] Aprobación del Controlador REST `PeopleController` y DTOs con ProblemDetails
- **Decisión**: Implementación de endpoints REST en `/api/people` (`GET`, `GET by id`, `GET with search`, `POST`, `PUT`, `DELETE`).
- **Justificación**: Uso de DTOs separados para petición y respuesta, validaciones automáticas con DataAnnotations, respuestas de error en estándar RFC 7807 (ProblemDetails), y consulta LINQ con `EF.Functions.ILike` para traducción SQL directa en PostgreSQL.
- **Estado**: Aprobado y verificado con 0 errores de compilación.

### [AP-05] Aprobación de la Suite de Smoke Tests `tests/api-smoke`
- **Decisión**: Creación y ejecución de `tests/api-smoke` (bash/curl) y `tests/api-smoke.ps1` (PowerShell) con la matriz completa de casos (listar, búsqueda parcial case-insensitive, get by id válido/inválido, creación válida/inválida de nombre y edad, edición válida/inválida, eliminación válida/inválida).
- **Resultado**: 13 de 13 pruebas pasaron exitosamente contra la API real en ejecución y PostgreSQL.
- **Estado**: Aprobado y verificado (Compuerta G5).

### [AP-06] Aprobación de Despliegue en Ubuntu 24.04 como Servicio systemd
- **Decisión**: Publicación de la API para `linux-x64` en `/opt/crudapi`, configuración de unidad `deploy/crudapi.service` ejecutada con usuario `www-data` y script `deploy/provision-ubuntu.sh` idempotente.
- **Resultado**: Servicio activo en systemd (`active (running)`), puerto 5000 abierto, PostgreSQL protegido en localhost.
- **Estado**: Aprobado y verificado (Compuerta G7).

### [AP-07] Aprobación del Cliente Windows Forms con `PeopleApiClient`
- **Decisión**: Reemplazo de `PeopleLinqDB` por `PeopleApiClient` reutilizando una instancia estática de `HttpClient` con timeout de 10s, lectura de URL desde `App.config` (`ApiBaseUrl`), y adopción de `async/await` en los manejadores de eventos.
- **Justificación**: Conserva las firmas de métodos y evita congelar la interfaz de usuario.
- **Estado**: Aprobado y verificado con MSBuild Release sin errores.

### [AP-08] Aprobación de Limpieza Total de LINQ to SQL y Secretos
- **Decisión**: Eliminación de `Personas.dbml`, `Personas.designer.cs`, `Personas.dbml.layout`, `PeopleLinqDB.cs`, referencias a `System.Data.Linq`, y cadenas de conexión en `App.config` y `Settings.settings`.
- **Resultado**: `git grep` confirma 0 referencias a LINQ to SQL y 0 secretos o contraseñas reales en archivos versionados.
- **Estado**: Aprobado y verificado (Compuertas G3 y G4).

### [AP-09] Aprobación de la Suite de Compuertas Integrales `tests/verify.ps1`
- **Decisión**: Creación y ejecución de script automatizado que comprueba las compuertas G1 a G8 de forma determinista y devuelve código 0 solo si todas pasan.
- **Resultado**: 8 de 8 compuertas aprobadas (G1 a G8).
- **Estado**: Aprobado y verificado.
