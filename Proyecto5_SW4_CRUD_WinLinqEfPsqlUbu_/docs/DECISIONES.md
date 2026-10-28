# DECISIONES.md - Registro de Decisiones Técnicas

Este documento contiene las decisiones técnicas tomadas de forma autónoma durante el proceso de migración, sus alternativas consideradas y la justificación.

---

### D-01: Estructura de carpetas en `src/Client.WinForms` y `src/Api`
- **Elección**: Separar el cliente Windows Forms en `src/Client.WinForms` y la API en `src/Api`.
- **Alternativas**: Mantener todo en la raíz del proyecto.
- **Justificación**: Cumple con el estándar de arquitectura en capas de .NET y aísla dependencias, evitando conflictos entre .NET Framework 4.8 y .NET 8 / EF Core.

---

### D-02: Target Framework de la API REST (.NET 8)
- **Elección**: ASP.NET Core en .NET 8 LTS (`net8.0`).
- **Alternativas**: .NET 6 o .NET Framework con Web API 2.
- **Justificación**: .NET 8 es la versión LTS actual soportada por Ubuntu 24.04 (paquete oficial `aspnetcore-runtime-8.0`), ofreciendo alto rendimiento y compatibilidad con EF Core 8.

---

### D-03: Mantenimiento del cliente WinForms en .NET Framework 4.8 con Newtonsoft.Json
- **Elección**: Conservar `net48` en `src/Client.WinForms`, incorporando `Newtonsoft.Json` para serialización JSON.
- **Alternativas**: Migrar el cliente a `net8.0-windows`.
- **Justificación**: La especificación solicita explícitamente "cambiar lo mínimo posible en los formularios" y "mantener el cliente en .NET Framework 4.8 y añadir Newtonsoft.Json por NuGet", documentando la migración a .NET 8 como mejora futura.

---

### D-04: Estrategia de búsqueda insensible a mayúsculas con `EF.Functions.ILike`
- **Elección**: Usar `EF.Functions.ILike(p.Name, $"%{search}%")` en la consulta LINQ de EF Core.
- **Alternativas**: `p.Name.ToLower().Contains(search.ToLower())` o collation no determinista en PostgreSQL.
- **Justificación**: En PostgreSQL, `LIKE` distingue mayúsculas por defecto. `EF.Functions.ILike` se traduce de forma nativa a la instrucción SQL `ILIKE` de PostgreSQL en el servidor, replicando fielmente el comportamiento de SQL Server sin cargar datos a memoria.

---

### D-05: HttpClient único y cliente tipado `PeopleApiClient`
- **Elección**: Un único `HttpClient` estático reutilizado con `Timeout = TimeSpan.FromSeconds(10)`, consumiendo la URL base desde `App.config` (`ApiBaseUrl`).
- **Alternativas**: Instanciar `new HttpClient()` en cada llamada o inyección IHttpClientFactory.
- **Justificación**: En WinForms tradicional .NET Framework 4.8, un `HttpClient` estático evita el agotamiento de sockets (socket exhaustion) y el timeout de 10s previene congelamientos prolongados de UI si el servidor no responde.

---

### D-06: Gestión de credenciales y variables de entorno
- **Elección**: La API lee la cadena de conexión desde `ConnectionStrings__Default` o `appsettings.json`. El repositorio solo contiene `appsettings.Example.json` con marcadores sin secretos.
- **Alternativas**: Hardcodear credenciales en el código fuente o en `App.config`.
- **Justificación**: Principio de menor privilegio y seguridad (regla crítica: sin secretos en el repositorio).
