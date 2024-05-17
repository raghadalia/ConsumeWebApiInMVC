using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.Authentication_Models;
using ToDo.Controllers;
using ToDo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;

    // Lockout Settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI(); // This line adds default UI elements like login, registration, etc.


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Identity.Application";
    options.DefaultChallengeScheme = "Identity.Application";
    options.DefaultSignInScheme = "Identity.Application";
})
.AddIdentityCookies();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddHttpClient();
var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //await ContextSeed.SeedRolesAsync(userManager, roleManager);
    //await ContextSeed.SeedAdminAsync(userManager, roleManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();
