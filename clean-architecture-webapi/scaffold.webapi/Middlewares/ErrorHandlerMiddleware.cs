using Scaffold.Application.Exceptions;
using Scaffold.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scaffold.WebApi.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlerMiddleware
    {


        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
          //  _logger = logger;
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                //if (context.Response.StatusCode == 404)
                //{
                //    await context.Response.WriteAsync("End Point Not Found");
                //    //await next();
                //}
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ApiResponse<string>() {
                    Succeeded = false,
                    Message = $"{error?.InnerException?.Message}",
                    Errors = new List<string>()
                    {
                        (error.Message)
                    }
                };

                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        error.Data["Validation Errors"] = JsonSerializer.Serialize(e.Errors);
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);

            //    _logger.LogError(error.GetBaseException(), responseModel.Message);

                await response.WriteAsync(result);
            }
        }
    }
}
