@echo off
title Servidor Backend - CrudWindowsForms
chcp 65001 > nul
echo ========================================================
echo   Iniciando / Verificando Servidor Backend (WSL Ubuntu)
echo ========================================================
echo.

echo [1/3] Despertando WSL Ubuntu-24.04 y servicios...
wsl -u root -d Ubuntu-24.04 bash -c "systemctl start postgresql crudapi"

echo [2/3] Verificando estado del servicio crudapi en Ubuntu...
wsl -d Ubuntu-24.04 bash -c "systemctl is-active crudapi"

echo.
echo [3/3] Probando conexion con la API (http://localhost:5000/api/people)...
powershell -NoProfile -Command "try { $data = Invoke-RestMethod -Uri 'http://localhost:5000/api/people' -TimeoutSec 5; Write-Host '>>> CONEXION EXITOSA: El servidor esta respondiendo correctamente.' -ForegroundColor Green; Write-Host ('Registros en base de datos: ' + $data.Count); } catch { Write-Host '>>> ERROR: No se pudo conectar a la API.' -ForegroundColor Red; }"

echo.
echo ========================================================
echo Ya puedes iniciar el cliente Windows Forms desde Visual Studio (F5).
echo ========================================================
pause
