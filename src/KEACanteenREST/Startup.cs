﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KEACanteenREST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Diagnostics;

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
            services.AddMvc(setupAction => {

                // Content negotiation Request Header Accept: (default) application/json, but application/xml is supported too
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });

            services.AddDbContext<db_sysint_prodContext>(options => options.UseSqlServer(Configuration["connectionStrings:azureDBConnectionString"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {            
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
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("Unexpected fault happened. Please try again later!");
                        });
                });
            }

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

            app.UseMvc();
        }
    }
}
