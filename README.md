# EAppointment - Enterprise Grade Appointment Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?style=flat&logo=docker)](https://www.docker.com/)
[![CI/CD](https://github.com/alicanyucel/EAppointment/workflows/CI/CD%20Pipeline/badge.svg)](https://github.com/alicanyucel/EAppointment/actions)
[![Code Coverage](https://codecov.io/gh/alicanyucel/EAppointment/branch/master/graph/badge.svg)](https://codecov.io/gh/alicanyucel/EAppointment)

## 🚀 Enterprise Features

This project implements **production-ready**, **enterprise-level** features including:

- ✅ **Clean Architecture** - Domain-driven design with CQRS pattern
- ✅ **Unit & Integration Tests** - Comprehensive test coverage with xUnit
- ✅ **SonarQube Integration** - Code quality analysis
- ✅ **Docker & Docker Compose** - Containerized deployment
- ✅ **Kubernetes** - Production-ready K8s manifests with auto-scaling
- ✅ **Saga Pattern** - Distributed transaction management
- ✅ **OpenTelemetry** - Distributed tracing and observability
- ✅ **Prometheus & Grafana** - Metrics and visualization
- ✅ **Serilog** - Structured logging with multiple sinks
- ✅ **Rate Limiting** - Advanced rate limiting with multiple strategies
- ✅ **HATEOAS** - Hypermedia-driven REST API
- ✅ **Load Testing** - k6 performance testing
- ✅ **CI/CD Pipeline** - GitHub Actions automation
- ✅ **Health Checks** - Comprehensive service monitoring
- ✅ **Redis Caching** - Distributed caching
- ✅ **RabbitMQ** - Message queue for async operations

## 📋 Table of Contents

- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Running Tests](#running-tests)
- [Monitoring & Observability](#monitoring--observability)
- [Load Testing](#load-testing)
- [CI/CD](#cicd)
- [Docker Deployment](#docker-deployment)
- [Code Quality](#code-quality)

## 🏗️ Architecture

```
EAppointment/
├── EAppointment.Domain/          # Domain entities and interfaces
├── EAppointment.Application/     # Business logic and use cases
├── EAppointment.Infrastructure/  # Data access and external services
├── EAppointment.WebApi/          # API controllers and startup
├── EAppointment.Tests/           # Unit tests
├── EAppointment.IntegrationTests/ # Integration tests
├── tests/load/                   # k6 load tests
├── monitoring/                   # Prometheus, Grafana, OpenTelemetry configs
└── .github/workflows/            # CI/CD pipelines
```

### Clean Architecture Layers

1. **Domain Layer** - Core business entities and rules
2. **Application Layer** - Use cases and business logic (CQRS with MediatR)
3. **Infrastructure Layer** - Data persistence and external integrations
4. **Presentation Layer** - RESTful API with Swagger

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Kubernetes](https://kubernetes.io/) (minikube, Docker Desktop, or cloud provider)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [k6](https://k6.io/docs/getting-started/installation/) (for load testing)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🚀 Quick Start

### 1. Clone the repository

```bash
git clone https://github.com/alicanyucel/EAppointment.git
cd EAppointment
```

### 2. Start infrastructure with Docker Compose

```bash
docker-compose up -d
```

This will start:
- SQL Server (port 1433)
- Redis (port 6379)
- RabbitMQ (port 5672, Management UI: 15672)
- Prometheus (port 9090)
- Grafana (port 3000)
- Jaeger (port 16686)
- OpenTelemetry Collector (port 4317)
- SonarQube (port 9000)

### 3. Run the application

```bash
cd EAppointment.WebApi
dotnet run
```

The API will be available at `https://localhost:5001` (Swagger UI)

### 4. Access Services

| Service | URL | Credentials |
|---------|-----|-------------|
| **API Swagger** | http://localhost:5000/swagger | - |
| **Grafana** | http://localhost:3000 | admin/admin |
| **Prometheus** | http://localhost:9090 | - |
| **Jaeger UI** | http://localhost:16686 | - |
| **RabbitMQ Management** | http://localhost:15672 | guest/guest |
| **SonarQube** | http://localhost:9000 | admin/admin |

## 🧪 Running Tests

### Unit Tests

```bash
dotnet test EAppointment.Tests/EAppointment.Tests.csproj
```

### Integration Tests

```bash
dotnet test EAppointment.IntegrationTests/EAppointment.IntegrationTests.csproj
```

### Test Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

View coverage report in `coverage.opencover.xml`

## 📊 Monitoring & Observability

### OpenTelemetry

The application is instrumented with OpenTelemetry for:
- **Traces** - Distributed tracing across services
- **Metrics** - Performance metrics and custom counters
- **Logs** - Structured logging with context

### Prometheus Metrics

Access metrics at: `http://localhost:5000/metrics`

Key metrics:
- `http_requests_total` - Total HTTP requests
- `http_request_duration_seconds` - Request duration
- `dotnet_total_memory_bytes` - Memory usage
- `process_cpu_usage` - CPU usage

### Grafana Dashboards

Pre-configured dashboard available at: `http://localhost:3000`

Default credentials: `admin/admin`

Dashboards include:
- HTTP Request Rate
- CPU & Memory Usage
- Database Query Performance
- Error Rates

### Jaeger Tracing

View distributed traces at: `http://localhost:16686`

## ⚡ Load Testing

### Run Basic Load Test

```bash
k6 run tests/load/load-test.js
```

### Run Spike Test

```bash
k6 run tests/load/spike-test.js
```

### Run Stress Test

```bash
k6 run tests/load/stress-test.js
```

### Custom Load Test

```bash
k6 run --vus 100 --duration 30s tests/load/load-test.js
```

## 🔄 CI/CD

### GitHub Actions Pipeline

The project includes a complete CI/CD pipeline:

1. **Build** - Compile .NET solution
2. **Test** - Run unit and integration tests
3. **Code Coverage** - Generate and upload coverage reports
4. **SonarQube Analysis** - Code quality scanning
5. **Docker Build** - Build and push Docker images
6. **Load Test** - Performance testing

### Running SonarQube Locally

```bash
# Start SonarQube
docker-compose up -d sonarqube

# Run analysis
dotnet sonarscanner begin /k:"eappointment" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="your-token"
dotnet build
dotnet test --collect:"XPlat Code Coverage"
dotnet sonarscanner end /d:sonar.login="your-token"
```

## 🐳 Docker Deployment

### Build Docker Image

```bash
docker build -t eappointment-api:latest .
```

### Run with Docker Compose

```bash
docker-compose up -d
```

### Production Deployment

```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## 📈 Code Quality

### SonarQube Integration

Code quality metrics tracked:
- **Code Coverage** - Minimum 80%
- **Code Smells** - Auto-detected
- **Security Vulnerabilities** - Continuous scanning
- **Technical Debt** - Tracked and reported
- **Duplications** - Detected and highlighted

### Best Practices

- Clean Code principles
- SOLID principles
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple, Stupid)
- YAGNI (You Aren't Gonna Need It)

## 🔧 Configuration

### Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost;Database=EAppointmentDb;...

# OpenTelemetry
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
OTEL_SERVICE_NAME=eappointment-api

# Redis
ConnectionStrings__Redis=localhost:6379

# RabbitMQ
RabbitMQ__Host=localhost
RabbitMQ__Username=guest
RabbitMQ__Password=guest
```

## 📚 API Documentation

Interactive API documentation available at:
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health
- **Metrics**: http://localhost:5000/metrics

## 🎯 Saga Pattern Example

The project implements the Saga pattern for distributed transactions:

```csharp
// Create appointment with Saga orchestration
POST /api/appointments/create-with-saga
{
    "doctorId": "guid",
    "patientId": "guid",
    "startDate": "2024-01-01T10:00:00",
    "endDate": "2024-01-01T11:00:00"
}
```

Saga steps:
1. Validate doctor availability
2. Create appointment
3. Send notification

If any step fails, compensating transactions rollback changes.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 👨‍💻 Author

**Ali Can Yücel**
- GitHub: [@alicanyucel](https://github.com/alicanyucel)

## 🌟 Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- Microsoft .NET Documentation

---

**Note**: This is a production-ready, enterprise-grade application with all modern DevOps and software engineering practices implemented.
