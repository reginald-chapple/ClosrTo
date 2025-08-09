using Microsoft.AspNetCore.Identity;

namespace Mvc.Entities;

public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUserRole> Users { get; set; } = [];
}