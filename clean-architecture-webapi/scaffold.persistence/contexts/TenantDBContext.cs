using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Entities;
using Scaffold.Domain.Entities.Multitenancy;
using Scaffold.Domain.Interfaces;
using Scaffold.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scaffold.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scaffold.Persistence.Contexts
{
    public class TenantDbContext : DbContext
    {        
        private TenantSettings _settings;

        public TenantDbContext(DbContextOptions<TenantDbContext> options,
            IOptions<TenantSettings> tenantSettings) : base(options)
        {
            _settings  = tenantSettings.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _settings.Defaults.ConnectionString;
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _settings.Defaults.DBProvider;
                if (DBProvider.ToLower() == "postgresql")
                {
                    optionsBuilder.UseOracle(tenantConnectionString);
                }
            }

        }

        public DbSet<Tenant> Tenant { get; set; }

      


    }
}
