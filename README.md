# 📋 نظام إدارة المهام - Todo Application

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57?logo=sqlite)
![License](https://img.shields.io/badge/license-MIT-green)
![Build](https://img.shields.io/badge/build-passing-brightgreen)

تطبيق ويب احترافي لإدارة المهام مبني باستخدام **Clean Architecture** و **CQRS Pattern**

[التثبيت](#-التثبيت-والتشغيل) • [الميزات](#-الميزات) • [المعمارية](#-المعمارية) • [CI/CD](#-cicd) • [الوثائق](#-الوثائق)

</div>

---

## 📸 لقطات الشاشة

<div align="center">

### الصفحة الرئيسية
![Homepage](docs/images/homepage.png)

### إدارة المهام
![Todo Management](docs/images/todos.png)

### الإحصائيات
![Statistics](docs/images/stats.png)

</div>

## 🌟 الميزات

### ✨ الميزات الرئيسية
- ✅ **إدارة كاملة للمهام** - إنشاء، تعديل، حذف، وتتبع المهام
- 🏷️ **التصنيف والتنظيم** - تصنيف المهام حسب الفئات والوسوم
- ⚡ **نظام الأولويات** - 4 مستويات للأولوية (منخفضة، متوسطة، عالية، عاجلة)
- 💬 **نظام التعليقات** - إضافة تعليقات على المهام
- 📎 **المرفقات** - إرفاق ملفات بالمهام
- 📊 **إحصائيات تفصيلية** - تقارير وتحليلات شاملة
- 🔍 **بحث وفلترة متقدمة** - بحث في العنوان والوصف والتصنيفات
- ⏰ **تتبع المهام المتأخرة** - تنبيهات للمهام المتجاوزة لموعدها
- 🌐 **واجهة عربية كاملة** - دعم كامل للغة العربية (RTL)
- 📱 **تصميم متجاوب** - يعمل على جميع الأجهزة

### 🎨 الواجهة
- 🎯 **تصميم عصري** - واجهة نظيفة وسهلة الاستخدام
- 🌓 **دعم الوضع الداكن** - راحة للعينين
- ⚡ **أداء عالي** - تحميل سريع وتفاعل فوري
- 📊 **رسوم بيانية** - تصوير بصري للبيانات

### 🔧 التقنيات المستخدمة

#### Backend
- **ASP.NET Core 8.0** - إطار العمل الرئيسي
- **Entity Framework Core 8.0** - ORM للتعامل مع قاعدة البيانات
- **SQLite** - قاعدة بيانات خفيفة وسريعة
- **MediatR** - تطبيق CQRS Pattern
- **AutoMapper** - تحويل بين الكائنات
- **FluentValidation** - التحقق من صحة البيانات
- **Serilog** - تسجيل الأحداث

#### Frontend
- **Blazor Server** - واجهة تفاعلية
- **Bootstrap 5** - إطار عمل CSS
- **Bootstrap Icons** - مكتبة الأيقونات

#### DevOps & CI/CD
- **Jenkins** - أتمتة البناء والنشر
- **GitHub Actions** - CI/CD على GitHub
- **Azure DevOps** - خطوط أنابيب Azure
- **Docker** - الحاويات
- **Docker Compose** - إدارة الحاويات المتعددة

## 🏗️ المعمارية

المشروع يتبع **Clean Architecture** مع فصل واضح للطبقات:

```
TodoBlazorApp/
├── src/
│   ├── Core/
│   │   ├── TodoBlazorApp.Domain/          # الكيانات والقواعد التجارية
│   │   └── TodoBlazorApp.Application/     # منطق الأعمال، CQRS، DTOs
│   ├── Infrastructure/
│   │   └── TodoBlazorApp.Infrastructure/  # EF Core، Repositories، Services
│   └── Presentation/
│       ├── TodoBlazorApp.API/             # RESTful API
│       └── TodoBlazorApp.BlazorUI/        # Blazor Server UI
├── tests/                                  # الاختبارات
├── scripts/                                # سكريبتات البناء والنشر
└── docker/                                 # ملفات Docker
```

### 🔄 CQRS Pattern

```
Command → Handler → Repository → Database
Query → Handler → Repository → Database
```

### 📦 Layers

1. **Domain Layer** - الطبقة الأساسية
   - Entities
   - Enums
   - Interfaces
   - Domain Events

2. **Application Layer** - منطق الأعمال
   - Commands & Queries (MediatR)
   - DTOs
   - Validators (FluentValidation)
   - Mappings (AutoMapper)

3. **Infrastructure Layer** - البنية التحتية
   - DbContext (EF Core)
   - Repositories
   - Services
   - Migrations

4. **Presentation Layer** - العرض
   - API Controllers
   - Blazor Components
   - Services

## 🚀 التثبيت والتشغيل

### المتطلبات الأساسية

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- محرر كود (Visual Studio 2022 أو VS Code)
- (اختياري) [Docker Desktop](https://www.docker.com/products/docker-desktop)

### خطوات التثبيت

#### 1. استنساخ المشروع

```bash
git clone https://github.com/hnjm/TodoBlazorApp.git
cd TodoBlazorApp
```

#### 2. استعادة الحزم

```bash
dotnet restore TodoBlazorApp.sln
```

#### 3. بناء المشروع

```bash
dotnet build TodoBlazorApp.sln --configuration Release
```

#### 4. تشغيل التطبيق

##### الطريقة الأولى: تشغيل يدوي

```bash
# Terminal 1 - تشغيل API
cd src/Presentation/TodoBlazorApp.API
dotnet run

# Terminal 2 - تشغيل Blazor UI
cd src/Presentation/TodoBlazorApp.BlazorUI
dotnet run
```

##### الطريقة الثانية: استخدام السكريبت

**Windows:**
```powershell
.\scripts\deploy\start-services.ps1
```

**Linux/Mac:**
```bash
chmod +x scripts/deploy/build-and-deploy.sh
./scripts/deploy/build-and-deploy.sh build
```

##### الطريقة الثالثة: استخدام Docker

```bash
# بناء وتشغيل الحاويات
docker-compose up -d

# عرض حالة الحاويات
docker-compose ps

# عرض السجلات
docker-compose logs -f
```

### 5. الوصول للتطبيق

- **API**: http://localhost:7000
- **Swagger UI**: http://localhost:7000/swagger
- **Blazor UI**: http://localhost:5000
- **Health Check**: http://localhost:7000/api/health

## 🔧 التكوين

### appsettings.json (API)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=todoapp.db;Cache=Shared"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

### appsettings.json (Blazor UI)

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7000/"
  }
}
```

## 🧪 الاختبارات

```bash
# تشغيل جميع الاختبارات
dotnet test TodoBlazorApp.sln

# مع تقرير التغطية
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 البناء والنشر

### البناء للإنتاج

```bash
# استخدام السكريبت
./scripts/deploy/build-and-deploy.sh build

# أو يدوياً
dotnet publish src/Presentation/TodoBlazorApp.API/TodoBlazorApp.API.csproj -c Release -o ./publish/api
dotnet publish src/Presentation/TodoBlazorApp.BlazorUI/TodoBlazorApp.BlazorUI.csproj -c Release -o ./publish/ui
```

### النشر على الخادم

```bash
# Linux
sudo ./scripts/deploy/build-and-deploy.sh deploy

# أو يدوياً
rsync -av publish/api/ user@server:/var/www/todoapp/api/
rsync -av publish/ui/ user@server:/var/www/todoapp/ui/
```

### استخدام Docker

```bash
# بناء الصور
docker-compose build

# نشر
docker-compose up -d

# إيقاف
docker-compose down
```

## 🔄 CI/CD

### Jenkins

```bash
# إنشاء Pipeline Job في Jenkins
# استخدم Jenkinsfile الموجود في المشروع
```

### GitHub Actions

الـ Workflows موجودة في `.github/workflows/`:
- `ci-cd.yml` - البناء والنشر الآلي
- `pr-validation.yml` - فحص Pull Requests
- `release.yml` - إنشاء Releases

### Azure DevOps

```bash
# استيراد azure-pipelines.yml إلى Azure DevOps
```

## 📊 API Documentation

### Swagger UI

الوصول إلى Swagger UI:
```
http://localhost:7000/swagger
```

### Endpoints الرئيسية

#### Todos

| Method | Endpoint | الوصف |
|--------|----------|-------|
| GET | `/api/todos` | الحصول على جميع المهام |
| GET | `/api/todos/{id}` | الحصول على مهمة محددة |
| GET | `/api/todos/stats` | الحصول على الإحصائيات |
| GET | `/api/todos/overdue` | المهام المتأخرة |
| POST | `/api/todos` | إنشاء مهمة جديدة |
| PUT | `/api/todos/{id}` | تحديث مهمة |
| DELETE | `/api/todos/{id}` | حذف مهمة |
| PATCH | `/api/todos/{id}/toggle` | تبديل حالة الإكمال |
| POST | `/api/todos/{id}/comments` | إضافة تعليق |

#### Health

| Method | Endpoint | الوصف |
|--------|----------|-------|
| GET | `/api/health` | فحص صحة التطبيق |

## 🗄️ قاعدة البيانات

### Entity Framework Migrations

```bash
# إنشاء Migration جديد
./scripts/database/add-migration.sh MigrationName

# تطبيق Migrations
./scripts/database/migrate.sh

# إنشاء نسخة احتياطية
./scripts/database/backup.sh
```

### مخطط قاعدة البيانات

```
TodoItems
├── Id (PK)
├── Title
├── Description
├── IsCompleted
├── Priority
├── Category
├── Tags
├── DueDate
├── CreatedBy
├── CreatedDate
└── Comments (FK)
    └── Attachments (FK)
```

## 🐛 استكشاف الأخطاء

### المشاكل الشائعة

**1. لا يمكن الاتصال بـ API**

```bash
# تحقق من أن API يعمل
curl http://localhost:7000/api/health

# تحقق من المنافذ
netstat -an | grep 7000
```

**2. خطأ في قاعدة البيانات**

```bash
# حذف قاعدة البيانات وإعادة إنشائها
rm todoapp.db
dotnet ef database update --project src/Presentation/TodoBlazorApp.API
```

**3. مشاكل Docker**

```bash
# إعادة بناء الحاويات
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

## 📚 الوثائق الإضافية

- [دليل المساهمة](docs/CONTRIBUTING.md)
- [سياسة الأمان](docs/SECURITY.md)
- [سجل التغييرات](CHANGELOG.md)
- [الرخصة](LICENSE)

## 🤝 المساهمة

نرحب بالمساهمات! يرجى اتباع الخطوات التالية:

1. Fork المشروع
2. إنشاء فرع جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى الفرع (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

## 📝 الترخيص

هذا المشروع مرخص تحت رخصة MIT - انظر ملف [LICENSE](LICENSE) للتفاصيل.

## 👨‍💻 المطور

**HNJM**

- GitHub: [@hnjm](https://github.com/hnjm)
- Email: hnjm@example.com

## 🙏 شكر وتقدير

- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [MediatR](https://github.com/jbogard/MediatR)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://fluentvalidation.net/)
- [Bootstrap](https://getbootstrap.com/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)

## ⭐ إذا أعجبك المشروع

إذا وجدت هذا المشروع مفيداً، يرجى إعطائه ⭐ على GitHub!

---

<div align="center">

**صُنع بـ ❤️ بواسطة HNJM**

© 2025 Todo Application. جميع الحقوق محفوظة.

</div>