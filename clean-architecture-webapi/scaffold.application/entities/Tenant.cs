using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Scaffold;

namespace Scaffold.Application.Entities
{
    public class Tenant : Domain.Entities.Multitenancy.Tenant
    {
        public Tenant() { }
    }
}
