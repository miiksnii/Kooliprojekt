using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Kooliprojekt.Data;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
        }

        public HttpClient CreateClient()
        {
            return Factory.CreateClient();
        }

        public ApplicationDbContext GetDbContext()
        {
            return (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        public void Dispose()
        {
            var dbContext = GetDbContext();
            if (dbContext != null)
            {
                dbContext.Database.EnsureDeleted();
            }
            Factory.Dispose();
        }
    }
}
