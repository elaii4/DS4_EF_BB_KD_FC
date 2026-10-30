-- =============================================================================
-- 02_schema_windows.sql - Esquema y datos semilla para Servidor Windows
-- Base de datos: crud_windows_forms_win
-- Usuario de aplicacion: crud_app
-- =============================================================================

CREATE TABLE IF NOT EXISTS people (
    id   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    age  INTEGER NOT NULL CHECK (age BETWEEN 0 AND 120)
);

-- Inserción de datos semilla específicos para el Servidor Windows
INSERT INTO people (name, age)
SELECT 'Carlos Mendoza', 34
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Carlos Mendoza');

INSERT INTO people (name, age)
SELECT 'Valeria Gómez', 26
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Valeria Gómez');

INSERT INTO people (name, age)
SELECT 'Diego Herrera', 41
WHERE NOT EXISTS (SELECT 1 FROM people WHERE name = 'Diego Herrera');

-- Permisos para el usuario crud_app
DO $$
BEGIN
    IF EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'crud_app') THEN
        GRANT USAGE ON SCHEMA public TO crud_app;
        GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE people TO crud_app;
        GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO crud_app;
    END IF;
END
$$;

SELECT id, name, age FROM people ORDER BY id;
