namespace Mvc.Entities;

public class Assessment : Entity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    // Navigation property to all the individual answers for this assessment
    public virtual ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();
    
    // Navigation property to the final calculated results for this assessment
    public virtual ICollection<AssessmentResult> Results { get; set; } = new List<AssessmentResult>();
}