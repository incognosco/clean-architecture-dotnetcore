using Scaffold.Application.Wrappers;
using Scaffold.Application.Interfaces.Http;
using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;
using Scaffold.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Scaffold.ScaPersistence.PipelineBehaviors
{
     public class FlurlHttpClientBehavior<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse> where TRequest : IFlurlHttpClient
    {

        private readonly ILogger _logger;
        private readonly HttpSettings _settings;

        public IFlurlClientFactory _flurlClientFac{ get; set; }

        public FlurlHttpClientBehavior(ILogger<TResponse> logger, IOptions<HttpSettings> settings,
            IFlurlClientFactory flurlClientFac)
        {
            _logger = logger;
            _settings = settings.Value;
            _flurlClientFac = flurlClientFac;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            TResponse response;

            request.httpClient = _flurlClientFac.Get($"{_settings.BaseUrl}{request.Endpoint}");

            request.httpClient.WithHeader("contenttype", "application/json");
            request.httpClient.WithHeader("x-api-version", "1");
            request.httpClient.WithHeader("tenant", "mt-101d");
            
            response = await next();


            return response;

            //response = request.Response;
            //  request.Response = flurlResponse;


            //if (request.BypassCache) return await next();
            //async Task<TResponse> GetResponseAndAddToCache()
            //{
            //    response = await next();
            //    var slidingExpiration = request.SlidingExpiration == null ? TimeSpan.FromHours(_settings.SlidingExpiration) : request.SlidingExpiration;
            //    var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
            //    var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
            //    await _cache.SetAsync((string)request.CacheKey, serializedData, options, cancellationToken);
            //    return response;
            //}
            //var cachedResponse = await _cache.GetAsync((string)request.CacheKey, cancellationToken);
            //if (cachedResponse != null)
            //{
            //    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            //    _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            //}
            //else
            //{
            //    response = await GetResponseAndAddToCache();
            //    _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            //}
           
        }

    }
}
