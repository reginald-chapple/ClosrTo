namespace Mvc.Entities;

public class ScriptureReference : Entity
{
    public int Id { get; set; }
   
    public required string Book { get; set; }
    public int Chapter { get; set; }
    public required string Verses { get; set; } // Using string to accommodate ranges like "10-18"
    public required string Text { get; set; }

    // Navigation property back to practices
    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}