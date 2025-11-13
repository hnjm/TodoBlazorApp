#!/bin/bash

# Database Backup Script
# Author: HNJM
# Date: 2025-11-13

set -e

GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Configuration
DB_PATH="todoapp.db"
BACKUP_DIR="backups"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="${BACKUP_DIR}/todoapp_backup_${TIMESTAMP}.db"

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Database Backup Script                  ${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# Create backup directory if not exists
mkdir -p ${BACKUP_DIR}

if [ ! -f "${DB_PATH}" ]; then
    echo -e "${YELLOW}⚠️  قاعدة البيانات غير موجودة: ${DB_PATH}${NC}"
    exit 1
fi

echo -e "${BLUE}📦 إنشاء نسخة احتياطية...${NC}"
cp ${DB_PATH} ${BACKUP_FILE}

echo -e "${GREEN}✅ تم إنشاء النسخة الاحتياطية بنجاح${NC}"
echo -e "${BLUE}📍 الموقع: ${BACKUP_FILE}${NC}"
echo -e "${BLUE}📊 الحجم: $(du -h ${BACKUP_FILE} | cut -f1)${NC}"

# Keep only last 30 backups
echo ""
echo -e "${BLUE}🧹 تنظيف النسخ القديمة...${NC}"
ls -t ${BACKUP_DIR}/todoapp_backup_*.db | tail -n +31 | xargs -r rm
echo -e "${GREEN}✅ تم الاحتفاظ بآخر 30 نسخة احتياطية${NC}"

# Compress old backups (older than 7 days)
find ${BACKUP_DIR} -name "todoapp_backup_*.db" -mtime +7 -exec gzip {} \;
echo -e "${GREEN}✅ تم ضغط النسخ الأقدم من 7 أيام${NC}"