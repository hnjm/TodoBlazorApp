using TodoBlazorApp.Domain.Enums;

namespace TodoBlazorApp.Application.DTOs;

public record TodoItemDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime? CompletedDate { get; init; }
    public Priority Priority { get; init; }
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public DateTime? DueDate { get; init; }
    public string? AssignedTo { get; init; }
    public int? EstimatedHours { get; init; }
    public int? ActualHours { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string? ModifiedBy { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public bool IsOverdue { get; init; }
    public List<TodoCommentDto> Comments { get; init; } = new();
    public List<TodoAttachmentDto> Attachments { get; init; } = new();
}

public record CreateTodoItemDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Priority Priority { get; init; } = Priority.Medium;
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public DateTime? DueDate { get; init; }
    public string? AssignedTo { get; init; }
    public int? EstimatedHours { get; init; }
}

public record UpdateTodoItemDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public Priority Priority { get; init; }
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public DateTime? DueDate { get; init; }
    public string? AssignedTo { get; init; }
    public int? EstimatedHours { get; init; }
    public int? ActualHours { get; init; }
}

public record TodoStatsDto
{
    public int TotalCount { get; init; }
    public int CompletedCount { get; init; }
    public int PendingCount { get; init; }
    public int OverdueCount { get; init; }
    public double CompletionPercentage { get; init; }
    public Dictionary<Priority, int> ByPriority { get; init; } = new();
    public Dictionary<string, int> ByCategory { get; init; } = new();
}

public record TodoCommentDto
{
    public int Id { get; init; }
    public int TodoItemId { get; init; }
    public string Content { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
}

public record CreateTodoCommentDto
{
    public int TodoItemId { get; init; }
    public string Content { get; init; } = string.Empty;
}

public record TodoAttachmentDto
{
    public int Id { get; init; }
    public int TodoItemId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public DateTime UploadedDate { get; init; }
    public string UploadedBy { get; init; } = string.Empty;
}