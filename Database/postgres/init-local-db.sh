#!/usr/bin/env bash
set -e

echo "Starting PostgreSQL service..."
service postgresql start || true

echo "Configuring user and database..."
sudo -u postgres psql -c "DO \$\$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'crud_app') THEN
        CREATE ROLE crud_app WITH LOGIN PASSWORD 'crud_secret_dev_pass';
    ELSE
        ALTER ROLE crud_app WITH PASSWORD 'crud_secret_dev_pass';
    END IF;
END \$\$;"

sudo -u postgres psql -tc "SELECT 1 FROM pg_database WHERE datname = 'crud_windows_forms'" | grep -q 1 || \
    sudo -u postgres psql -c "CREATE DATABASE crud_windows_forms OWNER crud_app;"

sudo -u postgres psql -d crud_windows_forms -c "GRANT ALL PRIVILEGES ON DATABASE crud_windows_forms TO crud_app;"
sudo -u postgres psql -d crud_windows_forms -c "GRANT ALL ON SCHEMA public TO crud_app;"

echo "Applying schema and seeds from 01_schema.sql..."
SCHEMA_FILE="${SCRIPT_DIR}/01_schema.sql"
if [ ! -f "$SCHEMA_FILE" ]; then
    SCHEMA_FILE="/mnt/c/Users/VICTUS15FB/Downloads/CrudWindowsFormsLinq/CrudWindowsFormsLinq/database/postgres/01_schema.sql"
fi
PGPASSWORD='crud_secret_dev_pass' psql -h 127.0.0.1 -U crud_app -d crud_windows_forms -f "$SCHEMA_FILE"

echo "PostgreSQL initialization completed successfully!"
