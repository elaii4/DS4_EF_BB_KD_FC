@echo off
title Habilitar Conexion para Companero (Red Local)
chcp 65001 > nul
echo ===================================================================
echo     CONFIGURAR ACCESO DE RED LOCAL (PUERTOS 5000 Y 5144)
echo ===================================================================
echo.

echo [1/3] Habilitando puertos 5000 (Ubuntu) y 5144 (Windows) en Firewall...
netsh advfirewall firewall delete rule name="CrudAPI Port 5000" >nul 2>&1
netsh advfirewall firewall add rule name="CrudAPI Port 5000" dir=in action=allow protocol=TCP localport=5000

netsh advfirewall firewall delete rule name="CrudAPI Port 5144" >nul 2>&1
netsh advfirewall firewall add rule name="CrudAPI Port 5144" dir=in action=allow protocol=TCP localport=5144

echo.
echo [2/3] Configurando acceso a WSL/Docker en puerto 5000...
netsh interface portproxy delete v4tov4 listenport=5000 listenaddress=0.0.0.0 >nul 2>&1
netsh interface portproxy add v4tov4 listenport=5000 listenaddress=0.0.0.0 connectport=5000 connectaddress=127.0.0.1

echo.
echo [3/3] Tus direcciones IPv4 actuales:
echo -------------------------------------------------------------------
ipconfig | findstr /i "IPv4"
echo -------------------------------------------------------------------
echo.
echo Identifica la IP de tu adaptador de Wi-Fi.
echo Esa es la que tu compañero colocará en el archivo App.config:
echo.
echo   ApiUrl_Windows -> http://TU_IP:5144
echo   ApiUrl_Ubuntu  -> http://TU_IP:5000
echo.
echo ===================================================================
pause
