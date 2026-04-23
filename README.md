# EAppointment - Enterprise Grade Appointment Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?style=flat&logo=docker)](https://www.docker.com/)
[![Kubernetes](https://img.shields.io/badge/Kubernetes-Ready-326CE5?style=flat&logo=kubernetes)](https://kubernetes.io/)
[![CI/CD](https://github.com/alicanyucel/EAppointment/workflows/CI/CD%20Pipeline/badge.svg)](https://github.com/alicanyucel/EAppointment/actions)
[![Code Coverage](https://codecov.io/gh/alicanyucel/EAppointment/branch/master/graph/badge.svg)](https://codecov.io/gh/alicanyucel/EAppointment)
[![SonarQube](https://img.shields.io/badge/SonarQube-Integrated-4E9BCD?style=flat&logo=sonarqube)](http://localhost:9000)

## 🚀 Enterprise Features

This project implements **production-ready**, **enterprise-level** features including:

### Architecture & Design Patterns
- ✅ **Clean Architecture** - Domain-driven design with CQRS pattern
- ✅ **Saga Pattern** - Distributed transaction management with compensation
- ✅ **Repository Pattern** - Generic repository implementation
- ✅ **HATEOAS** - Hypermedia-driven REST API for discoverability

### Testing & Quality Assurance
- ✅ **Unit Tests** - Comprehensive coverage with xUnit, Moq, FluentAssertions
- ✅ **Integration Tests** - End-to-end API testing
- ✅ **Load Testing** - k6 performance testing (load, spike, stress tests)
- ✅ **Benchmarking** - BenchmarkDotNet for performance analysis
- ✅ **SonarQube Integration** - Code quality, security, and technical debt analysis

### Containerization & Orchestration
- ✅ **Docker** - Multi-stage Dockerfile for optimized images
- ✅ **Docker Compose** - Complete infrastructure stack
- ✅ **Kubernetes** - Production-ready K8s manifests with:
  - Deployments with rolling updates
  - StatefulSets for databases
  - ConfigMaps & Secrets management
  - Horizontal Pod Autoscaler (HPA)
  - Health checks (liveness, readiness, startup)
  - Ingress with TLS/SSL
  - Network policies
  - Persistent volume claims

### Observability & Monitoring
- ✅ **OpenTelemetry** - Distributed tracing, metrics, and logs
- ✅ **Prometheus** - Metrics collection and alerting
- ✅ **Grafana** - Real-time dashboards and visualization
- ✅ **Jaeger** - Distributed tracing UI
- ✅ **Serilog** - Structured logging with multiple sinks:
  - Console
  - File (rolling)
  - Seq (structured log server)
  - Elasticsearch
- ✅ **Health Checks** - Comprehensive monitoring:
  - Database connectivity
  - Redis cache
  - RabbitMQ messaging
  - Disk space
  - Custom API health

### Security & Performance
- ✅ **Rate Limiting** - Advanced rate limiting with multiple strategies:
  - Fixed Window
  - Sliding Window
  - Token Bucket
  - Concurrency Limiter
  - Global rate limiter per user/IP
- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **CORS** - Configurable cross-origin resource sharing
- ✅ **HTTPS** - TLS/SSL support

### Caching & Messaging
- ✅ **Redis** - Distributed caching for performance
- ✅ **RabbitMQ** - Message queue for async operations and event-driven architecture
- ✅ **Hangfire** - Background job processing:
  - Fire-and-forget jobs
  - Delayed jobs
  - Recurring jobs (cron expressions)
  - Dashboard UI for job monitoring

### CI/CD & DevOps
- ✅ **GitHub Actions** - Automated CI/CD pipeline:
  - Build & test
  - Code coverage
  - SonarQube analysis
  - Docker image build & push
  - Load testing
- ✅ **Makefile** - Build automation (Linux/Mac)
- ✅ **PowerShell Scripts** - Build automation (Windows)

### Documentation
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Comprehensive README** - Setup and usage guides
- ✅ **Architecture Documentation** - HATEOAS, Rate Limiting guides
- ✅ **Kubernetes Documentation** - Deployment and operations guide

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
- **SQL Server** (port 1433) - Primary database
- **Redis** (port 6379) - Distributed cache
- **RabbitMQ** (ports 5672, 15672) - Message broker
- **Prometheus** (port 9090) - Metrics collection
- **Grafana** (port 3000) - Visualization dashboards
- **Jaeger** (port 16686) - Distributed tracing
- **OpenTelemetry Collector** (port 4317) - Telemetry aggregation
- **SonarQube** (port 9000) - Code quality platform
- **Seq** (port 5341) - Structured log server

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
| **Hangfire Dashboard** | http://localhost:5000/hangfire | - |
| **Grafana** | http://localhost:3000 | admin/admin |
| **Prometheus** | http://localhost:9090 | - |
| **Jaeger UI** | http://localhost:16686 | - |
| **Seq** | http://localhost:5341 | - |
| **RabbitMQ Management** | http://localhost:15672 | guest/guest |
| **SonarQube** | http://localhost:9000 | admin/admin |

## 📊 Technology Stack

### Backend
- **.NET 8** - Latest LTS version
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM
- **MediatR** - CQRS implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Validation library

### Databases & Caching
- **SQL Server 2022** - Relational database
- **Redis** - In-memory cache
- **Entity Framework Core** - Code-first migrations

### Messaging & Background Jobs
- **RabbitMQ** - Message broker
- **MassTransit** - Distributed application framework
- **Hangfire** - Background job processing

### Observability
- **OpenTelemetry** - Telemetry framework
- **Prometheus** - Metrics & monitoring
- **Grafana** - Visualization
- **Jaeger** - Distributed tracing
- **Serilog** - Structured logging
- **Seq** - Log aggregation

### Testing
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **Coverlet** - Code coverage
- **k6** - Load testing
- **BenchmarkDotNet** - Performance benchmarking

### DevOps & CI/CD
- **Docker** - Containerization
- **Kubernetes** - Container orchestration
- **GitHub Actions** - CI/CD pipeline
- **SonarQube** - Code quality

### Security & Performance
- **JWT** - Authentication
- **AspNetCoreRateLimit** - Rate limiting
- **CORS** - Cross-origin resource sharing

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

## 👨‍💻 Creator & Maintainer

**Ali Can Yücel** tarafından yapılmıştır.

### Contact & Links
- 👨‍💻 GitHub: [@alicanyucel](https://github.com/alicanyucel)
- 📧 Email: [alicanyucel@example.com](mailto:alicanyucel@example.com)
- 💼 LinkedIn: [Ali Can Yücel](https://linkedin.com/in/alicanyucel)
- 🌐 Website: [alicanyucel.dev](https://alicanyucel.dev)

## 🌟 Acknowledgments

- **Clean Architecture** by Robert C. Martin (Uncle Bob)
- **Domain-Driven Design** by Eric Evans
- **Microservices Patterns** by Chris Richardson
- **.NET Community** - For excellent documentation and support
- **Open Source Community** - For all the amazing tools and libraries

## 🎯 Project Goals

This project demonstrates **enterprise-level software engineering practices**:
- Production-ready architecture
- Comprehensive testing strategy
- DevOps automation
- Security best practices
- Performance optimization
- Observability & monitoring
- Documentation standards

## 📈 Project Stats

- **Lines of Code**: 10,000+
- **Test Coverage**: 80%+
- **Docker Images**: Multi-stage optimized
- **K8s Resources**: 15+ manifests
- **Background Jobs**: 3 recurring jobs
- **API Endpoints**: 20+
- **Health Checks**: 6 comprehensive checks

## 🚀 Future Enhancements

- [ ] GraphQL API
- [ ] gRPC Services
- [ ] Event Sourcing
- [ ] CQRS with separate read/write databases
- [ ] API Gateway (Ocelot/YARP)
- [ ] Service Mesh (Istio/Linkerd)
- [ ] Advanced caching strategies
- [ ] Multi-tenancy support
- [ ] Internationalization (i18n)
- [ ] Real-time notifications (SignalR)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Contribution Guidelines
- Follow clean code principles
- Write comprehensive tests
- Update documentation
- Follow existing code style
- Add meaningful commit messages

## ⭐ Show Your Support

If you find this project helpful, please consider giving it a ⭐ star on GitHub!

---

<div align="center">

**🎉 This is a production-ready, enterprise-grade application with all modern DevOps and software engineering practices implemented. 🎉**

**Made with ❤️ by [Ali Can Yücel](https://github.com/alicanyucel)**

**© 2024 Ali Can Yücel. All Rights Reserved.**

</div>
