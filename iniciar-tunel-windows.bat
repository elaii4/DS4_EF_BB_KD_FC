@echo off
title Tunel Cloudflare - Servidor Windows (Puerto 5144)
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO TÚNEL PÚBLICO PARA EL SERVIDOR WINDOWS (Puerto 5144)
echo ===================================================================
echo.

cd /d "%~dp0"

if not exist cloudflared.exe (
    echo Descargando herramienta oficial de Cloudflare...
    curl -L --fail -o cloudflared.exe https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe
)

echo.
echo Iniciando túnel... Copia la dirección que termine en .trycloudflare.com
echo.
cloudflared.exe tunnel --url http://localhost:5144
pause

