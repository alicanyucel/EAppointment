.PHONY: help build test run docker-up docker-down clean restore sonar load-test

help: ## Show this help
	@echo "EAppointment - Enterprise Appointment Management System"
	@echo ""
	@echo "Available targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  %-20s %s\n", $$1, $$2}'

restore: ## Restore NuGet packages
	dotnet restore

build: restore ## Build the solution
	dotnet build --configuration Release

test: ## Run all tests
	dotnet test --configuration Release --verbosity normal

test-coverage: ## Run tests with coverage
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/

unit-test: ## Run unit tests only
	dotnet test EAppointment.Tests/EAppointment.Tests.csproj --configuration Release

integration-test: ## Run integration tests only
	dotnet test EAppointment.IntegrationTests/EAppointment.IntegrationTests.csproj --configuration Release

run: ## Run the application
	cd EAppointment.WebApi && dotnet run

watch: ## Run the application with hot reload
	cd EAppointment.WebApi && dotnet watch run

docker-build: ## Build Docker image
	docker build -t eappointment-api:latest .

docker-up: ## Start all services with Docker Compose
	docker-compose up -d

docker-down: ## Stop all Docker services
	docker-compose down

docker-logs: ## View Docker logs
	docker-compose logs -f

docker-restart: docker-down docker-up ## Restart Docker services

sonar-start: ## Start SonarQube
	docker-compose up -d sonarqube

sonar-scan: ## Run SonarQube analysis
	dotnet sonarscanner begin /k:"eappointment" /d:sonar.host.url="http://localhost:9000"
	dotnet build --configuration Release
	dotnet test --collect:"XPlat Code Coverage"
	dotnet sonarscanner end

load-test: ## Run k6 load test
	k6 run tests/load/load-test.js

spike-test: ## Run k6 spike test
	k6 run tests/load/spike-test.js

stress-test: ## Run k6 stress test
	k6 run tests/load/stress-test.js

clean: ## Clean build artifacts
	dotnet clean
	find . -type d -name bin -exec rm -rf {} +
	find . -type d -name obj -exec rm -rf {} +

format: ## Format code
	dotnet format

migration-add: ## Add new migration (use NAME=MigrationName)
	cd EAppointment.Infrastructure && dotnet ef migrations add $(NAME) --startup-project ../EAppointment.WebApi

migration-update: ## Update database
	cd EAppointment.Infrastructure && dotnet ef database update --startup-project ../EAppointment.WebApi

health-check: ## Check service health
	curl http://localhost:5000/health

metrics: ## View Prometheus metrics
	curl http://localhost:5000/metrics

setup: docker-up restore build ## Initial setup

ci: restore build test ## Run CI pipeline locally

all: clean restore build test docker-build ## Build everything
