using Scaffold.Application.Entities;
using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Application.Handlers
{
    public abstract class BaseRequestHandler
    {
        protected readonly IUnitOfWork dbContext;
        protected readonly string timeZone;
        protected Domain.Entities.Multitenancy.Tenant ActiveTenant { get; }
        public AuthorizedUser LoggedInUser { get; set; }
        protected BaseRequestHandler(IUnitOfWork uow, ITenantService service)
        {
            dbContext = uow;
            uow.IgnorePreview = false;
            ActiveTenant = service.GetCurrentTenant();
            LoggedInUser = service.GetAuthorizedUser();
;            timeZone = service.GetCurrentTenant()?.TimeZone;
        }
        protected BaseRequestHandler(IUnitOfWork uow, ITenantService service, bool ignorePreview)
        {
            uow.IgnorePreview = ignorePreview;
            dbContext = uow;
            LoggedInUser = service.GetAuthorizedUser();
            ActiveTenant = service.GetCurrentTenant();
            timeZone = service.GetCurrentTenant()?.TimeZone;
        }
    }

}
