@echo off
title Monitor de Consultas SQL en Tiempo Real - Backend CrudAPI
chcp 65001 > nul
echo ===================================================================
echo   MONITOR EN TIEMPO REAL DE ACCIONES Y CONSULTAS SQL
echo ===================================================================
echo.
echo Mostrando en vivo las consultas SQL generadas por Entity Framework Core.
echo Cada vez que uses el formulario Windows Forms (Crear, Editar, Eliminar),
echo veras aqui reflejada la sentencia SQL exacta con sus parametros.
echo.
echo Presiona Ctrl + C para detener el monitoreo en cualquier momento.
echo ===================================================================
echo.
wsl -d Ubuntu-24.04 bash -c "journalctl -u crudapi -f -n 15 --no-pager"
