namespace Mvc.Entities;

public interface IEntity
{
    DateTime Created { get; set; }
    DateTime Updated { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}