using TodoBlazorApp.Domain.Common;

namespace TodoBlazorApp.Domain.Entities;

public class TodoComment : BaseEntity, IAuditableEntity
{
    public int TodoItemId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // Navigation Properties
    public virtual TodoItem TodoItem { get; set; } = null!;
}