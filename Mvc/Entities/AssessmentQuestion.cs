namespace Mvc.Entities;

public class AssessmentQuestion : Entity
{
    public int Id { get; set; }
    public required string Text { get; set; }

    // Foreign key to the Vice this question helps measure
    public int ViceId { get; set; }
    public virtual Vice? Vice { get; set; }
}