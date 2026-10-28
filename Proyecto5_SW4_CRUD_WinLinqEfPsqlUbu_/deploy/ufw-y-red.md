# Configuración de Red, Firewall (UFW) y Seguridad en Ubuntu 24.04

## 1. Topología de Red y Principio de Menor Exposición
En esta arquitectura desacoplada:
- **Cliente Windows**: Se comunica exclusivamente vía HTTP con la API REST en el puerto `5000`.
- **Servidor Ubuntu 24.04**:
  - Aloja la API REST en `http://0.0.0.0:5000`.
  - Aloja PostgreSQL 16 escuchando **únicamente** en `localhost` (`127.0.0.1:5432`).
  - **PostgreSQL nunca se expone a la red externa**.

---

## 2. Configuración de UFW (Uncomplicated Firewall)
Para garantizar que solo la API y la administración remota (SSH) sean accesibles desde el exterior:

```bash
# 1. Restablecer políticas por defecto
sudo ufw default deny incoming
sudo ufw default allow outgoing

# 2. Permitir SSH (puerto 22)
sudo ufw allow 22/tcp comment 'SSH Remote Admin'

# 3. Permitir el puerto de la API REST (5000)
sudo ufw allow 5000/tcp comment 'CrudWindowsForms API REST'

# 4. Habilitar el firewall
sudo ufw --force enable

# 5. Verificar estado de reglas
sudo ufw status verbose
```

Salida esperada de `sudo ufw status`:
```
Status: active
Logging: on (low)
Default: deny (incoming), allow (outgoing), disabled (routed)
New profiles: skip

To                         Action      From
--                         ------      ----
22/tcp (SSH Remote Admin)  ALLOW IN    Anywhere
5000/tcp (CrudWindowsForms API REST) ALLOW IN Anywhere
22/tcp (v6)                ALLOW IN    Anywhere (v6)
5000/tcp (v6)              ALLOW IN    Anywhere (v6)
```

---

## 3. Acceso Administrativo Seguro a PostgreSQL (Túnel SSH)
Dado que el puerto 5432 está cerrado al exterior, para conectar herramientas de administración como **DBeaver** o **pgAdmin**:

### Opción A: Desde DBeaver
1. En la configuración de conexión PostgreSQL:
   - **Host**: `localhost`
   - **Database**: `crud_windows_forms`
   - **Username**: `crud_app`
2. Pestaña **SSH**:
   - Marcar: *Use SSH Tunnel*
   - **Host/IP**: `<IP_DE_LA_VM>`
   - **Port**: `22`
   - **User**: Tu usuario de Ubuntu
   - **Authentication**: Password o Clave privada SSH.

### Opción B: Túnel SSH por consola
```bash
ssh -L 5433:localhost:5432 usuario@<IP_DE_LA_VM>
```
Luego conectar en Windows a `localhost:5433`.
