using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using UrlsAndRoutes.Infrastructure;

namespace UrlsAndRoutes
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => {
                options.ConstraintMap.Add("weekday", typeof(WeekDayConstraint));
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(builder =>
            {
                builder.MapRoute(name: "areas", template: "{area:exists}/{controller=Home}/{action=Index}");

                builder.Routes.Add(new LegacyRoute(
                    app.ApplicationServices,
                    "/articles/Windows_3.1_Overview.html",
                    "/old/.NET_1.0_Class_Library"));

                //routes.MapRoute(name: "NewRoute", template: "App/Do{action}", defaults: new {controller = "Home"});
                builder.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                builder.MapRoute(name: "out", template: "outbound/{controller=Home}/{action=Index}");
            });
        }
    }
}
