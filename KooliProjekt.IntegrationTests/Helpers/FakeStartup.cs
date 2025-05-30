using System;
using KooliProjekt.Controllers;
using KooliProjekt.Data;

using Kooliprojekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data.Repositories;
using Kooliprojekt.Data;
using Kooliprojekt.Controllers;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class FakeStartup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var dbGuid = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: dbGuid);
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews()
                    .AddApplicationPart(typeof(HomeController).Assembly);

            services.AddScoped<IProjectListService, ProjectListService>();
            services.AddScoped<IProjectItemService, ProjectItemService>();
            services.AddScoped<IWorkLogService, WorkLogService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{pathStr?}");
            });
        }
    }
}
