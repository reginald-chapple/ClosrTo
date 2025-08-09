using System.ComponentModel.DataAnnotations;

namespace Mvc.Entities;

public class Entity : IEntity
{
    [ScaffoldColumn(false)] public DateTime Created { get; set; }

    [ScaffoldColumn(false)] public DateTime Updated { get; set; }

    [ScaffoldColumn(false)] public bool IsDeleted { get; set; } = false;

    [ScaffoldColumn(false)] public DateTime? DeletedAt { get; set; }

    // [Required] [ScaffoldColumn(false)] [Editable(false)] public Guid TenantId { get; set; }
}