using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using Mvc.Data;
using Mvc.Entities;
using Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DevConnection") ??
                       throw new InvalidOperationException("Connection string 'DevConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);

    // Register SoftDeleteCascadeInterceptor if needed
    // options.AddInterceptors(new Web.Interceptors.SoftDeleteCascadeInterceptor());
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

var ollamaConfig = new OllamaConfig
{
    Endpoint = "http://localhost:11434",
    TextModel = new OllamaModelConfig("phi4-mini-reasoning", 131072),
    EmbeddingModel = new OllamaModelConfig("mxbai-embed-large", 1024)
};

builder.Services.AddKernelMemory(memory =>
{
    memory.WithOllamaTextGeneration(ollamaConfig);
    memory.WithOllamaTextEmbeddingGeneration(ollamaConfig);
    memory.Build<MemoryServerless>();
});

builder.Services.AddMemoryCache();
// builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
//builder.Services.AddScoped<FlashMessageService>();
//builder.Services.AddTransient<NotificationService>();
builder.Services.AddScoped<AssessmentService>();
builder.Services.AddSignalR(configure => configure.EnableDetailedErrors = true);
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>(); // Optional: for logging
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

    // Check if seeding is likely needed (e.g., no roles exist yet)
    // You could also check !userManager.Users.Any() or a combination
    if (!await roleManager.Roles.AnyAsync()) // More efficient initial check
    {
        logger.LogInformation("Performing initial database seeding for roles and admin user.");
        await SeedRolesAsync(roleManager, logger);
        await SeedAdminUserAsync(userManager, roleManager, logger); // Pass roleManager if needed for role assignment check
        logger.LogInformation("Seeding complete.");
    }
    else
    {
        logger.LogDebug("Database already seeded. Skipping.");
    }
}

async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager, ILogger logger)
{
    var roles = new[] { "Administrator", "Member", "Staff" };
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new ApplicationRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded) { /* Log error */ }
        }
    }
}

async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger logger)
{
    var adminEmail = "sudo@local.com";
    var adminUserName = "sudo";
    var adminPassword = "P@ss1234";
    var adminRole = "Administrator";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = adminUserName,
            Email = adminEmail,
            EmailConfirmed = true,
            IsActive = true,
        };

        var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createUserResult.Succeeded)
        {
            logger.LogInformation($"Admin user {adminEmail} created successfully.");
            // Ensure the role exists before assigning
            if (await roleManager.RoleExistsAsync(adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
                logger.LogInformation($"Admin user {adminEmail} added to role {adminRole}.");
            }
            else
            {
                logger.LogWarning($"Role {adminRole} does not exist. Cannot assign to admin user.");
            }
        }
        else
        {
            // Log errors
            logger.LogError($"Failed to create admin user {adminUserName}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers().WithStaticAssets();
//app.MapHub<NotificationHub>("/notificationHub");

app.Run();
