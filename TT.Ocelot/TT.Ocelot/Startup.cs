using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Ocelot.Administration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TT.Ocelot.Models;
using TT.Ocelot.Models.Constants;

namespace TT.Ocelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        void Options(IdentityServerAuthenticationOptions o)
        {
            o.Authority = Configuration[AppSettingKeys.Authority];
            o.RequireHttpsMetadata = Configuration.GetValue<bool>(AppSettingKeys.RequireHttpsMetadata);
            o.SupportedTokens = SupportedTokens.Both;
            o.ApiSecret = Configuration[AppSettingKeys.ApiSecret];
            o.ApiName = Configuration[AppSettingKeys.ApiName];
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var adminPath = Configuration[AppSettingKeys.AdministrationPath];
            //IdentityModelEventSource.ShowPII = true;
            var enableForOutsideIdentity = Configuration.GetValue<bool>(AppSettingKeys.EnableOutsideIdentityServer);
            if (enableForOutsideIdentity) 
            {
                services
               .AddOcelot(Configuration)
               .AddAdministration("/administration", Options );
            }
            if (!enableForOutsideIdentity && !string.IsNullOrEmpty(Configuration[AppSettingKeys.SecretForInternal])) 
            {
                services
               .AddOcelot(Configuration)
               .AddAdministration("/administration", Configuration[AppSettingKeys.SecretForInternal]);
            }
 
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseOcelot().Wait();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
