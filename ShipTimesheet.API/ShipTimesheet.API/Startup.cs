using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShipTimesheet.API.Helpers;
using ShipTimesheet.API.MappingProfiles;
using ShipTimesheet.API.Repositories;
using ShipTimesheet.API.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShipTimesheet.API
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
            services.AddOptions();
            services.AddDbContext<ShipTimesheetDbContext>(opt => opt.UseInMemoryDatabase("ShipTimesheetDatabase"));
            services.AddCustomCors("AllowAllOrigins");

            services.AddSingleton<ISeedDataService, SeedDataService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IShipRepository, ShipRepository>();
            services.AddScoped<ISkipperRepository, SkipperRepository>();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            
            services.AddControllers()
                   .AddNewtonsoftJson(options =>
                       {
                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                           options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                       }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);



        services.AddVersioning();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(EventMappingProfile));
            services.AddAutoMapper(typeof(ShipMappingProfile));
            services.AddAutoMapper(typeof(SkipperMappingProfile));

            services.AddHostedService<TcpClientService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            ILoggerFactory loggerFactory, 
            IWebHostEnvironment env, 
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.AddProductionExceptionHandling(loggerFactory);
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAllOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}
