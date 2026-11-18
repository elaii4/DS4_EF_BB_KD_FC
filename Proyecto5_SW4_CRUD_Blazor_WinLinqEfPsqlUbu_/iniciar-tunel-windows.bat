@echo off
title Tunel Cloudflare - Servidor Windows (Puerto 5144)
chcp 65001 > nul
echo ===================================================================
echo   INICIANDO TUNEL CLOUDFLARE PARA WINDOWS (Puerto 5144)
echo   Permite a tus companeros conectarse a tu API de Windows
echo ===================================================================
echo.

if not exist "%~dp0cloudflared.exe" (
    echo [AVISO] Descargando cloudflared.exe oficial de Cloudflare...
    powershell -Command "Invoke-WebRequest -Uri 'https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe' -OutFile '%~dp0cloudflared.exe'"
)

echo.
echo Conectando puerto 5144 a la red global de Cloudflare...
echo Copia la URL 'https://xxxx.trycloudflare.com' que aparezca abajo.
echo.
"%~dp0cloudflared.exe" tunnel --url http://localhost:5144
pause
