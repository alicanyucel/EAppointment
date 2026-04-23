# Kubernetes Deployment Guide

## Prerequisites

- Kubernetes cluster (minikube, Docker Desktop, or cloud provider)
- kubectl installed and configured
- Docker for building images

## Quick Start

### 1. Build Docker Image

```bash
docker build -t eappointment-api:latest .
```

### 2. Deploy to Kubernetes

**Linux/Mac:**
```bash
chmod +x k8s/deploy.sh
./k8s/deploy.sh
```

**Windows:**
```powershell
.\k8s\deploy.ps1
```

### 3. Manual Deployment Steps

```bash
# Create namespace
kubectl apply -f k8s/namespace.yaml

# Apply configurations
kubectl apply -f k8s/configmap.yaml

# Create persistent volumes
kubectl apply -f k8s/pvc.yaml

# Deploy databases
kubectl apply -f k8s/statefulsets.yaml

# Deploy monitoring
kubectl apply -f k8s/monitoring.yaml

# Deploy application
kubectl apply -f k8s/deployment.yaml

# Apply autoscaling
kubectl apply -f k8s/hpa.yaml

# Configure ingress
kubectl apply -f k8s/ingress.yaml
```

## Architecture Components

### Application Layer
- **eappointment-api**: Main API deployment with 3 replicas
- **HPA**: Auto-scaling from 3 to 10 pods based on CPU/Memory
- **LoadBalancer**: External access to the API

### Data Layer
- **SQL Server**: StatefulSet with persistent storage
- **Redis**: In-memory cache
- **RabbitMQ**: Message queue for async operations

### Monitoring Stack
- **Prometheus**: Metrics collection
- **Grafana**: Visualization dashboards
- **Jaeger**: Distributed tracing
- **OpenTelemetry Collector**: Telemetry aggregation

## Resource Limits

### API Pods
- Requests: 256Mi RAM, 250m CPU
- Limits: 512Mi RAM, 500m CPU

### SQL Server
- Requests: 2Gi RAM, 1000m CPU
- Limits: 4Gi RAM, 2000m CPU

### Redis
- Requests: 128Mi RAM, 100m CPU
- Limits: 256Mi RAM, 200m CPU

## Scaling

### Manual Scaling
```bash
kubectl scale deployment eappointment-api -n eappointment --replicas=5
```

### Auto-scaling (HPA)
The HPA automatically scales pods based on:
- CPU utilization (target: 70%)
- Memory utilization (target: 80%)
- Min replicas: 3
- Max replicas: 10

## Monitoring

### Access Grafana
```bash
kubectl port-forward -n eappointment svc/grafana-service 3000:3000
```
Visit: http://localhost:3000 (admin/admin)

### Access Prometheus
```bash
kubectl port-forward -n eappointment svc/prometheus-service 9090:9090
```
Visit: http://localhost:9090

### Access Jaeger
```bash
kubectl port-forward -n eappointment svc/jaeger-service 16686:16686
```
Visit: http://localhost:16686

## Health Checks

### Liveness Probe
- Path: `/health`
- Initial delay: 30s
- Period: 10s

### Readiness Probe
- Path: `/health`
- Initial delay: 15s
- Period: 5s

### Startup Probe
- Path: `/health`
- Failure threshold: 30
- Period: 10s

## Secrets Management

Update secrets:
```bash
kubectl edit secret eappointment-secrets -n eappointment
```

Important secrets:
- SQL_CONNECTION_STRING
- JWT_SECRET_KEY
- JWT_ISSUER
- JWT_AUDIENCE

## Troubleshooting

### Check pod status
```bash
kubectl get pods -n eappointment
```

### View logs
```bash
kubectl logs -f deployment/eappointment-api -n eappointment
```

### Describe pod
```bash
kubectl describe pod <pod-name> -n eappointment
```

### Execute into pod
```bash
kubectl exec -it <pod-name> -n eappointment -- /bin/bash
```

### Check events
```bash
kubectl get events -n eappointment --sort-by='.lastTimestamp'
```

## Network Policies

The deployment includes network policies to:
- Restrict ingress traffic to ports 80/443
- Allow egress to database (1433), Redis (6379), RabbitMQ (5672)
- Enable DNS resolution

## Ingress

### Configure DNS
Point your domain to the LoadBalancer IP:
```bash
kubectl get ingress -n eappointment
```

### TLS/SSL
The ingress is configured for cert-manager with Let's Encrypt.
Install cert-manager first:
```bash
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.0/cert-manager.yaml
```

## Persistence

### SQL Server
- Storage: 10Gi
- Access Mode: ReadWriteOnce
- Backup recommended

### Prometheus
- Storage: 10Gi
- Retention: Default (15 days)

### Grafana
- Storage: 5Gi
- Contains dashboards and settings

## Updates & Rollouts

### Update deployment
```bash
kubectl set image deployment/eappointment-api eappointment-api=eappointment-api:v2 -n eappointment
```

### Rollback
```bash
kubectl rollout undo deployment/eappointment-api -n eappointment
```

### Check rollout status
```bash
kubectl rollout status deployment/eappointment-api -n eappointment
```

## Cleanup

Remove all resources:
```bash
kubectl delete namespace eappointment
```

Or individual components:
```bash
kubectl delete -f k8s/deployment.yaml
kubectl delete -f k8s/statefulsets.yaml
kubectl delete -f k8s/monitoring.yaml
```

## Production Considerations

1. **Security**
   - Use proper secrets management (e.g., HashiCorp Vault, Azure Key Vault)
   - Enable RBAC
   - Use network policies
   - Enable Pod Security Policies

2. **High Availability**
   - Deploy across multiple availability zones
   - Use pod anti-affinity rules
   - Implement proper backup strategies

3. **Monitoring**
   - Set up alerts in Prometheus
   - Configure log aggregation (e.g., ELK, Loki)
   - Enable audit logging

4. **Performance**
   - Tune resource limits based on load testing
   - Configure persistent volume performance classes
   - Use node affinity for database pods

5. **Cost Optimization**
   - Use cluster autoscaler
   - Implement pod disruption budgets
   - Monitor resource utilization

## Support

For issues and questions:
- GitHub Issues: https://github.com/alicanyucel/EAppointment/issues
- Documentation: See main README.md
