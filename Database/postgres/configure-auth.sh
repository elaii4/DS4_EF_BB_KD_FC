#!/usr/bin/env bash
set -e

DB_PASS="${DB_PASS:-${PGPASSWORD:-crud_secret_dev_pass}}"

echo "Setting password for crud_app..."
sudo -u postgres psql -c "ALTER ROLE crud_app WITH LOGIN PASSWORD '$DB_PASS';"

echo "Ensuring md5/scram-sha-256 in pg_hba.conf..."
HBA_CONF="/etc/postgresql/16/main/pg_hba.conf"
if ! grep -q "crud_app" "$HBA_CONF"; then
    echo "host    crud_windows_forms    crud_app    127.0.0.1/32    scram-sha-256" >> "$HBA_CONF"
    echo "host    crud_windows_forms    crud_app    ::1/128         scram-sha-256" >> "$HBA_CONF"
    echo "host    crud_windows_forms    crud_app    all             scram-sha-256" >> "$HBA_CONF"
fi

service postgresql reload

echo "Testing connection with psql..."
export PGPASSWORD="$DB_PASS"
psql -h 127.0.0.1 -U crud_app -d crud_windows_forms -c "SELECT COUNT(*) FROM people;"

echo "PostgreSQL auth configured successfully."
