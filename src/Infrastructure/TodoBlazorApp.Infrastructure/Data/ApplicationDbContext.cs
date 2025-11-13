using Microsoft.EntityFrameworkCore;
using TodoBlazorApp.Domain.Common;
using TodoBlazorApp.Domain.Entities;

namespace TodoBlazorApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<TodoComment> TodoComments => Set<TodoComment>();
    public DbSet<TodoAttachment> TodoAttachments => Set<TodoAttachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TodoItem Configuration
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(2000);

            entity.Property(e => e.Category)
                .HasMaxLength(50);

            entity.Property(e => e.Tags)
                .HasMaxLength(200);

            entity.Property(e => e.AssignedTo)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100);

            entity.Property(e => e.Priority)
                .HasConversion<int>();

            entity.HasIndex(e => e.IsCompleted);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.DueDate);
            entity.HasIndex(e => e.CreatedDate);

            // Relationships
            entity.HasMany(e => e.Comments)
                .WithOne(c => c.TodoItem)
                .HasForeignKey(c => c.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Attachments)
                .WithOne(a => a.TodoItem)
                .HasForeignKey(a => a.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TodoComment Configuration
        modelBuilder.Entity<TodoComment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(e => e.TodoItemId);
            entity.HasIndex(e => e.CreatedDate);
        });

        // TodoAttachment Configuration
        modelBuilder.Entity<TodoAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.UploadedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.TodoItemId);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService?.UserId ?? "system";
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _currentUserService?.UserId ?? "system";
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem
            {
                Id = 1,
                Title = "إعداد المشروع بـ Clean Architecture",
                Description = "إنشاء مشروع ASP.NET Core مع تطبيق معمارية Clean Architecture",
                Priority = Domain.Enums.Priority.High,
                Category = "تطوير",
                Tags = "architecture,setup",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 10, 10, 0, 0, DateTimeKind.Utc),
                IsCompleted = true,
                CompletedDate = new DateTime(2025, 11, 11, 14, 30, 0, DateTimeKind.Utc)
            },
            new TodoItem
            {
                Id = 2,
                Title = "تطبيق MediatR و CQRS Pattern",
                Description = "إضافة MediatR لتطبيق نمط CQRS في المشروع",
                Priority = Domain.Enums.Priority.High,
                Category = "تطوير",
                Tags = "cqrs,mediatr",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 11, 9, 0, 0, DateTimeKind.Utc),
                IsCompleted = true,
                CompletedDate = new DateTime(2025, 11, 12, 16, 0, 0, DateTimeKind.Utc)
            },
            new TodoItem
            {
                Id = 3,
                Title = "إنشاء API Controllers",
                Description = "بناء RESTful API مع Swagger documentation",
                Priority = Domain.Enums.Priority.Medium,
                Category = "تطوير",
                Tags = "api,rest,swagger",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 12, 11, 0, 0, DateTimeKind.Utc),
                IsCompleted = false,
                DueDate = new DateTime(2025, 11, 15, 23, 59, 59, DateTimeKind.Utc)
            },
            new TodoItem
            {
                Id = 4,
                Title = "تصميم واجهة Blazor",
                Description = "إنشاء واجهة مستخدم تفاعلية باستخدام Blazor Server",
                Priority = Domain.Enums.Priority.Medium,
                Category = "تصميم",
                Tags = "blazor,ui,frontend",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 12, 14, 0, 0, DateTimeKind.Utc),
                IsCompleted = false,
                EstimatedHours = 16,
                DueDate = new DateTime(2025, 11, 16, 23, 59, 59, DateTimeKind.Utc)
            },
            new TodoItem
            {
                Id = 5,
                Title = "إعداد Jenkins Pipeline",
                Description = "تكوين Jenkins للـ CI/CD مع GitHub",
                Priority = Domain.Enums.Priority.Urgent,
                Category = "DevOps",
                Tags = "jenkins,cicd,devops",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 13, 8, 0, 0, DateTimeKind.Utc),
                IsCompleted = false,
                EstimatedHours = 8,
                DueDate = new DateTime(2025, 11, 14, 23, 59, 59, DateTimeKind.Utc)
            },
            new TodoItem
            {
                Id = 6,
                Title = "كتابة Unit Tests",
                Description = "إضافة اختبارات شاملة للمشروع",
                Priority = Domain.Enums.Priority.Medium,
                Category = "اختبار",
                Tags = "testing,unit-tests",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 13, 10, 0, 0, DateTimeKind.Utc),
                IsCompleted = false,
                EstimatedHours = 12,
                DueDate = new DateTime(2025, 11, 17, 23, 59, 59, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<TodoComment>().HasData(
            new TodoComment
            {
                Id = 1,
                TodoItemId = 1,
                Content = "تم إنشاء المشروع بنجاح مع جميع الطبقات المطلوبة",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 11, 14, 35, 0, DateTimeKind.Utc)
            },
            new TodoComment
            {
                Id = 2,
                TodoItemId = 2,
                Content = "MediatR يعمل بشكل ممتاز! تم تطبيق Commands و Queries",
                CreatedBy = "hnjm",
                CreatedDate = new DateTime(2025, 11, 12, 16, 10, 0, DateTimeKind.Utc)
            }
        );
    }
}