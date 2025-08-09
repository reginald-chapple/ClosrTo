namespace Mvc.Entities;

public class AssessmentResponse : Entity
{
    public int Id { get; set; }
   
    public int Score { get; set; } // The user's answer (1-5)

    public string AssessmentId { get; set; } = string.Empty;
    public virtual Assessment? Assessment { get; set; }

    public int AssessmentQuestionId { get; set; }
    public virtual AssessmentQuestion? AssessmentQuestion { get; set; }
}