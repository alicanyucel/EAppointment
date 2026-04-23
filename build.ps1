# EAppointment - Build and Run Script for Windows
param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('build', 'test', 'run', 'docker-up', 'docker-down', 'sonar', 'load-test', 'clean', 'setup', 'help')]
    [string]$Command = 'help'
)

function Show-Help {
    Write-Host "EAppointment - Enterprise Appointment Management System" -ForegroundColor Green
    Write-Host ""
    Write-Host "Available commands:" -ForegroundColor Yellow
    Write-Host "  build         - Build the solution" -ForegroundColor Cyan
    Write-Host "  test          - Run all tests" -ForegroundColor Cyan
    Write-Host "  run           - Run the application" -ForegroundColor Cyan
    Write-Host "  docker-up     - Start all Docker services" -ForegroundColor Cyan
    Write-Host "  docker-down   - Stop all Docker services" -ForegroundColor Cyan
    Write-Host "  sonar         - Run SonarQube analysis" -ForegroundColor Cyan
    Write-Host "  load-test     - Run k6 load test" -ForegroundColor Cyan
    Write-Host "  clean         - Clean build artifacts" -ForegroundColor Cyan
    Write-Host "  setup         - Initial setup" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage: .\build.ps1 -Command <command>" -ForegroundColor Gray
}

function Build-Solution {
    Write-Host "Building solution..." -ForegroundColor Green
    dotnet restore
    dotnet build --configuration Release
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Build successful!" -ForegroundColor Green
    } else {
        Write-Host "Build failed!" -ForegroundColor Red
        exit 1
    }
}

function Run-Tests {
    Write-Host "Running tests..." -ForegroundColor Green
    dotnet test --configuration Release --verbosity normal
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Tests passed!" -ForegroundColor Green
    } else {
        Write-Host "Tests failed!" -ForegroundColor Red
        exit 1
    }
}

function Run-Application {
    Write-Host "Starting application..." -ForegroundColor Green
    Set-Location -Path "EAppointment.WebApi"
    dotnet run
    Set-Location -Path ".."
}

function Start-Docker {
    Write-Host "Starting Docker services..." -ForegroundColor Green
    docker-compose up -d
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Docker services started!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Services:" -ForegroundColor Yellow
        Write-Host "  API:              http://localhost:5000" -ForegroundColor Cyan
        Write-Host "  Grafana:          http://localhost:3000 (admin/admin)" -ForegroundColor Cyan
        Write-Host "  Prometheus:       http://localhost:9090" -ForegroundColor Cyan
        Write-Host "  Jaeger:           http://localhost:16686" -ForegroundColor Cyan
        Write-Host "  RabbitMQ:         http://localhost:15672 (guest/guest)" -ForegroundColor Cyan
        Write-Host "  SonarQube:        http://localhost:9000 (admin/admin)" -ForegroundColor Cyan
    } else {
        Write-Host "Failed to start Docker services!" -ForegroundColor Red
        exit 1
    }
}

function Stop-Docker {
    Write-Host "Stopping Docker services..." -ForegroundColor Green
    docker-compose down
    Write-Host "Docker services stopped!" -ForegroundColor Green
}

function Run-SonarAnalysis {
    Write-Host "Running SonarQube analysis..." -ForegroundColor Green
    dotnet tool install --global dotnet-sonarscanner
    dotnet sonarscanner begin /k:"eappointment" /d:sonar.host.url="http://localhost:9000"
    dotnet build --configuration Release
    dotnet test --collect:"XPlat Code Coverage"
    dotnet sonarscanner end
    Write-Host "SonarQube analysis complete! Visit http://localhost:9000" -ForegroundColor Green
}

function Run-LoadTest {
    Write-Host "Running load test..." -ForegroundColor Green
    k6 run tests/load/load-test.js
}

function Clean-Solution {
    Write-Host "Cleaning solution..." -ForegroundColor Green
    dotnet clean
    Get-ChildItem -Path . -Include bin,obj -Recurse | Remove-Item -Recurse -Force
    Write-Host "Clean complete!" -ForegroundColor Green
}

function Setup-Project {
    Write-Host "Setting up project..." -ForegroundColor Green
    Start-Docker
    Start-Sleep -Seconds 10
    Build-Solution
    Run-Tests
    Write-Host ""
    Write-Host "Setup complete! Run '.\build.ps1 -Command run' to start the application" -ForegroundColor Green
}

# Main execution
switch ($Command) {
    'build' { Build-Solution }
    'test' { Run-Tests }
    'run' { Run-Application }
    'docker-up' { Start-Docker }
    'docker-down' { Stop-Docker }
    'sonar' { Run-SonarAnalysis }
    'load-test' { Run-LoadTest }
    'clean' { Clean-Solution }
    'setup' { Setup-Project }
    'help' { Show-Help }
    default { Show-Help }
}
