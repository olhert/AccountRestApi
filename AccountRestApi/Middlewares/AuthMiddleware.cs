using System;
using System.Text;
using System.Threading.Tasks;
using AccountRestApi.Controllers;
using Microsoft.AspNetCore.Http;

namespace AccountRestApi.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
    
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
         public async Task InvokeAsync(HttpContext context)
         {
             var authHeader = context.Request.Headers["Authorization"];

             var decodeUserId = Extensions.ParseAuthToken(authHeader);
             
             
         }

         
    }
}
