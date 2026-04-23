#!/bin/bash

# EAppointment Kubernetes Deployment Script

set -e

echo "🚀 Deploying EAppointment to Kubernetes..."

# Color codes
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Create namespace
echo -e "${YELLOW}Creating namespace...${NC}"
kubectl apply -f k8s/namespace.yaml

# Apply ConfigMaps and Secrets
echo -e "${YELLOW}Applying ConfigMaps and Secrets...${NC}"
kubectl apply -f k8s/configmap.yaml

# Apply PVCs
echo -e "${YELLOW}Creating Persistent Volume Claims...${NC}"
kubectl apply -f k8s/pvc.yaml

# Deploy StatefulSets (Database, Redis, RabbitMQ)
echo -e "${YELLOW}Deploying StatefulSets...${NC}"
kubectl apply -f k8s/statefulsets.yaml

# Wait for databases to be ready
echo -e "${YELLOW}Waiting for databases to be ready...${NC}"
kubectl wait --for=condition=ready pod -l app=sqlserver -n eappointment --timeout=300s
kubectl wait --for=condition=ready pod -l app=redis -n eappointment --timeout=120s
kubectl wait --for=condition=ready pod -l app=rabbitmq -n eappointment --timeout=120s

# Deploy monitoring stack
echo -e "${YELLOW}Deploying monitoring stack...${NC}"
kubectl apply -f k8s/monitoring.yaml

# Deploy main application
echo -e "${YELLOW}Deploying EAppointment API...${NC}"
kubectl apply -f k8s/deployment.yaml

# Apply HPA
echo -e "${YELLOW}Applying Horizontal Pod Autoscaler...${NC}"
kubectl apply -f k8s/hpa.yaml

# Apply Ingress
echo -e "${YELLOW}Configuring Ingress...${NC}"
kubectl apply -f k8s/ingress.yaml

# Wait for deployment
echo -e "${YELLOW}Waiting for deployment to be ready...${NC}"
kubectl wait --for=condition=available deployment/eappointment-api -n eappointment --timeout=300s

# Get service information
echo -e "${GREEN}✅ Deployment complete!${NC}"
echo ""
echo -e "${GREEN}Service Information:${NC}"
kubectl get services -n eappointment

echo ""
echo -e "${GREEN}Pod Status:${NC}"
kubectl get pods -n eappointment

echo ""
echo -e "${GREEN}Ingress Information:${NC}"
kubectl get ingress -n eappointment

echo ""
echo -e "${YELLOW}Access URLs:${NC}"
echo "API: http://$(kubectl get ingress eappointment-ingress -n eappointment -o jsonpath='{.spec.rules[0].host}')"
echo "Grafana: http://$(kubectl get service grafana-service -n eappointment -o jsonpath='{.status.loadBalancer.ingress[0].ip}'):3000"
echo "Prometheus: http://$(kubectl get service prometheus-service -n eappointment -o jsonpath='{.spec.clusterIP}'):9090"

echo ""
echo -e "${GREEN}🎉 EAppointment successfully deployed to Kubernetes!${NC}"
