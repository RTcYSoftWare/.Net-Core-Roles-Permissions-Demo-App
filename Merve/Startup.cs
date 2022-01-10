using Merve.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merve
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
            services.AddRazorPages();
            services.AddEntityFrameworkNpgsql().AddDbContext<DbTContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("DBConnection")));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users",
                    defaults: new { controller = "Users", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Create",
                    defaults: new { controller = "Users", action = "Create" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Delete",
                    defaults: new { controller = "Users", action = "Delete" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Delete/{UserId?}",
                    defaults: new { controller = "Users", action = "Delete" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Details",
                    defaults: new { controller = "Users", action = "Details" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Edit/{UserId?}",
                    defaults: new { controller = "Users", action = "Edit" });
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "/Users/Login",
                    defaults: new { controller = "Users", action = "Login" });
            });
        }
    }
}
