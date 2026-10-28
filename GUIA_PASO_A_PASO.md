# 🚀 Guía Paso a Paso para Ejecutar el Proyecto
> **Destinatarios:** Guía práctica orientada a compañeros de equipo y evaluadores para clonar, configurar y ejecutar la solución en sus computadoras desde cero sin complicaciones.

---

## 📋 ¿Cómo está compuesto el sistema?
El proyecto funciona con una arquitectura cliente-servidor en 4 capas:
1. **Frontend (Cliente):** Aplicación de escritorio en Windows (.NET Framework 4.8 con Windows Forms).
2. **Backend (Servidor):** API REST en ASP.NET Core (.NET 8) y base de datos relacional (PostgreSQL 16) ejecutándose en **Ubuntu 24.04** (usando WSL o una Máquina Virtual).

---

## 🛠️ Requisitos Previos

Antes de empezar, asegúrate de tener instalado en tu computadora:

### En Windows:
* **Visual Studio 2022** (con la carga de trabajo *"Desarrollo de escritorio de .NET"* marcada al instalar).
* **WSL con Ubuntu 24.04** (Recomendado) o una Máquina Virtual (Hyper-V / VirtualBox con Ubuntu 24.04).
  * *Si no tienes WSL instalado, en una terminal PowerShell como Administrador ejecuta:*
    ```powershell
    wsl --install -d Ubuntu-24.04
    ```

---

## 🏁 Paso 1: Configurar el Servidor en Ubuntu 24.04 (Base de Datos + API)

Abre tu terminal de **Ubuntu 24.04** y sigue estos pasos:

### 1.1 Clonar o situarse en la carpeta del proyecto
Si tienes el repositorio en Windows, puedes acceder a él desde Ubuntu mediante `/mnt/c/`:
```bash
cd /mnt/c/Users/TU_USUARIO/Downloads/CrudWindowsFormsLinq/CrudWindowsFormsLinq
```
*(Reemplaza `TU_USUARIO` por el nombre de tu usuario de Windows).*

### 1.2 Ejecutar el aprovisionamiento automatizado
Hemos incluido un script que instala automáticamente PostgreSQL 16, .NET 8 Runtime, crea la base de datos `crud_windows_forms`, el usuario `crud_app` y levanta el servicio de la API:

```bash
sudo chmod +x deploy/provision-ubuntu.sh
sudo ./deploy/provision-ubuntu.sh
```

### 1.3 Publicar e iniciar el servicio de la API
Compila los binarios para Linux e instálalos en el directorio del servicio (`/opt/crudapi`):
```bash
dotnet publish src/Api/Api.csproj -c Release -r linux-x64 --self-contained false -o ./publish
sudo cp -r ./publish/* /opt/crudapi/
sudo chown -R www-data:www-data /opt/crudapi
sudo systemctl restart crudapi
```

### 1.4 Comprobar que el backend está vivo
Prueba que la API responde ejecutando:
```bash
curl http://localhost:5000/api/people
```
✅ **Resultado esperado:** Debe devolverte un JSON con los registros iniciales:
```json
[{"id":1,"name":"Ana","age":20},{"id":2,"name":"Luis","age":25},{"id":3,"name":"Maria","age":22}]
```

---

## 🖥️ Paso 2: Abrir y Ejecutar el Cliente en Windows

Una vez que la API está respondiendo en Ubuntu, pasamos a Windows:

### 2.1 Abrir la solución
1. Dirígete a la carpeta del proyecto en el Explorador de Archivos de Windows.
2. Haz doble clic en el archivo de la solución: **`CrudWindowsFormsLinq.slnx`** para abrirlo en Visual Studio 2022.

### 2.2 Verificar la dirección en `App.config`
1. En el *Explorador de Soluciones*, entra a:
   `src` ➔ `Client.WinForms` ➔ abre el archivo **`App.config`**.
2. Verifica que tenga la URL local:
   ```xml
   <appSettings>
       <add key="ApiBaseUrl" value="http://localhost:5000" />
   </appSettings>
   ```
   *(Si usas WSL en la misma computadora, `http://localhost:5000` funciona directamente).*

### 2.3 ⚠️ IMPORTANTE: Seleccionar el Proyecto de Inicio
Para evitar que Visual Studio intente ejecutar la API en lugar de la ventana:
1. En el panel derecho (*Explorador de soluciones*), busca el proyecto **`CrudWindowsFormsLinq`** (dentro de la carpeta `src/Client.WinForms`).
2. Dale **clic derecho**.
3. Selecciona la opción: **"Establecer como proyecto de inicio"** (se pondrá en negrita).

### 2.4 Ejecutar la Aplicación
1. Presiona la tecla **F5** o haz clic en el botón verde con forma de play: **▶ Iniciar**.
2. ¡Listo! Se abrirá la ventana gráfica con la lista de personas cargada desde PostgreSQL.

---

## 🧪 Paso 3: Probar las Funcionalidades en la Ventana

Puedes verificar que cada operación del CRUD funciona correctamente:
* **Ver lista:** Los registros deben aparecer en la tabla automáticamente al abrir la ventana.
* **Crear nuevo:** Clic en **"+ Nuevo"**, ingresa un nombre y edad (entre 0 y 120), y presiona **Guardar**.
* **Editar:** Haz clic sobre una fila, presiona **"Editar"**, modifica algún dato y guarda.
* **Buscar:** Escribe una letra o nombre en la barra superior (ej: `lu` o `ana`) y haz clic en **"Buscar"**.
* **Eliminar:** Selecciona una persona, haz clic en **"Eliminar"** y confirma el cuadro de diálogo.

---

## 🔍 Paso 4 (Opcional): Cómo ver los datos directamente en PostgreSQL

Si deseas comprobar con tus propios ojos que los datos realmente se guardan en PostgreSQL en Ubuntu:

Abre tu terminal de Ubuntu y ejecuta:
```bash
PGPASSWORD='crud_secret_dev_pass' psql -h 127.0.0.1 -U crud_app -d crud_windows_forms -c "SELECT * FROM people ORDER BY id;"
```
Verás la tabla actualizada con las modificaciones que hayas hecho desde la pantalla de Windows.

---

## ❓ Preguntas Frecuentes y Solución de Problemas

#### 1. La ventana se abre pero dice: *"Error al consultar: No se pudo conectar con el servidor"*
* **Causa:** El servicio de la API en Ubuntu no está encendido o el puerto 5000 no responde.
* **Solución:** En tu terminal de Ubuntu ejecuta:
  ```bash
  sudo systemctl restart crudapi
  sudo systemctl status crudapi
  ```
  Debe mostrar `active (running)`.

#### 2. Al dar F5 se abre una consola negra de comandos en vez de la ventana
* **Causa:** Visual Studio tiene seleccionado el proyecto `Api` como proyecto de inicio predeterminado.
* **Solución:** En el Explorador de Soluciones, dale clic derecho a `CrudWindowsFormsLinq` (el de Windows Forms) y elige **"Establecer como proyecto de inicio"**.

#### 3. ¿Tengo que instalar o abrir pgAdmin?
* **No.** Toda la base de datos se crea, migra y alimenta automáticamente mediante scripts y Entity Framework Core. pgAdmin es completamente opcional.
