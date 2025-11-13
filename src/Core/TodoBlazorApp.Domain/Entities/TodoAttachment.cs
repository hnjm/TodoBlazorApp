using TodoBlazorApp.Domain.Common;

namespace TodoBlazorApp.Domain.Entities;

public class TodoAttachment : BaseEntity
{
    public int TodoItemId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedDate { get; set; }
    public string UploadedBy { get; set; } = string.Empty;

    // Navigation Properties
    public virtual TodoItem TodoItem { get; set; } = null!;
}