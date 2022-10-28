using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Core.Interfaces;
using API.Helpers;
using API.Middleware;
using API.Errors;
using API.Extensions;

namespace API
{
    public class Startup
    {
        
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
            //candel Configuration = configuration;

        }

        //candel public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddScoped<IProductRepository, ProductRepository>();
            // services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            // });

            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddApplicationServices();
            services.AddSwaggerDocumentation();

            // services.Configure<ApiBehaviorOptions>( options =>{

            //     options.InvalidModelStateResponseFactory = actionContext => {
            //         var errors = actionContext.ModelState
            //                 .Where(e => e.Value.Errors.Count > 0)
            //                 .SelectMany(x => x.Value.Errors)
            //                 .Select( x => x.ErrorMessage).ToArray();
                        
            //         var errorResponse = new ApiValidationErrorResponse
            //         {
            //             Errors = errors
            //         };
                    
            //         return new BadRequestObjectResult(errorResponse);

            //     };
            // });

            //wag mong iuuncomment ung sa baba, mageerror ung web api services
            // services.AddSwaggerGen( c =>{
            //     c.SwaggerDoc("v1", new OpenApiInfo{ Title="API", Version="v1"});
            // });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            // app.UseSwagger();
            // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));

            // if (env.IsDevelopment())
            // {
            //     //app.UseDeveloperExceptionPage();
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            // }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
