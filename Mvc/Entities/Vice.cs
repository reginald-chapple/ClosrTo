using Microsoft.EntityFrameworkCore;

namespace Mvc.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Vice : Entity
{
    public int Id { get; set; }

    public required string Name { get; set; } // e.g., "Pride"
    public required string Description { get; set; }

    // Foreign key to its corresponding virtue
    public int CorrespondingVirtueId { get; set; }
    public virtual Virtue? CorrespondingVirtue { get; set; }

    // Navigation property for associated practices
    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
    public virtual ICollection<AssessmentQuestion> AssessmentQuestions { get; set; } = new List<AssessmentQuestion>();
}