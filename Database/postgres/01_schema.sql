-- =============================================================================
-- 01_schema.sql - Esquema de referencia y datos iniciales para PostgreSQL 16
-- Base de datos: crud_windows_forms
-- Usuario de aplicacion: crud_app
-- =============================================================================

-- 1. Creacion de la tabla people
CREATE TABLE IF NOT EXISTS people (
    id   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    age  INTEGER NOT NULL CHECK (age BETWEEN 0 AND 120)
);

-- 2. Insercion de datos semilla (idempotente)
INSERT INTO people (name, age)
SELECT 'Ana', 20
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Ana' AND age = 20);

INSERT INTO people (name, age)
SELECT 'Luis', 25
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Luis' AND age = 25);

INSERT INTO people (name, age)
SELECT 'Maria', 22
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Maria' AND age = 22);

-- 3. Permisos minimos para el usuario crud_app (ejecutar conectado a crud_windows_forms)
DO $$
BEGIN
    IF EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'crud_app') THEN
        GRANT USAGE ON SCHEMA public TO crud_app;
        GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE people TO crud_app;
        GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO crud_app;
    END IF;
END
$$;

-- 4. Consulta de verificacion
SELECT id, name, age FROM people ORDER BY id;
