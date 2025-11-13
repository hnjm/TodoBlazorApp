#!/bin/bash

# Build and Deploy Script for TodoBlazorApp
# Author: HNJM
# Date: 2025-11-13

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_NAME="TodoBlazorApp"
BUILD_CONFIG="Release"
VERSION="1.0.$(date +%Y%m%d%H%M%S)"
DEPLOY_PATH="/var/www/todoapp"

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Todo App - Build and Deploy Script     ${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# Function to print colored messages
print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

# Check if running as root for deployment
check_permissions() {
    if [ "$1" == "deploy" ] && [ "$EUID" -ne 0 ]; then
        print_error "يجب تشغيل النشر كمستخدم root"
        exit 1
    fi
}

# Clean previous builds
clean_build() {
    print_info "تنظيف البناء السابق..."
    
    if [ -d "publish" ]; then
        rm -rf publish
        print_success "تم حذف مجلد publish"
    fi
    
    find . -type d -name "bin" -o -name "obj" | xargs rm -rf 2>/dev/null || true
    print_success "تم تنظيف مجلدات bin و obj"
}

# Restore dependencies
restore_dependencies() {
    print_info "استعادة الحزم..."
    dotnet restore ${PROJECT_NAME}.sln
    print_success "تم استعادة الحزم بنجاح"
}

# Build solution
build_solution() {
    print_info "بناء المشروع..."
    dotnet build ${PROJECT_NAME}.sln \
        --configuration ${BUILD_CONFIG} \
        --no-restore \
        /p:Version=${VERSION}
    print_success "تم البناء بنجاح"
}

# Run tests
run_tests() {
    print_info "تشغيل الاختبارات..."
    
    if [ -d "tests" ]; then
        dotnet test ${PROJECT_NAME}.sln \
            --configuration ${BUILD_CONFIG} \
            --no-build \
            --verbosity normal
        print_success "نجحت جميع الاختبارات"
    else
        print_warning "لا توجد اختبارات"
    fi
}

# Publish API
publish_api() {
    print_info "نشر API..."
    
    dotnet publish src/Presentation/TodoBlazorApp.API/TodoBlazorApp.API.csproj \
        -c ${BUILD_CONFIG} \
        -o publish/api \
        --no-build \
        /p:Version=${VERSION}
    
    print_success "تم نشر API"
}

# Publish Blazor UI
publish_ui() {
    print_info "نشر Blazor UI..."
    
    dotnet publish src/Presentation/TodoBlazorApp.BlazorUI/TodoBlazorApp.BlazorUI.csproj \
        -c ${BUILD_CONFIG} \
        -o publish/ui \
        --no-build \
        /p:Version=${VERSION}
    
    print_success "تم نشر Blazor UI"
}

# Create deployment package
create_package() {
    print_info "إنشاء حزمة النشر..."
    
    cd publish
    tar -czf ${PROJECT_NAME}-v${VERSION}.tar.gz api/ ui/
    
    print_success "تم إنشاء الحزمة: ${PROJECT_NAME}-v${VERSION}.tar.gz"
    print_info "حجم الحزمة: $(du -h ${PROJECT_NAME}-v${VERSION}.tar.gz | cut -f1)"
    
    cd ..
}

# Deploy to server
deploy_to_server() {
    print_info "النشر على الخادم..."
    
    # Backup current deployment
    if [ -d "${DEPLOY_PATH}" ]; then
        print_info "إنشاء نسخة احتياطية..."
        BACKUP_PATH="${DEPLOY_PATH}_backup_$(date +%Y%m%d_%H%M%S)"
        cp -r ${DEPLOY_PATH} ${BACKUP_PATH}
        print_success "تم إنشاء نسخة احتياطية: ${BACKUP_PATH}"
    fi
    
    # Create deployment directory
    mkdir -p ${DEPLOY_PATH}/{api,ui}
    
    # Deploy API
    print_info "نشر API..."
    rsync -av --delete publish/api/ ${DEPLOY_PATH}/api/
    print_success "تم نشر API"
    
    # Deploy UI
    print_info "نشر UI..."
    rsync -av --delete publish/ui/ ${DEPLOY_PATH}/ui/
    print_success "تم نشر UI"
    
    # Set permissions
    chown -R www-data:www-data ${DEPLOY_PATH}
    chmod -R 755 ${DEPLOY_PATH}
    print_success "تم تعيين الصلاحيات"
}

# Restart services
restart_services() {
    print_info "إعادة تشغيل الخدمات..."
    
    if systemctl is-active --quiet todoapp-api; then
        systemctl restart todoapp-api
        print_success "تم إعادة تشغيل todoapp-api"
    fi
    
    if systemctl is-active --quiet todoapp-ui; then
        systemctl restart todoapp-ui
        print_success "تم إعادة تشغيل todoapp-ui"
    fi
}

# Health check
health_check() {
    print_info "فحص صحة التطبيق..."
    
    sleep 5
    
    # Check API
    if curl -f http://localhost:7000/api/health > /dev/null 2>&1; then
        print_success "API يعمل بشكل صحيح"
    else
        print_error "API لا يستجيب"
        return 1
    fi
    
    # Check UI
    if curl -f http://localhost:5000 > /dev/null 2>&1; then
        print_success "UI يعمل بشكل صحيح"
    else
        print_error "UI لا يستجيب"
        return 1
    fi
}

# Main execution
main() {
    case "${1:-build}" in
        build)
            print_info "بدء عملية البناء..."
            clean_build
            restore_dependencies
            build_solution
            run_tests
            publish_api
            publish_ui
            create_package
            print_success "اكتملت عملية البناء بنجاح! ✨"
            ;;
        
        deploy)
            check_permissions deploy
            print_info "بدء عملية النشر..."
            clean_build
            restore_dependencies
            build_solution
            run_tests
            publish_api
            publish_ui
            deploy_to_server
            restart_services
            health_check
            print_success "اكتمل النشر بنجاح! 🚀"
            ;;
        
        docker)
            print_info "بناء صور Docker..."
            docker-compose build
            print_success "تم بناء الصور بنجاح"
            
            print_info "تشغيل الحاويات..."
            docker-compose up -d
            print_success "تم تشغيل الحاويات بنجاح"
            
            sleep 10
            docker-compose ps
            ;;
        
        clean)
            clean_build
            print_success "تم التنظيف بنجاح"
            ;;
        
        *)
            echo "Usage: $0 {build|deploy|docker|clean}"
            echo ""
            echo "Commands:"
            echo "  build   - بناء المشروع فقط"
            echo "  deploy  - بناء ونشر المشروع"
            echo "  docker  - بناء وتشغيل Docker containers"
            echo "  clean   - تنظيف ملفات البناء"
            exit 1
            ;;
    esac
}

# Run main function
main "$@"