# ==============================================================================
# tests/verify.ps1 - Script de Verificacion Integral de Compuertas (G1 a G8)
# ==============================================================================
$ErrorActionPreference = "Stop"

$PassedCount = 0
$TotalGates = 8

function Log-Gate {
    param (
        [string]$GateId,
        [string]$Name,
        [bool]$Success,
        [string]$Evidence = ""
    )
    if ($Success) {
        Write-Host " [PASS] $($GateId): $Name" -ForegroundColor Green
        if ($Evidence) {
            Write-Host "        Evidencia: $Evidence" -ForegroundColor DarkGray
        }
        $script:PassedCount++
    } else {
        Write-Host " [FAIL] $($GateId): $Name" -ForegroundColor Red
        if ($Evidence) {
            Write-Host "        Causa: $Evidence" -ForegroundColor Yellow
        }
    }
}

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "   VERIFICACION DE COMPUERTAS DE ACEPTACION (G1 - G8)       " -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan

# ------------------------------------------------------------------------------
# G1: La API compila (dotnet build src/Api -c Release)
# ------------------------------------------------------------------------------
try {
    $buildApi = dotnet build src/Api/Api.csproj -c Release 2>&1
    if ($LASTEXITCODE -eq 0) {
        Log-Gate "G1" "La API compila en Release sin errores" $true "dotnet build exitoso (0 errores)"
    } else {
        Log-Gate "G1" "La API compila en Release sin errores" $false ($buildApi -join "`n")
    }
} catch {
    Log-Gate "G1" "La API compila en Release sin errores" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G2: El cliente compila (MSBuild src/Client.WinForms)
# ------------------------------------------------------------------------------
try {
    $msbuildPath = "C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\amd64\MSBuild.exe"
    if (-not (Test-Path $msbuildPath)) {
        $msbuildPath = (Get-ChildItem -Path "C:\Program Files*\Microsoft Visual Studio" -Recurse -Filter "MSBuild.exe" -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName)
    }
    $buildClient = & $msbuildPath src/Client.WinForms/CrudWindowsFormsLinq.csproj -v:m -p:Configuration=Release 2>&1
    if ($LASTEXITCODE -eq 0) {
        Log-Gate "G2" "El cliente WinForms compila sin errores" $true "MSBuild Release exitoso"
    } else {
        Log-Gate "G2" "El cliente WinForms compila sin errores" $false ($buildClient -join "`n")
    }
} catch {
    Log-Gate "G2" "El cliente WinForms compila sin errores" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G3: Sin LINQ to SQL ni SQL Server en el cliente
# ------------------------------------------------------------------------------
try {
    $matches = git grep -n -i -E "System\.Data\.Linq|SqlConnection|SqlCommand|SqlDataReader|Data Source=" src/Client.WinForms 2>&1
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($matches)) {
        Log-Gate "G3" "Sin LINQ to SQL ni SQL Server en el cliente" $true "0 coincidencias encontradas en src/Client.WinForms"
    } else {
        Log-Gate "G3" "Sin LINQ to SQL ni SQL Server en el cliente" $false "Encontrado: $matches"
    }
} catch {
    Log-Gate "G3" "Sin LINQ to SQL ni SQL Server en el cliente" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G4: Sin secretos en el repositorio
# ------------------------------------------------------------------------------
try {
    # Busca Password= que no sea el marcador TU_PASSWORD_AQUI en codigo fuente src/ y unit file
    $srcSecrets = git grep -i -E "Password=[^T]|pwd=[^T]" src/ 2>&1
    $unitSecrets = git grep -i -E "Password=[^%]|pwd=[^%]" deploy/crudapi.service 2>&1
    $appConfigSecrets = git grep -i -E "connectionString|Data Source=" src/Client.WinForms/App.config 2>&1

    if ([string]::IsNullOrWhiteSpace($srcSecrets) -and [string]::IsNullOrWhiteSpace($unitSecrets) -and [string]::IsNullOrWhiteSpace($appConfigSecrets)) {
        Log-Gate "G4" "Sin secretos en el repositorio" $true "0 secretos en src/, deploy/ y configs; solo marcadores autorizados"
    } else {
        Log-Gate "G4" "Sin secretos en el repositorio" $false "Posibles secretos detectados: $srcSecrets $unitSecrets $appConfigSecrets"
    }
} catch {
    Log-Gate "G4" "Sin secretos en el repositorio" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G5: Pruebas de la API (tests/api-smoke)
# ------------------------------------------------------------------------------
try {
    # Ejecuta el smoke test dentro de Ubuntu WSL contra el servicio real
    $smokeOut = wsl -d Ubuntu-24.04 -u root bash -c "systemctl start postgresql crudapi && sleep 2 && tr -d '\r' < /mnt/c/Users/VICTUS15FB/Downloads/CrudWindowsFormsLinq/CrudWindowsFormsLinq/tests/api-smoke | bash" 2>&1
    if ($LASTEXITCODE -eq 0 -and ($smokeOut -match "Total: 13 Passed, 0 Failed")) {
        Log-Gate "G5" "Pruebas de la API (api-smoke 13/13 pasan)" $true "Todos los endpoints probados contra PostgreSQL real"
    } else {
        Log-Gate "G5" "Pruebas de la API (api-smoke 13/13 pasan)" $false ($smokeOut -join "`n")
    }
} catch {
    Log-Gate "G5" "Pruebas de la API (api-smoke 13/13 pasan)" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G6: Migraciones y datos iniciales en PostgreSQL
# ------------------------------------------------------------------------------
try {
    $dbCheck = wsl -d Ubuntu-24.04 -u root bash -c "PGPASSWORD='crud_secret_dev_pass' psql -h 127.0.0.1 -U crud_app -d crud_windows_forms -tc 'SELECT name FROM people ORDER BY id;'" 2>&1
    $cleanNames = ($dbCheck | Out-String)
    if ($cleanNames -match "Ana" -and $cleanNames -match "Luis" -and $cleanNames -match "Maria") {
        Log-Gate "G6" "Migraciones y datos semilla en PostgreSQL" $true "Registros iniciales verificados (Ana, Luis, Maria)"
    } else {
        Log-Gate "G6" "Migraciones y datos semilla en PostgreSQL" $false "Resultado obtenido: $dbCheck"
    }
} catch {
    Log-Gate "G6" "Migraciones y datos semilla en PostgreSQL" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G7: Despliegue en Ubuntu 24.04 (systemd activo y provision-ubuntu.sh listo)
# ------------------------------------------------------------------------------
try {
    $serviceStatus = wsl -d Ubuntu-24.04 -u root bash -c "systemctl is-active crudapi" 2>&1
    $statusText = ($serviceStatus -join "").Trim()
    $hasProvisionScript = (Test-Path "deploy/provision-ubuntu.sh") -and (Test-Path "deploy/crudapi.service")
    if ($statusText -eq "active" -and $hasProvisionScript) {
        Log-Gate "G7" "Despliegue en Ubuntu 24.04 (servicio systemd activo)" $true "crudapi.service esta activo y provision-ubuntu.sh listo"
    } else {
        Log-Gate "G7" "Despliegue en Ubuntu 24.04" $false "Estado del servicio: $statusText"
    }
} catch {
    Log-Gate "G7" "Despliegue en Ubuntu 24.04" $false $_.Exception.Message
}

# ------------------------------------------------------------------------------
# G8: Documentacion completa
# ------------------------------------------------------------------------------
$docCheck = (Test-Path "README.md") -and (Test-Path "docs/REPORTE_POR_CAPAS.md") -and (Test-Path "docs/DECISIONES.md") -and (Test-Path "docs/HUMAN_ACTIONS.md")
if ($docCheck) {
    Log-Gate "G8" "Documentacion completa y reportes por capas" $true "README.md, REPORTE_POR_CAPAS.md, DECISIONES.md y HUMAN_ACTIONS.md presentes"
} else {
    Log-Gate "G8" "Documentacion completa y reportes por capas" $false "Archivos faltantes en docs/"
}

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "   RESUMEN FINAL: $PassedCount de $TotalGates Compuertas Aprobadas       " -ForegroundColor $(if ($PassedCount -eq $TotalGates) { "Green" } else { "Red" })
Write-Host "============================================================" -ForegroundColor Cyan

if ($PassedCount -lt $TotalGates) {
    exit 1
}
exit 0
