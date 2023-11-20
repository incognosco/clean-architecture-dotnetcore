using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Constants.AuditTrial;
using Scaffold.Domain.Entities;
//using Scaffold.Domain.ViewEntities;
using Scaffold.Domain.Entities.AuditTrial;
using Scaffold.Domain.Entities.Multitenancy;
using Scaffold.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Scaffold.Domain.ViewEntities.Summary;

namespace Scaffold.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; }
        public string TenantKey { get; set; }
        public string PreviewVersion { get; set; }
        
        private readonly ITenantService _tenantService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetCurrentTenant()?.TenantId;
            if (!String.IsNullOrEmpty(tenantService.GetPreviewVersion()))
            {
                PreviewVersion = tenantService.GetPreviewVersion();
            }
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "oracle")
                {
                   // optionsBuilder
                        //.UseNpgsql(_tenantService.GetConnectionString())
                     //   .UseSnakeCaseNamingConvention();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Domain.Entities.User>()
             .HasQueryFilter(p => !p.IsDeleted && p.ClientKey == TenantKey);

        }
       
        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = TenantKey;
                auditEntries.Add(auditEntry);
                var auditAttr = entry.Entity.GetType()
                    .GetCustomAttributes(typeof(Domain.Common.AuditTableAttribute), false)
                    .FirstOrDefault();
                if (auditAttr != null)
                {
                    var auditTable = (Domain.Common.AuditTableAttribute)auditAttr;
                    auditEntry.TableDisplayName = $"{auditTable.TableName}";
                }

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            var propEntity = entry.Entity.GetType().GetProperty(propertyName);
                            if (propEntity != null)
                            {
                                var auditPropAttr = Attribute.IsDefined(propEntity,
                                    typeof(Domain.Common.AuditAttribute));
                                if (auditPropAttr &&                                   
                                    !string.IsNullOrEmpty(auditEntry.AuditMessage))
                                {
                                    auditEntry.AuditMessage = 
                                        $"A new {auditEntry.TableDisplayName}" +
                                        $" '{property.CurrentValue}' has been created" ;
                                }
                            }
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditTrialEntry.Add(auditEntry.ToAudit());
            }
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            OnBeforeSaveChanges();

            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantKey = TenantKey;
                        break;
                }
            }

            // for all entities that implemented IAuditableEntity, update Audit Columns.
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {

             switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _tenantService.GetAuthorizedUser()?.UserName; 
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastmodifiedBy = _tenantService.GetAuthorizedUser()?.UserName;
                        entry.Entity.LastmodifiedOn = DateTime.UtcNow;
                        break;
                }
            }

            // for all entities that implemented IAuditableEntity, update Audit Columns.
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantKey = TenantKey;
                        break;
                }
            }

            // for all entities that implemented ISoftDelete, update Delete Columns.
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedBy = _tenantService.GetAuthorizedUser()?.UserName;
                        entry.Entity.DeletedOn = DateTime.UtcNow;
                        break;
                }
            }


            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public DbSet<Tenant> Tenants { get; set; }

        private DbSet<Audit> AuditTrialEntry { get; set; }

        //public DbSet<UserTenant> UserTenants { get; set; }

        public DbSet<UserRoleMapping> UserRoles { get; set; }

       

    }
}
