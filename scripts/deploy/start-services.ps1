# Start Services Script
# Author: HNJM
# Date: 2025-11-13

param(
    [string]$Environment = "Development"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "   Todo App - Start Services               " -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

$apiPath = "src/Presentation/TodoBlazorApp.API"
$uiPath = "src/Presentation/TodoBlazorApp.BlazorUI"

# Start API
Write-Host "🚀 تشغيل API..." -ForegroundColor Blue
Start-Process powershell -ArgumentList @"
    cd $apiPath;
    `$env:ASPNETCORE_ENVIRONMENT='$Environment';
    dotnet run
"@ -NoNewWindow

Start-Sleep -Seconds 5

# Start UI
Write-Host "🚀 تشغيل Blazor UI..." -ForegroundColor Blue
Start-Process powershell -ArgumentList @"
    cd $uiPath;
    `$env:ASPNETCORE_ENVIRONMENT='$Environment';
    dotnet run
"@ -NoNewWindow

Start-Sleep -Seconds 10

# Check services
Write-Host ""
Write-Host "🔍 فحص الخدمات..." -ForegroundColor Blue

try {
    $apiHealth = Invoke-WebRequest -Uri "http://localhost:7000/api/health" -UseBasicParsing
    Write-Host "✅ API يعمل بشكل صحيح" -ForegroundColor Green
} catch {
    Write-Host "❌ API لا يستجيب" -ForegroundColor Red
}

try {
    $uiHealth = Invoke-WebRequest -Uri "http://localhost:5000" -UseBasicParsing
    Write-Host "✅ UI يعمل بشكل صحيح" -ForegroundColor Green
} catch {
    Write-Host "❌ UI لا يستجيب" -ForegroundColor Red
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "📍 API: http://localhost:7000" -ForegroundColor Cyan
Write-Host "📍 Swagger: http://localhost:7000/swagger" -ForegroundColor Cyan
Write-Host "📍 UI: http://localhost:5000" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan