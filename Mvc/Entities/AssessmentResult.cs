namespace Mvc.Entities;

public class AssessmentResult : Entity
{
    public int Id { get; set; }

    public int TotalScore { get; set; }

    public string AssessmentId { get; set; } = string.Empty;
    public virtual Assessment? Assessment { get; set; }

    public int ViceId { get; set; }
    public virtual Vice? Vice { get; set; }
}