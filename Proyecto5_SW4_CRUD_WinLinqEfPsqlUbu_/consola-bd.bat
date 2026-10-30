@echo off
title Consola PostgreSQL - crud_windows_forms
chcp 65001 > nul
echo ===================================================================
echo   CONSOLA INTERACTIVA DE POSTGRESQL (crud_windows_forms)
echo ===================================================================
echo.
echo Conectando a la base de datos crud_windows_forms en Ubuntu WSL...
echo Comandos utiles:
echo   - SELECT * FROM people ORDER BY id;
echo   - \d people            (Ver estructura y tipos de datos de la tabla)
echo   - \l                   (Listar todas las bases de datos)
echo   - \q                   (Salir de la consola)
echo ===================================================================
echo.
wsl -u postgres -d Ubuntu-24.04 psql -d crud_windows_forms
