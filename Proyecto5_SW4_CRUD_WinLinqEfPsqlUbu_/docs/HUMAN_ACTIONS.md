# HUMAN_ACTIONS.md - Acciones Requeridas por el Operador / Usuario

Este archivo registra las acciones que requieren intervención manual del usuario, tales como la verificación visual del cliente WinForms o la configuración en una VM externa dedicada si no se usa la instancia local de Ubuntu.

---

## 1. Verificación Manual del Cliente Windows Forms (Checklist)

Dado que la interfaz gráfica WinForms no puede automatizarse completamente en modo headless, complete el siguiente checklist ejecutando el cliente desde Visual Studio o mediante `src\Client.WinForms\bin\Debug\CrudWindowsFormsLinq.exe`:

- [ ] **1. Inicio y listado ordenado por Id**:
  - Al abrir la aplicación, el `DataGridView` carga los registros existentes (Ana [20], Luis [25], Maria [22]) ordenados por `Id`.
- [ ] **2. Alta de registro**:
  - Hacer clic en "Nuevo" / "Agregar".
  - Ingresar Nombre: `Carlos`, Edad: `30`.
  - Guardar. Se debe mostrar mensaje de éxito informando el nuevo `Id`.
- [ ] **3. Validación de nombre vacío**:
  - Abrir ventana de creación y dejar el nombre vacío o con espacios.
  - Guardar. Debe mostrarse el mensaje de validación: "El nombre es obligatorio y debe tener entre 1 y 100 caracteres."
- [ ] **4. Validación de edad inválida**:
  - Ingresar nombre válido y edad `150` o negativa.
  - Debe mostrarse el mensaje de validación: "La edad debe ser un número entero entre 0 y 120."
- [ ] **5. Búsqueda insensible a mayúsculas**:
  - Escribir `an` en la caja de texto y presionar "Buscar".
  - Deben aparecer registros como `Ana` y `Maria` (ambos contienen "an").
- [ ] **6. Edición de persona**:
  - Seleccionar un registro, hacer clic en "Editar", modificar la edad o nombre y guardar.
  - El listado principal se refresca con los datos actualizados.
- [ ] **7. Confirmación de eliminación**:
  - Seleccionar un registro y hacer clic en "Eliminar".
  - Debe solicitar confirmación ("¿Está seguro...?"). Al aceptar, el registro se elimina y la tabla se actualiza.
- [ ] **8. Resiliencia si la API está apagada**:
  - Detener el servicio de la API y presionar "Actualizar" o "Buscar".
  - Debe mostrarse un mensaje informativo amigable ("No se pudo conectar con el servidor...") sin crashear la aplicación.

---

## 2. Instrucciones para Despliegue en VM Ubuntu 24.04 Externa (si aplica)

Si se desea desplegar en una máquina virtual externa (Hyper-V, VirtualBox, VMware, cloud):

1. **Copiar el script de aprovisionamiento a la VM**:
   ```bash
   scp deploy/provision-ubuntu.sh usuario@<IP_VM>:~/
   ```
2. **Ejecutar en la VM como root o sudo**:
   ```bash
   ssh usuario@<IP_VM>
   chmod +x ~/provision-ubuntu.sh
   sudo ~/provision-ubuntu.sh
   ```
3. **Publicar y copiar la API binaria compilada**:
   ```powershell
   dotnet publish src/Api/Api.csproj -c Release -r linux-x64 --self-contained false -o ./publish
   scp -r ./publish/* usuario@<IP_VM>:/opt/crudapi/
   ```
4. **Verificar el servicio**:
   ```bash
   sudo systemctl status crudapi
   curl http://localhost:5000/api/people
   ```
5. **Comprobar conectividad desde Windows**:
   ```powershell
   Test-NetConnection <IP_VM> -Port 5000
   curl http://<IP_VM>:5000/api/people
   ```
