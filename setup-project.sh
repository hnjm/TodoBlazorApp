#!/bin/bash

# Complete Project Setup Script
# Author: HNJM
# Date: 2025-11-13

set -e

GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Todo App - Project Setup               ${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# Function to print messages
print_step() {
    echo -e "${BLUE}▶ $1${NC}"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

# Create directory structure
print_step "إنشاء هيكل المجلدات..."

mkdir -p src/Core/TodoBlazorApp.Domain
mkdir -p src/Core/TodoBlazorApp.Application
mkdir -p src/Infrastructure/TodoBlazorApp.Infrastructure
mkdir -p src/Presentation/TodoBlazorApp.API
mkdir -p src/Presentation/TodoBlazorApp.BlazorUI
mkdir -p tests/TodoBlazorApp.UnitTests
mkdir -p tests/TodoBlazorApp.IntegrationTests
mkdir -p scripts/{deploy,database,monitoring}
mkdir -p docker
mkdir -p docs/images
mkdir -p .github/workflows

print_success "تم إنشاء هيكل المجلدات"

# Create solution
print_step "إنشاء Solution..."
dotnet new sln -n TodoBlazorApp
print_success "تم إنشاء Solution"

# Create projects
print_step "إنشاء المشاريع..."

cd src/Core/TodoBlazorApp.Domain
dotnet new classlib
cd ../../..

cd src/Core/TodoBlazorApp.Application
dotnet new classlib
cd ../../..

cd src/Infrastructure/TodoBlazorApp.Infrastructure
dotnet new classlib
cd ../../..

cd src/Presentation/TodoBlazorApp.API
dotnet new webapi
cd ../../..

cd src/Presentation/TodoBlazorApp.BlazorUI
dotnet new blazorserver
cd ../../..

print_success "تم إنشاء جميع المشاريع"

# Add projects to solution
print_step "إضافة المشاريع إلى Solution..."

dotnet sln add src/Core/TodoBlazorApp.Domain/TodoBlazorApp.Domain.csproj
dotnet sln add src/Core/TodoBlazorApp.Application/TodoBlazorApp.Application.csproj
dotnet sln add src/Infrastructure/TodoBlazorApp.Infrastructure/TodoBlazorApp.Infrastructure.csproj
dotnet sln add src/Presentation/TodoBlazorApp.API/TodoBlazorApp.API.csproj
dotnet sln add src/Presentation/TodoBlazorApp.BlazorUI/TodoBlazorApp.BlazorUI.csproj

print_success "تم إضافة جميع المشاريع"

# Add project references
print_step "إضافة المراجع بين المشاريع..."

dotnet add src/Core/TodoBlazorApp.Application reference src/Core/TodoBlazorApp.Domain
dotnet add src/Infrastructure/TodoBlazorApp.Infrastructure reference src/Core/TodoBlazorApp.Domain
dotnet add src/Presentation/TodoBlazorApp.API reference src/Core/TodoBlazorApp.Application
dotnet add src/Presentation/TodoBlazorApp.API reference src/Infrastructure/TodoBlazorApp.Infrastructure
dotnet add src/Presentation/TodoBlazorApp.BlazorUI reference src/Core/TodoBlazorApp.Application

print_success "تم إضافة جميع المراجع"

# Install packages
print_step "تثبيت الحزم المطلوبة..."

# Application Layer
dotnet add src/Core/TodoBlazorApp.Application package MediatR
dotnet add src/Core/TodoBlazorApp.Application package AutoMapper
dotnet add src/Core/TodoBlazorApp.Application package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Core/TodoBlazorApp.Application package FluentValidation
dotnet add src/Core/TodoBlazorApp.Application package FluentValidation.DependencyInjectionExtensions

# Infrastructure Layer
dotnet add src/Infrastructure/TodoBlazorApp.Infrastructure package Microsoft.EntityFrameworkCore
dotnet add src/Infrastructure/TodoBlazorApp.Infrastructure package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Infrastructure/TodoBlazorApp.Infrastructure package Microsoft.EntityFrameworkCore.Design

# API
dotnet add src/Presentation/TodoBlazorApp.API package Swashbuckle.AspNetCore
dotnet add src/Presentation/TodoBlazorApp.API package Serilog.AspNetCore
dotnet add src/Presentation/TodoBlazorApp.API package Serilog.Sinks.File

print_success "تم تثبيت جميع الحزم"

# Initialize Git
print_step "تهيئة Git..."
git init
git add .
git commit -m "Initial commit: Project structure created"
print_success "تم تهيئة Git"

echo ""
echo -e "${GREEN}============================================${NC}"
echo -e "${GREEN}   ✅ اكتمل إعداد المشروع بنجاح!          ${NC}"
echo -e "${GREEN}============================================${NC}"
echo ""
echo -e "${BLUE}الخطوات التالية:${NC}"
echo "1. نسخ الملفات المصدرية إلى المجلدات المناسبة"
echo "2. تشغيل: dotnet build"
echo "3. تشغيل: dotnet run --project src/Presentation/TodoBlazorApp.API"
echo "4. تشغيل: dotnet run --project src/Presentation/TodoBlazorApp.BlazorUI"
echo ""
echo -e "${BLUE}🚀 ابدأ الآن!${NC}"