@echo off
title Habilitar Conexion para Companero
chcp 65001 > nul
echo ===================================================================
echo     CONFIGURAR ACCESO DE RED LOCAL (FIREWALL Y PUERTO 5000)
echo ===================================================================
echo.

echo [1/3] Habilitando puerto 5000 en el Firewall de Windows...
netsh advfirewall firewall add rule name="CrudAPI Port 5000" dir=in action=allow protocol=TCP localport=5000

echo.
echo [2/3] Configurando redireccion hacia WSL en el puerto 5000...
netsh interface portproxy delete v4tov4 listenport=5000 listenaddress=0.0.0.0 >nul 2>&1
netsh interface portproxy add v4tov4 listenport=5000 listenaddress=0.0.0.0 connectport=5000 connectaddress=localhost

echo.
echo [3/3] Tus direcciones IPv4 actuales:
echo -------------------------------------------------------------------
ipconfig | findstr /i "IPv4"
echo -------------------------------------------------------------------
echo.
echo Identifica la IP de tu adaptador de Wi-Fi (la que te dio tu celular).
echo.
echo Esa es la que tu companero pondra en su App.config:
echo   http://TU_IP:5000
echo.
echo ===================================================================
pause
