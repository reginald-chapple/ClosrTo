using Microsoft.EntityFrameworkCore;

namespace Mvc.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Virtue : Entity
{
    public int Id { get; set; }

    public required string Name { get; set; } // e.g., "Humility"
    public required string Description { get; set; }

    // Navigation property back to its corresponding vice
    public virtual Vice? CorrespondingVice { get; set; }

    // Navigation property for associated practices
    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}