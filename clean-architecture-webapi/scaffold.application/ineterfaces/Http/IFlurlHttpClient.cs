using System;
using Flurl.Http;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Application.Interfaces.Http
{
    public interface IFlurlHttpClient
    {
        public IFlurlClient httpClient{ get; set; }
        public string Endpoint { get; set; }
    }
}
