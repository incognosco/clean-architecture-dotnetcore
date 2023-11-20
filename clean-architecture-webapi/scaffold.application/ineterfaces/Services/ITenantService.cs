using Scaffold.Domain.Entities.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;
using Scaffold.Domain.Entities.Authorization;

namespace Scaffold.Application.Interfaces.Services
{
    public interface ITenantService : IScopedService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public Tenant GetCurrentTenant();
        public AuthorizedUser GetAuthorizedUser();
        string GetPreviewVersion();
        string GetSiteConfiguration();
    }
}
