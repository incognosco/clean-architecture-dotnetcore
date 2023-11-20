

using System;
using System.Net.Http;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Scaffold.WebApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultHeaderFilter : IOperationFilter
    {    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
           // if (string.Equals(context.ApiDescription.HttpMethod, HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase))
           // {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "tenant",
                    In = ParameterLocation.Header,
                    Required = true,
                    Example = new OpenApiString("")
                });                
            // }
        }
    }
}
