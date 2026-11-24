@echo off
title Cliente Web Blazor - CRUD Personas
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO CLIENTE WEB BLAZOR (Puerto 5292)
echo   Conectado a APIs: Windows (5144) y Ubuntu (Cloudflare AWS)
echo ===================================================================
echo.

cd /d "%~dp0src\Client.Blazor"

echo Compilando proyecto con los ultimos cambios...
dotnet build -c Debug

echo.
echo Iniciando servidor Blazor WebAssembly...
echo.
dotnet run --launch-profile http --no-build
pause
