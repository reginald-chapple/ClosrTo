namespace Mvc.Entities;

public class RelationshipGoal : Entity
{
    public long Id { get; set; }

    public RelationshipChoices Relationship { get; set; }

    public required string Name { get; set; }

    public required string Goal { get; set; }

    public required string ToWhom { get; set; }

    public DateOnly Date { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsCompleted { get; set; } = false;

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

}