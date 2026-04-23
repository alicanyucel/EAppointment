# EAppointment Kubernetes Deployment Script for Windows

Write-Host "🚀 Deploying EAppointment to Kubernetes..." -ForegroundColor Green

# Create namespace
Write-Host "Creating namespace..." -ForegroundColor Yellow
kubectl apply -f k8s/namespace.yaml

# Apply ConfigMaps and Secrets
Write-Host "Applying ConfigMaps and Secrets..." -ForegroundColor Yellow
kubectl apply -f k8s/configmap.yaml

# Apply PVCs
Write-Host "Creating Persistent Volume Claims..." -ForegroundColor Yellow
kubectl apply -f k8s/pvc.yaml

# Deploy StatefulSets (Database, Redis, RabbitMQ)
Write-Host "Deploying StatefulSets..." -ForegroundColor Yellow
kubectl apply -f k8s/statefulsets.yaml

# Wait for databases to be ready
Write-Host "Waiting for databases to be ready..." -ForegroundColor Yellow
kubectl wait --for=condition=ready pod -l app=sqlserver -n eappointment --timeout=300s
kubectl wait --for=condition=ready pod -l app=redis -n eappointment --timeout=120s
kubectl wait --for=condition=ready pod -l app=rabbitmq -n eappointment --timeout=120s

# Deploy monitoring stack
Write-Host "Deploying monitoring stack..." -ForegroundColor Yellow
kubectl apply -f k8s/monitoring.yaml

# Deploy main application
Write-Host "Deploying EAppointment API..." -ForegroundColor Yellow
kubectl apply -f k8s/deployment.yaml

# Apply HPA
Write-Host "Applying Horizontal Pod Autoscaler..." -ForegroundColor Yellow
kubectl apply -f k8s/hpa.yaml

# Apply Ingress
Write-Host "Configuring Ingress..." -ForegroundColor Yellow
kubectl apply -f k8s/ingress.yaml

# Wait for deployment
Write-Host "Waiting for deployment to be ready..." -ForegroundColor Yellow
kubectl wait --for=condition=available deployment/eappointment-api -n eappointment --timeout=300s

# Get service information
Write-Host "✅ Deployment complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Service Information:" -ForegroundColor Green
kubectl get services -n eappointment

Write-Host ""
Write-Host "Pod Status:" -ForegroundColor Green
kubectl get pods -n eappointment

Write-Host ""
Write-Host "Ingress Information:" -ForegroundColor Green
kubectl get ingress -n eappointment

Write-Host ""
Write-Host "🎉 EAppointment successfully deployed to Kubernetes!" -ForegroundColor Green
