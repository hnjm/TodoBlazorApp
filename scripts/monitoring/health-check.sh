#!/bin/bash

# Health Check and Monitoring Script
# Author: HNJM
# Date: 2025-11-13

set -e

GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

API_URL="http://localhost:7000"
UI_URL="http://localhost:5000"
HEALTH_ENDPOINT="/api/health"

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Todo App - Health Check                ${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# Function to check service
check_service() {
    local name=$1
    local url=$2
    local endpoint=$3
    
    echo -e "${BLUE}🔍 فحص ${name}...${NC}"
    
    if curl -f -s "${url}${endpoint}" > /dev/null 2>&1; then
        echo -e "${GREEN}✅ ${name} يعمل بشكل صحيح${NC}"
        
        # Get detailed health info
        local response=$(curl -s "${url}${endpoint}")
        echo -e "${BLUE}   الاستجابة: ${response}${NC}"
        return 0
    else
        echo -e "${RED}❌ ${name} لا يستجيب${NC}"
        return 1
    fi
}

# Check API
check_service "API" "${API_URL}" "${HEALTH_ENDPOINT}"
API_STATUS=$?

echo ""

# Check UI
check_service "UI" "${UI_URL}" "/"
UI_STATUS=$?

echo ""
echo -e "${BLUE}============================================${NC}"

# Overall status
if [ $API_STATUS -eq 0 ] && [ $UI_STATUS -eq 0 ]; then
    echo -e "${GREEN}✅ جميع الخدمات تعمل بشكل صحيح${NC}"
    exit 0
else
    echo -e "${RED}❌ بعض الخدمات لا تعمل${NC}"
    exit 1
fi