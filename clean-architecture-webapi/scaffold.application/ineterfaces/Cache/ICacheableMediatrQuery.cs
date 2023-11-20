using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Application.Interfaces.Cache
{
    public interface ICacheableMediatrQuery
    {
        bool BypassCache { get; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
