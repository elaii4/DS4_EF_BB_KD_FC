# ==============================================================================
# tests/api-smoke.ps1 - Smoke tests para la API REST de Personas
# ==============================================================================
param (
    [string]$BaseUrl = "http://localhost:5000"
)

$ErrorActionPreference = "Stop"
$TotalPassed = 0
$TotalFailed = 0

function Assert-Condition {
    param (
        [string]$TestName,
        [bool]$Condition,
        [string]$Details = ""
    )
    if ($Condition) {
        Write-Host " [PASS] $TestName" -ForegroundColor Green
        $script:TotalPassed++
    } else {
        Write-Host " [FAIL] $TestName - $Details" -ForegroundColor Red
        $script:TotalFailed++
    }
}

Write-Host "=== Iniciando Smoke Tests de la API en $BaseUrl ===" -ForegroundColor Cyan

# 1. GET /api/people - Listar todos ordenados por ID
try {
    $res = Invoke-RestMethod -Uri "$BaseUrl/api/people" -Method Get
    Assert-Condition "1. GET /api/people responde con lista de personas" ($res.Count -ge 3) "Conteo: $($res.Count)"
} catch {
    Assert-Condition "1. GET /api/people" $false $_.Exception.Message
}

# 2. GET /api/people?search=an - Búsqueda insensible a mayúsculas
try {
    $res = Invoke-RestMethod -Uri "$BaseUrl/api/people?search=an" -Method Get
    $names = $res | ForEach-Object { $_.name }
    $hasAna = $names -contains "Ana"
    Assert-Condition "2. GET /api/people?search=an devuelve Ana" ($hasAna) "Nombres: $($names -join ', ')"
} catch {
    Assert-Condition "2. GET /api/people?search=an" $false $_.Exception.Message
}

# 2b. GET /api/people?search=a - Búsqueda parcial que coincide con Ana y Maria
try {
    $res = Invoke-RestMethod -Uri "$BaseUrl/api/people?search=a" -Method Get
    $names = $res | ForEach-Object { $_.name }
    $hasAna = $names -contains "Ana"
    $hasMaria = $names -contains "Maria"
    Assert-Condition "2b. GET /api/people?search=a devuelve Ana y Maria" ($hasAna -and $hasMaria) "Nombres: $($names -join ', ')"
} catch {
    Assert-Condition "2b. GET /api/people?search=a" $false $_.Exception.Message
}

# 3. GET /api/people/{id} - Obtener por ID existente
try {
    $res = Invoke-RestMethod -Uri "$BaseUrl/api/people/1" -Method Get
    Assert-Condition "3. GET /api/people/1 devuelve persona existente (Ana)" ($res.id -eq 1 -and $res.name -eq "Ana") "Obtenido: $($res.name)"
} catch {
    Assert-Condition "3. GET /api/people/1" $false $_.Exception.Message
}

# 4. GET /api/people/999999 - Obtener por ID inexistente (debe ser 404)
try {
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people/999999" -Method Get -SkipHttpErrorCheck
    Assert-Condition "4. GET /api/people/999999 devuelve 404 Not Found" ($res.StatusCode -eq 404) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "4. GET /api/people/999999" ($_.Exception.Response.StatusCode.value__ -eq 404) $_.Exception.Message
}

# 5. POST /api/people - Crear persona válida
$createdId = $null
try {
    $body = @{ name = "Carlos"; age = 30 } | ConvertTo-Json
    $res = Invoke-RestMethod -Uri "$BaseUrl/api/people" -Method Post -Body $body -ContentType "application/json"
    $createdId = $res.id
    Assert-Condition "5. POST /api/people crea persona válida (201)" ($createdId -gt 0 -and $res.name -eq "Carlos") "Id creado: $createdId"
} catch {
    Assert-Condition "5. POST /api/people" $false $_.Exception.Message
}

# 6. POST /api/people con nombre vacío (debe ser 400)
try {
    $body = @{ name = ""; age = 30 } | ConvertTo-Json
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people" -Method Post -Body $body -ContentType "application/json" -SkipHttpErrorCheck
    Assert-Condition "6. POST /api/people con nombre vacío devuelve 400" ($res.StatusCode -eq 400) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "6. POST /api/people con nombre vacío" ($_.Exception.Response.StatusCode.value__ -eq 400) $_.Exception.Message
}

# 7. POST /api/people con edad inválida (121) (debe ser 400)
try {
    $body = @{ name = "Invalido"; age = 121 } | ConvertTo-Json
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people" -Method Post -Body $body -ContentType "application/json" -SkipHttpErrorCheck
    Assert-Condition "7. POST /api/people con edad 121 devuelve 400" ($res.StatusCode -eq 400) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "7. POST /api/people con edad 121" ($_.Exception.Response.StatusCode.value__ -eq 400) $_.Exception.Message
}

# 8. POST /api/people con edad negativa (-1) (debe ser 400)
try {
    $body = @{ name = "Invalido"; age = -1 } | ConvertTo-Json
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people" -Method Post -Body $body -ContentType "application/json" -SkipHttpErrorCheck
    Assert-Condition "8. POST /api/people con edad -1 devuelve 400" ($res.StatusCode -eq 400) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "8. POST /api/people con edad -1" ($_.Exception.Response.StatusCode.value__ -eq 400) $_.Exception.Message
}

# 9. PUT /api/people/{id} - Actualizar persona existente
if ($createdId) {
    try {
        $body = @{ name = "Carlos Actualizado"; age = 31 } | ConvertTo-Json
        $res = Invoke-WebRequest -Uri "$BaseUrl/api/people/$createdId" -Method Put -Body $body -ContentType "application/json" -SkipHttpErrorCheck
        Assert-Condition "9. PUT /api/people/$createdId actualiza y devuelve 204" ($res.StatusCode -eq 204) "Status: $($res.StatusCode)"
    } catch {
        Assert-Condition "9. PUT /api/people/$createdId" $false $_.Exception.Message
    }
} else {
    Assert-Condition "9. PUT omitido por falta de ID creado" $false
}

# 10. PUT /api/people/999999 - Actualizar persona inexistente (404)
try {
    $body = @{ name = "Inexistente"; age = 40 } | ConvertTo-Json
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people/999999" -Method Put -Body $body -ContentType "application/json" -SkipHttpErrorCheck
    Assert-Condition "10. PUT /api/people/999999 devuelve 404" ($res.StatusCode -eq 404) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "10. PUT /api/people/999999" ($_.Exception.Response.StatusCode.value__ -eq 404) $_.Exception.Message
}

# 11. DELETE /api/people/{id} - Eliminar persona existente
if ($createdId) {
    try {
        $res = Invoke-WebRequest -Uri "$BaseUrl/api/people/$createdId" -Method Delete -SkipHttpErrorCheck
        Assert-Condition "11. DELETE /api/people/$createdId devuelve 204" ($res.StatusCode -eq 204) "Status: $($res.StatusCode)"
    } catch {
        Assert-Condition "11. DELETE /api/people/$createdId" $false $_.Exception.Message
    }
} else {
    Assert-Condition "11. DELETE omitido por falta de ID creado" $false
}

# 12. DELETE /api/people/999999 - Eliminar persona inexistente (404)
try {
    $res = Invoke-WebRequest -Uri "$BaseUrl/api/people/999999" -Method Delete -SkipHttpErrorCheck
    Assert-Condition "12. DELETE /api/people/999999 devuelve 404" ($res.StatusCode -eq 404) "Status: $($res.StatusCode)"
} catch {
    Assert-Condition "12. DELETE /api/people/999999" ($_.Exception.Response.StatusCode.value__ -eq 404) $_.Exception.Message
}

Write-Host "=== Resumen: $TotalPassed Pasados, $TotalFailed Fallados ===" -ForegroundColor $(if ($TotalFailed -eq 0) { "Green" } else { "Red" })

if ($TotalFailed -gt 0) {
    exit 1
}
exit 0
