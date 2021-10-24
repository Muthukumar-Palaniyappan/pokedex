using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Pokedex.Api.Application.Middlewares
{
    public class ApplicationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ILogger<ApplicationExceptionMiddleware> _logger;

        public ApplicationExceptionMiddleware(
            RequestDelegate next,
            IExceptionHandler exceptionHandler,
            ILogger<ApplicationExceptionMiddleware> logger)
        {
            _next = next;
            _exceptionHandler = exceptionHandler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorDocument = _exceptionHandler.HandleException(ex);
                if (httpContext.Response != null && !httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    httpContext.Response.StatusCode = (int)errorDocument.StatusCode;
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorDocument, 
                                                                                   new JsonSerializerOptions
                                                                                    {
                                                                                        IgnoreNullValues = true,
                                                                                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                                                                                    }));
                }
            }
        }
    }
}
