using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Domain.Interfaces
{
    public interface IMustHaveTenant
    {
        public string TenantKey { get; set; }
    }
}
