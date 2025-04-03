using Kooliprojekt.Data;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the DbContext with a database provider (e.g., SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Or UseSqlite, UseNpgsql, etc.

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IProjectListService, ProjectListService>();
builder.Services.AddScoped<IProjectItemService, ProjectItemService>();
builder.Services.AddScoped<IWorkLogService, WorkLogService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//repos
builder.Services.AddScoped<IProjectListRepository, ProjectListRepository>();
builder.Services.AddScoped<IProjectItemRepository, ProjectItemRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define your controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#if DEBUG
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>())
{
    context.Database.EnsureCreated();
    SeedData.Generate(context, userManager); // This assumes you have a method to seed data.
}
#endif

app.Run();
