@echo off
title Servidor Windows - CrudWindowsForms API
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO SERVIDOR WEB API EN WINDOWS (Puerto 5144)
echo   Base de Datos: crud_windows_forms_win (PostgreSQL)
echo ===================================================================
echo.

cd /d "%~dp0src\Api"

echo Compilando y ejecutando API en Windows...
echo.
set ASPNETCORE_ENVIRONMENT=Windows
set ASPNETCORE_URLS=http://0.0.0.0:5144

dotnet run --no-launch-profile
pause
