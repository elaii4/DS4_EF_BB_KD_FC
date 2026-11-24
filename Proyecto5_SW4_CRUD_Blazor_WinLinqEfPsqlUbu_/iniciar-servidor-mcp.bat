@echo off
title Servidor MCP (Capa 4) - Herramientas SQL PostgreSQL
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO SERVIDOR MCP (Model Context Protocol - Puerto 5145)
echo   Capa 4: Herramientas SQL, Permisos y Consultas hacia PostgreSQL
echo ===================================================================
echo.

cd /d "%~dp0src\Mcp.Server"

echo Compilando y ejecutando Servidor MCP (.NET 8)...
echo.
set ASPNETCORE_URLS=http://0.0.0.0:5145

dotnet run --no-launch-profile
pause
