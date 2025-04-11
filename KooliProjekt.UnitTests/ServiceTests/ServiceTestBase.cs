using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                _dbContext = new ApplicationDbContext(options);
                return _dbContext;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
