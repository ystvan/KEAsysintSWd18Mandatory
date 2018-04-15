using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using KEAWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KEAWebApp
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
            //do not map old conventional claim types with WS - Trust
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            //Dependency Injection
            services.AddScoped<ISensorHttpClient, SensorHttpClient>();

            //OIDC
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = "https://rollcallkea-testidentity.azurewebsites.net/";
                
                options.RequireHttpsMetadata = true;
                options.ClientId = "keaclient-sysint-1";
                options.ClientSecret = "@mysupersecret321$";

                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                

                options.ResponseType = "code id_token";
                options.SignInScheme = "Cookies";
                options.SaveTokens = true;

                options.ClaimActions.Remove("");
                
                options.GetClaimsFromUserInfoEndpoint = false;
                
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
