using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Domain.Settings
{
    public class CorsSettings : IAppSettings
    {
        public string CORS { get; set; }
    }
}
