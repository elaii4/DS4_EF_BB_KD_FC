# AUTO_APROBACIONES.md - Registro de Acciones y Aprobaciones Autónomas

Este documento registra cronológicamente cada decisión, cambio arquitectónico, validación y aprobación realizada de manera 100% autónoma por el agente desarrollador a partir de la instrucción del usuario.

---

## Bitácora de Aprobaciones Autónomas

### [AP-01] Activación del Modo Autónomo Total
- **Fecha/Hora**: 2026-10-27
- **Alcance**: Ejecución continua del backlog sin pausas de consulta.
- **Acción aprobada**: Crear `docs/AUTO_APROBACIONES.md` e iterar de forma ininterrumpida sobre T-04 hasta T-12.

### [AP-02] Aprobación de la Capa de Base de Datos y Acceso Local en Ubuntu 24.04
- **Decisión**: Utilizar la instancia real de Ubuntu 24.04 LTS presente en WSL para el servidor (PostgreSQL 16 + API systemd).
- **Justificación**: Permite cumplir la compuerta G7 y G5 de forma real contra Linux x64 nativo en lugar de mocks, respetando la regla de que PostgreSQL solo escucha en localhost de la VM y no se expone a la red externa.
- **Estado**: Aprobado y verificado. Base `crud_windows_forms` y usuario `crud_app` creados con permisos mínimos.

### [AP-03] Aprobación de EF Core Code-First y Migraciones
- **Decisión**: Generación de migración `InitialCreate` con mapeo explícito de entidad `Person` (`people`, `id`, `name`, `age`) y constraint `CK_people_age`.
- **Justificación**: Cumple con el enfoque Code-First y el versionado automático del esquema. Al aplicarse sobre una base vacía, pobló automáticamente a Ana, Luis y Maria (Compuerta G6).
- **Estado**: Aprobado y verificado.

