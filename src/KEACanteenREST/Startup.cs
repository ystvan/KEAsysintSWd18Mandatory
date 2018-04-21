using AspNetCore.IServiceCollection.AddIUrlHelper;
using AspNetCoreRateLimit;
using KEACanteenREST.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.StaticFiles;

namespace KEACanteenREST
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
            services.AddCors();
            services.AddUrlHelper();
            services.AddMvc(setupAction =>
            {

                // Content negotiation Request Header Accept: (default) application/json, but application/xml is supported too
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());

                // Adding custom media type

                var jsonOutPutFormatter = setupAction.OutputFormatters
                                                .OfType<JsonOutputFormatter>()
                                                .FirstOrDefault();
                if (jsonOutPutFormatter != null)
                {
                    jsonOutPutFormatter.SupportedMediaTypes.Add("application/vnd.sysint.hateoas+json");
                }

            });

            // swagger for documentation

            services.AddSwaggerGen(c =>
            {
                 //this is a response to a call in case someone requests a swagger for my API
                 c.SwaggerDoc("v1",
                     new Info
                     {
                         Contact = new Contact
                         {
                             Email = "istv0050@edu.easj.dk",
                             Name = "Istvan M",
                             Url = "https://twitter.com/SmartCity_911"
                         },
                         License = new License
                         {
                             Name = "Do not try this at home",
                             Url = "https://fronter.com/kea"
                         },
                         Description = "Mandatory REST with five constarints",
                         TermsOfService = "None",
                         Title = "SysInt Mandatory REST API",
                         Version = "v1"

                     });
            });
            services.AddDbContext<db_sysint_prodContext>(options => options.UseSqlServer(Configuration["connectionStrings:azureDBConnectionString"]));

            // Cahche
            services.AddHttpCacheHeaders(
                (expirationModelOptions)
                    =>
                    { expirationModelOptions.MaxAge = 600; },
                (validationModelOptions)
                    =>
                    { validationModelOptions.AddMustRevalidate = true; }
                );
            services.AddResponseCaching();

            // Retelimiting
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>((options) =>
            {
                options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>()
                {
                    // All endpoints can have 10 calls within 2 minutes
                    new RateLimitRule(){
                        Endpoint = "*",
                        Limit = 4,
                        Period = "2m"
                    },
                    // or 2 calls within 10 seconds
                    new RateLimitRule(){
                        Endpoint = "*",
                        Limit = 2,
                        Period = "10s"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Error);

            if (env.IsDevelopment())
            {
                // When app is in development return the stack trace
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // In production return a generic error in the request/response pipeline
                // Exception is handled in a global level, no need for try-catch blocks in ie.: controllers
                app.UseExceptionHandler(appbuilder =>
                    {
                        appbuilder.Run(async context =>
                        {
                            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (exceptionHandlerFeature != null)
                            {
                                var logger = loggerFactory.CreateLogger("Serilog Global exception logger");
                                logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                            }

                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("Unexpected fault happened. Please try again later!");
                        });
                    });
            }
            //static file server
            app.UseStaticFiles();

            app.UseCors(builder =>
                builder.AllowAnyMethod().AllowAnyMethod().AllowAnyOrigin()
            );
            
            //configure a UI for swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SysInt Mandatory REST API v1");

            });

            AutoMapper.Mapper.Initialize(configure =>
            {
                configure.CreateMap<SensorDatas, RecordDto>()
                    .ForMember(dest => dest.Light, opt => opt.MapFrom(source => source.Light))
                    .ForMember(dest => dest.LocationIdentifier, opt => opt.MapFrom(source => source.Id))
                    .ForMember(dest => dest.Temperature, opt => opt.MapFrom(source => source.Temperature))
                    .ForMember(dest => dest.RecordedAt, opt => opt.MapFrom(source => source.Timestamp));

                configure.CreateMap<RecordForCreationDto, SensorDatas>();
                configure.CreateMap<RecordForUpdateDto, SensorDatas>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Timestamp, opt => opt.Ignore());

            });

            app.UseIpRateLimiting();
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            app.UseMvc();
        }
    }
}
