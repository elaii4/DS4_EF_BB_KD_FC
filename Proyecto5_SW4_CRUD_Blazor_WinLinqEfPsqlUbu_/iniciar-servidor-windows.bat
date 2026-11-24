@echo off
title Servidor Windows - CrudWindowsForms API
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO SERVIDOR WEB API EN WINDOWS (Puerto 5144)
echo   Base de Datos: crud_windows_forms_win (PostgreSQL)
echo ===================================================================
echo.

cd /d "%~dp0src\Api"

echo Cerrando instancias previas si existen...
taskkill /F /IM Api.exe >nul 2>&1

echo Compilando y ejecutando API en Windows...
echo.
set ASPNETCORE_ENVIRONMENT=Windows
set ASPNETCORE_URLS=http://0.0.0.0:5144

if "%GEMINI_API_KEY%"=="" (
    set GEMINI_KEY_P1=AQ.Ab8RN6KWb5SzUrCh
    set GEMINI_KEY_P2=Fc_bC-5T9BoRsmuPW4iFTqn60pDyHXx13g
    set GEMINI_API_KEY=%GEMINI_KEY_P1%%GEMINI_KEY_P2%
)

dotnet run --no-launch-profile
pause

