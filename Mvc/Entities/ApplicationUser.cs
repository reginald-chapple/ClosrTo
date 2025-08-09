using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Mvc.Entities;

public class ApplicationUser : IdentityUser<string>
{
    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = false;

    public virtual ICollection<ApplicationUserRole> Roles { get; set; } = new List<ApplicationUserRole>();
    public virtual ICollection<RelationshipGoal> RelationshipGoals { get; set; } = new List<RelationshipGoal>();
    public virtual ICollection<PracticeLog> PracticeLogs { get; set; } = new List<PracticeLog>();
    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

}