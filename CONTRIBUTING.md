# دليل المساهمة

شكراً لاهتمامك بالمساهمة في مشروع Todo Application! 🎉

## 📋 جدول المحتويات

- [قواعد السلوك](#قواعد-السلوك)
- [كيفية المساهمة](#كيفية-المساهمة)
- [معايير الكود](#معايير-الكود)
- [عملية المراجعة](#عملية-المراجعة)

## 🤝 قواعد السلوك

نحن ملتزمون بتوفير بيئة ترحيبية وشاملة. يرجى:

- استخدام لغة ترحيبية وشاملة
- احترام وجهات النظر والخبرات المختلفة
- قبول النقد البنّاء بلطف
- التركيز على ما هو أفضل للمجتمع

## 🚀 كيفية المساهمة

### الإبلاغ عن الأخطاء

إذا وجدت خطأ:

1. تحقق من أن الخطأ لم يتم الإبلاغ عنه من قبل
2. افتح Issue جديد مع:
   - وصف واضح للمشكلة
   - خطوات إعادة إنتاج الخطأ
   - السلوك المتوقع والفعلي
   - لقطات الشاشة (إن وجدت)
   - معلومات البيئة (نظام التشغيل، إصدار .NET، إلخ)

### اقتراح ميزات جديدة

1. افتح Issue بعنوان يبدأ بـ `[Feature Request]`
2. صف الميزة المقترحة
3. اشرح لماذا هذه الميزة مفيدة
4. قدم أمثلة على الاستخدام

### Pull Requests

1. Fork المشروع
2. إنشاء فرع جديد:
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. قم بإجراء التغييرات
4. اكتب أو حدّث الاختبارات
5. تأكد من نجاح جميع الاختبارات:
   ```bash
   dotnet test
   ```
6. Commit التغييرات:
   ```bash
   git commit -m "Add amazing feature"
   ```
7. Push إلى الفرع:
   ```bash
   git push origin feature/amazing-feature
   ```
8. افتح Pull Request

## 📝 معايير الكود

### C# Coding Standards

- اتبع [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- استخدم أسماء واضحة ووصفية
- أضف تعليقات للكود المعقد
- اتبع مبادئ SOLID

### نمط الكود

```csharp
// ✅ جيد
public class TodoService : ITodoService
{
    private readonly IRepository _repository;
    
    public TodoService(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<TodoItem> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID must be positive", nameof(id));
            
        return await _repository.GetByIdAsync(id);
    }
}

// ❌ سيء
public class todoservice
{
    public TodoItem get(int x)
    {
        return repo.get(x);
    }
}
```

### الالتزام بـ Clean Architecture

- ضع الكيانات في Domain Layer
- منطق الأعمال في Application Layer
- تفاصيل التنفيذ في Infrastructure Layer
- واجهة المستخدم في Presentation Layer

### الاختبارات

- اكتب اختبارات لكل ميزة جديدة
- حافظ على تغطية كود لا تقل عن 80%
- استخدم أسماء اختبار وصفية:

```csharp
[Fact]
public async Task CreateTodo_WithValidData_ShouldReturnCreatedTodo()
{
    // Arrange
    var createDto = new CreateTodoItemDto { Title = "Test" };
    
    // Act
    var result = await _service.CreateAsync(createDto);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test", result.Title);
}
```

## 🔍 عملية المراجعة

1. سيقوم أحد المشرفين بمراجعة PR الخاص بك
2. قد يُطلب منك إجراء تعديلات
3. بعد الموافقة، سيتم دمج PR

## 📧 التواصل

لأي أسئلة، يرجى:
- فتح Issue
- المراسلة على: hnjm@example.com

شكراً لمساهمتك! 🙏