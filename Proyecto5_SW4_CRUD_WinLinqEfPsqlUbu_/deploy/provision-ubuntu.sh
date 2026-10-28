#!/usr/bin/env bash
# ==============================================================================
# deploy/provision-ubuntu.sh
# Script idempotente de aprovisionamiento para Ubuntu 24.04 LTS
# Instala PostgreSQL 16, ASP.NET Core Runtime 8, configura base de datos,
# servicio systemd y reglas de firewall UFW.
# ==============================================================================
set -euo pipefail

DB_NAME="crud_windows_forms"
DB_USER="crud_app"
DB_PASS="${DB_PASS:-crud_secret_dev_pass}"
API_DIR="/opt/crudapi"

echo "=== 1. Actualizando paquetes del sistema e instalando dependencias ==="
export DEBIAN_FRONTEND=noninteractive
apt-get update
apt-get install -y --no-install-recommends \
    postgresql postgresql-contrib \
    aspnetcore-runtime-8.0 \
    ufw curl ca-certificates

echo "=== 2. Configurando servicio PostgreSQL ==="
systemctl enable postgresql
systemctl start postgresql

echo "=== 3. Creando usuario y base de datos PostgreSQL ==="
sudo -u postgres psql -v ON_ERROR_STOP=1 <<-EOSQL
    DO \$\$
    BEGIN
        IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = '$DB_USER') THEN
            CREATE ROLE $DB_USER WITH LOGIN PASSWORD '$DB_PASS';
        ELSE
            ALTER ROLE $DB_USER WITH PASSWORD '$DB_PASS';
        END IF;
    END
    \$\$;
EOSQL

if ! sudo -u postgres psql -tc "SELECT 1 FROM pg_database WHERE datname = '$DB_NAME'" | grep -q 1; then
    sudo -u postgres createdb -O "$DB_USER" "$DB_NAME"
    echo "Base de datos $DB_NAME creada."
fi

sudo -u postgres psql -d "$DB_NAME" -c "GRANT ALL PRIVILEGES ON DATABASE $DB_NAME TO $DB_USER;"
sudo -u postgres psql -d "$DB_NAME" -c "GRANT ALL ON SCHEMA public TO $DB_USER;"

echo "=== 4. Configurando pg_hba.conf para escuchar solo en localhost ==="
HBA_CONF="/etc/postgresql/16/main/pg_hba.conf"
if [ -f "$HBA_CONF" ]; then
    if ! grep -q "$DB_USER" "$HBA_CONF"; then
        echo "host    $DB_NAME    $DB_USER    127.0.0.1/32    scram-sha-256" >> "$HBA_CONF"
        echo "host    $DB_NAME    $DB_USER    ::1/128         scram-sha-256" >> "$HBA_CONF"
    fi
fi
systemctl reload postgresql

echo "=== 5. Preparando directorio y permisos para la API ==="
mkdir -p "$API_DIR"
chown -R www-data:www-data "$API_DIR"

echo "=== 6. Configurando variables de entorno y servicio systemd ==="
cat << EOF > /etc/default/crudapi
ConnectionStrings__Default=Host=localhost;Port=5432;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASS
EOF
chmod 600 /etc/default/crudapi
chown www-data:www-data /etc/default/crudapi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SERVICE_SRC="${SCRIPT_DIR}/crudapi.service"
if [ -f "$SERVICE_SRC" ]; then
    cp "$SERVICE_SRC" /etc/systemd/system/crudapi.service
fi

systemctl daemon-reload
systemctl enable crudapi.service

echo "=== 7. Configurando Firewall UFW ==="
if command -v ufw >/dev/null 2>&1; then
    ufw default deny incoming || true
    ufw default allow outgoing || true
    ufw allow 22/tcp comment 'SSH' || true
    ufw allow 5000/tcp comment 'Crud API' || true
fi

echo "=== Aprovisionamiento completado con exito ==="
