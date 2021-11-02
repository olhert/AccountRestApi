using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AccountRestApi.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"Url:{httpContext.Request.Path}");
            await _next(httpContext);
        }
    }
}