using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        Container DIContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            DIContainer = new Container(config =>
            {
                config.AddRegistry(new DataAccess.StructureMapRegistry());
                config.AddRegistry(new Business.StructureMapRegistry());
                config.For<IConfiguration>().Use(Configuration);
                config.For<IApplicationSettings>().Use<ASPNETCoreApplicationSettings>();
                config.For<ResourceFileUrlRewriteRule>().Use<ResourceFileUrlRewriteRule>();
            });
            
            DIContainer.Inject<IDIContainer>(new StructureMapDIContainer(DIContainer));

            services.AddTransient<IDomainHub>(provider => DIContainer.GetInstance<IDomainHub>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Rewrite File URLs with Azure Storage SAS
            var rewriteOptions = new RewriteOptions();
            rewriteOptions.Add(DIContainer.GetInstance<ResourceFileUrlRewriteRule>());

            app.UseRewriter(rewriteOptions);
        }
    }
}
