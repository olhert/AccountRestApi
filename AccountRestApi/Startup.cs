using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountRestApi.Controllers;
using AccountRestApi.DB;
using AccountRestApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;

namespace AccountRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IAccountsStore>(new DbAccountsStore("User ID=hays;Password=;Host=localhost;Port=5432;Database=AccountsDataBase;"));
            services.AddSingleton<IUserStore>(new DbUserStore("User ID=hays;Password=;Host=localhost;Port=5432;Database=AccountsDataBase;"));

            services.AddSwaggerDocument(o =>
            {
                o.Title = "API";
                o.GenerateEnumMappingDescription = true;
    
                o.AddSecurity("Bearer", Enumerable.Empty<string>(),
                    new NSwag.OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Description = "Bearer Token",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Name = "Authorization"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountRestApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/apisecure"), appBuilder =>
            {
                appBuilder.UseMiddleware<AuthMiddleware>();
            });
            
            app.UseMiddleware<LogMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            
        }
    }
}