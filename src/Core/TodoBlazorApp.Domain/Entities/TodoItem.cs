using TodoBlazorApp.Domain.Common;
using TodoBlazorApp.Domain.Enums;

namespace TodoBlazorApp.Domain.Entities;

public class TodoItem : BaseEntity, IAuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedDate { get; set; }
    public Priority Priority { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public int? EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
    public string? AttachmentUrl { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation Properties
    public virtual ICollection<TodoComment> Comments { get; set; } = new List<TodoComment>();
    public virtual ICollection<TodoAttachment> Attachments { get; set; } = new List<TodoAttachment>();

    // Domain Methods
    public void MarkAsCompleted(string completedBy)
    {
        IsCompleted = true;
        CompletedDate = DateTime.UtcNow;
        ModifiedBy = completedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void MarkAsIncomplete(string modifiedBy)
    {
        IsCompleted = false;
        CompletedDate = null;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void UpdatePriority(Priority newPriority, string modifiedBy)
    {
        Priority = newPriority;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public bool IsOverdue()
    {
        return !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;
    }
}