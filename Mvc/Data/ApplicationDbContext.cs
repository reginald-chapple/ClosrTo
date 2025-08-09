using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mvc.Entities;

namespace Mvc.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>,
    IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // public DbSet<Notification> Notifications { get; set; }
    // public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<RelationshipGoal> RelationshipGoals { get; set; }
    public DbSet<Vice> Vices { get; set; }
    public DbSet<Virtue> Virtues { get; set; }
    public DbSet<Practice> Practices { get; set; }
    public DbSet<PracticeLog> PracticeLogs { get; set; }
    public DbSet<ScriptureReference> ScriptureReferences { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
    public DbSet<AssessmentResponse> AssessmentResponses { get; set; }
    public DbSet<AssessmentResult> AssessmentResults { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRole.HasOne(ur => ur.User)
                .WithMany(r => r.Roles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        
        builder.Entity<Vice>()
          .HasOne(v => v.CorrespondingVirtue)
          .WithOne(vt => vt.CorrespondingVice)
          .HasForeignKey<Vice>(v => v.CorrespondingVirtueId);

        // builder.Entity<UserNotification>(un =>
        // {
        //     un.HasKey(u => new { u.NotificationId, u.UserId });

        //     un.HasOne(u => u.Notification)
        //         .WithMany(n => n.Users)
        //         .HasForeignKey(u => u.NotificationId)
        //         .IsRequired();

        //     un.HasOne(n => n.User)
        //         .WithMany(u => u.Notifications)
        //         .HasForeignKey(n => n.UserId)
        //         .IsRequired();
        // });


    }

    private void ApplyTimestamps()
    {
        var changedEntities = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity && 
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in changedEntities)
        {
            var entity = (Entity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.Created = DateTime.Now;
            }
            entity.Updated = DateTime.Now;
        }
    }

    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return await base.SaveChangesAsync(true, cancellationToken);
    }
}