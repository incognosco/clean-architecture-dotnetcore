using Scaffold.Domain.Entities.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Domain.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
   
    public class Configuration
    {
        public string DBProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
