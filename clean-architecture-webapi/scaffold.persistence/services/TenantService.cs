using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Entities.Multitenancy;
using Scaffold.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scaffold.Domain.Entities.Authorization;
using Scaffold.Persistence.Contexts;
using Microsoft.AspNetCore.Http;

namespace Scaffold.Persistence.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Tenant _currentTenant;
        private AuthorizedUser _authorizedUser;
        private TenantDbContext dbContext;

        public TenantService(IOptions<TenantSettings> tenantSettings, 
            IHttpContextAccessor contextAccessor, TenantDbContext dbContext)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContextAccessor = contextAccessor;
            this.dbContext = dbContext;
            if (_httpContextAccessor != null)
            {
                if (!_httpContextAccessor.HttpContext.Request.Path.Value.ToLower().Contains("/api/tenants/") &&
                    !_httpContextAccessor.HttpContext.Request.Path.Value.ToLower().Contains("/api/home/getbycode"))
                {
                    if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                    {
                        SetTenant(tenantId);
                        SetAuthorizedUser(_httpContextAccessor.HttpContext.User.Claims);
                    }
                    else
                    {
                        throw new Exception("Unauthorized Client!");
                    }
                }
            }
        }
        private void SetAuthorizedUser(IEnumerable<System.Security.Claims.Claim> claims)
        {
            _authorizedUser = new AuthorizedUser
            {
                Name = claims.FirstOrDefault(m => m.Type == "name")?.Value,
                roles = claims.FirstOrDefault(m => m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value,
                UserName = claims.FirstOrDefault(m => m.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,
            };
        }
        private void SetTenant(string tenantId)
        {
            // var spec = new Application.Features.Tenants.Queries.TenantsByValidDateSpecification();
            // var tenants = uow.Tenants.FindWithSpecificationPattern(spec);
            //return new Response<IEnumerable<Tenant>>(tenants);

            _currentTenant = dbContext.Set<Tenant>().Where(m=> m.TenantCode == tenantId).FirstOrDefault();

            if (_currentTenant != null)
            {
                _currentTenant.TenantId = _currentTenant.TenantKey;

            }

            //}
            //else
            //{
            //    // if does not exists in the database, try reading from the settings.config 
            //    _currentTenant = _tenantSettings.Tenants.Where(a => a.TenantId == tenantId).FirstOrDefault();
            //}

            // if does not exists even in config settings, throw exception.
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");

            // continue when the current Tenant is found

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }
        public AuthorizedUser GetAuthorizedUser()
        {
            return _authorizedUser;
        }
        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }
        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }
        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }
        public Tenant GetCurrentTenant()
        {
            return _currentTenant;
        }
        public string GetPreviewVersion()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("preview", out var prevId);
            return prevId;
        }
        public string GetSiteConfiguration()
        {
            return _currentTenant.TenantConfig;
        }
    }
}
