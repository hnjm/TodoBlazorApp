#!/bin/bash

# Add New Migration Script
# Author: HNJM
# Date: 2025-11-13

set -e

GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m'

if [ -z "$1" ]; then
    echo -e "${RED}❌ يرجى تحديد اسم Migration${NC}"
    echo "Usage: $0 <MigrationName>"
    exit 1
fi

MIGRATION_NAME=$1

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   إنشاء Migration جديد                    ${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

cd src/Infrastructure/TodoBlazorApp.Infrastructure

echo -e "${BLUE}📝 إنشاء Migration: ${MIGRATION_NAME}...${NC}"
dotnet ef migrations add ${MIGRATION_NAME} \
    --startup-project ../../Presentation/TodoBlazorApp.API \
    --output-dir Data/Migrations \
    --verbose

echo -e "${GREEN}✅ تم إنشاء Migration بنجاح${NC}"
echo ""
echo -e "${BLUE}لتطبيق Migration استخدم:${NC}"
echo -e "${GREEN}./scripts/database/migrate.sh${NC}"