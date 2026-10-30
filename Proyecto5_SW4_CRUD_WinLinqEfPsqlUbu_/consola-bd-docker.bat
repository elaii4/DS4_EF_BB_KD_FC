@echo off
title Consola PostgreSQL (Docker) - crud_windows_forms
chcp 65001 > nul
echo ===================================================================
echo   CONSOLA INTERACTIVA DE POSTGRESQL EN DOCKER (crud_postgres)
echo ===================================================================
echo.
echo Conectando a la base de datos crud_windows_forms dentro del contenedor...
echo.
echo Comandos utiles:
echo   - SELECT * FROM people ORDER BY id;
echo   - \dt                  (Ver tablas)
echo   - \d people            (Ver estructura)
echo   - \q                   (Salir)
echo ===================================================================
echo.
docker exec -it crud_postgres psql -U crud_app -d crud_windows_forms

