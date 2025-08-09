using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mvc.Entities;

namespace Mvc.Data;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserClaimsPrincipalFactory(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> options) :
        base(userManager, roleManager, options)
    {
        _context = context;
        _userManager = userManager;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim("FullName", user.FullName));

        return identity;
    }
}