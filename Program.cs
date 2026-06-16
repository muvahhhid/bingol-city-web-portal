using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MyCityProject.Data;
using MyCityProject.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 12;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
});

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Supported languages
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("tr"),
    new CultureInfo("ru")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default public route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedDatabaseAsync(services, app.Configuration);
}

app.Run();

async Task SeedDatabaseAsync(IServiceProvider services, IConfiguration configuration)
{
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    await dbContext.Database.MigrateAsync();

    await SeedRolesAndAdminAsync(roleManager, userManager, configuration);
    await SeedDistrictsAsync(dbContext);
}

async Task SeedRolesAndAdminAsync(
    RoleManager<IdentityRole> roleManager,
    UserManager<User> userManager,
    IConfiguration configuration)
{
    const string adminRole = "Admin";

    var adminEmail = configuration["AdminUser:Email"];
    var adminPassword = configuration["AdminUser:Password"];

    if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
    {
        throw new InvalidOperationException(
            "Admin credentials are missing. Set AdminUser:Email and AdminUser:Password using User Secrets or environment variables.");
    }

    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // Remove old demo admin account if it exists
    const string oldDemoEmail = "admin@test.com";
    var oldDemoUser = await userManager.FindByEmailAsync(oldDemoEmail);

    if (oldDemoUser != null &&
        !oldDemoEmail.Equals(adminEmail, StringComparison.OrdinalIgnoreCase))
    {
        await userManager.DeleteAsync(oldDemoUser);
    }

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Admin user could not be created: {errors}");
        }

        await userManager.AddToRoleAsync(adminUser, adminRole);
    }
    else if (!await userManager.IsInRoleAsync(adminUser, adminRole))
    {
        await userManager.AddToRoleAsync(adminUser, adminRole);
    }
}

async Task SeedDistrictsAsync(ApplicationDbContext dbContext)
{
    var seedDistricts = new List<District>
    {
        new District
        {
            Name = "Bingöl Merkez",
            Description = "Bingöl Merkez is the central urban area of the province and the administrative heart of Bingöl.",
            ImagePath = "/images/bingol-merkez.jpg"
        },
        new District
        {
            Name = "Adaklı",
            Description = "Adaklı is a district of Bingöl known for its rural settlements, highland geography and mountainous surroundings.",
            ImagePath = "/images/bingol-adakli.jpg"
        },
        new District
        {
            Name = "Genç",
            Description = "Genç is one of Bingöl's districts, located in a landscape shaped by valleys, roads and natural surroundings.",
            ImagePath = "/images/bingol-genc.jpg"
        },
        new District
        {
            Name = "Karlıova",
            Description = "Karlıova is a district of Bingöl associated with high altitude, cold winters and mountain landscapes.",
            ImagePath = "/images/karliova-2.jpeg"
        },
        new District
        {
            Name = "Kiğı",
            Description = "Kiğı is a district of Bingöl with historical settlement patterns, rural character and natural scenery.",
            ImagePath = "/images/kigi.jpeg"
        },
        new District
        {
            Name = "Solhan",
            Description = "Solhan is a Bingöl district known especially for the Yüzen Ada natural formation and its surrounding landscape.",
            ImagePath = "/images/bingol-solhan.jpg"
        },
        new District
        {
            Name = "Yayladere",
            Description = "Yayladere is a district of Bingöl connected with highland culture, rural life and mountain routes.",
            ImagePath = "/images/yayladere-1-big.jpg"
        },
        new District
        {
            Name = "Yedisu",
            Description = "Yedisu is a district of Bingöl known for its natural environment, rural settlements and mountainous geography.",
            ImagePath = "/images/bingol-yedisu.jpg"
        }
    };

    var existingDistricts = await dbContext.Districts.ToListAsync();

    var validDistrictNames = seedDistricts
        .Select(d => d.Name)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var invalidDistricts = existingDistricts
        .Where(d => !validDistrictNames.Contains(d.Name))
        .ToList();

    if (invalidDistricts.Any())
    {
        dbContext.Districts.RemoveRange(invalidDistricts);
    }

    foreach (var seedDistrict in seedDistricts)
    {
        var existingDistrict = existingDistricts
            .FirstOrDefault(d => d.Name.Equals(seedDistrict.Name, StringComparison.OrdinalIgnoreCase));

        if (existingDistrict == null)
        {
            dbContext.Districts.Add(seedDistrict);
        }
        else
        {
            existingDistrict.Description = seedDistrict.Description;
            existingDistrict.ImagePath = seedDistrict.ImagePath;
        }
    }

    await dbContext.SaveChangesAsync();
}