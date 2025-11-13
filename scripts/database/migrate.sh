#!/bin/bash

# Database Migration Script
# Author: HNJM

set -e

GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Database Migration Script              ${NC}"
echo -e "${BLUE}============================================${NC}"

cd src/Infrastructure/TodoBlazorApp.Infrastructure

echo -e "${BLUE}📋 تطبيق Migrations...${NC}"
dotnet ef database update \
    --startup-project ../../Presentation/TodoBlazorApp.API \
    --verbose

echo -e "${GREEN}✅ تم تطبيق Migrations بنجاح${NC}"