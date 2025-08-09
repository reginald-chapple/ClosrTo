namespace Mvc.Entities;

public class PracticeLog : Entity
{
    public int Id { get; set; }
    public string? Reflections { get; set; } // User's journal entry about the practice
    public int? Rating { get; set; } // Optional user rating (e.g., 1-5) on how it went

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    public int PracticeId { get; set; }
    public virtual Practice? Practice { get; set; }
}