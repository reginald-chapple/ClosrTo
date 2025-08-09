using Microsoft.EntityFrameworkCore;

namespace Mvc.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Practice : Entity
{
    public int Id { get; set; }

    public required string Name { get; set; } // e.g., "Practice Gratitude"
    public required string Description { get; set; }
    public PracticeType Type { get; set; }

    // Navigation properties to link practices to vices and virtues
    public virtual ICollection<Vice> Vices { get; set; } = new List<Vice>();
    public virtual ICollection<Virtue> Virtues { get; set; } = new List<Virtue>();
    public virtual ICollection<ScriptureReference> ScriptureReferences { get; set; } = new List<ScriptureReference>();
}