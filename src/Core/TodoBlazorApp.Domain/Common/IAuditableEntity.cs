namespace TodoBlazorApp.Domain.Common;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? ModifiedDate { get; set; }
}