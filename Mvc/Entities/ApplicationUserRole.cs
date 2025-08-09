using Microsoft.AspNetCore.Identity;

namespace Mvc.Entities;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public ApplicationUser? User { get; set; }
    public ApplicationRole? Role { get; set; }
}