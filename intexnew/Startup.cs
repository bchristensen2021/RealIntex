using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using intexnew.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using intexnew.Models;
using intexnew.Models.ViewModels;
using BookStore.Models;
using Microsoft.AspNetCore.Http;

namespace intexnew
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration temp)
        {
            Configuration = temp;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //This was for sqlite connection
            //services.AddDbContext<BookContext>(options =>
            //{
            //    options.UseSqlite(Configuration["ConnectionStrings:BookDBConnection"]);
            //});

            //why is the DbContext AppIdentity?, Bethany is commenting this out and changing it to the line below
            //services.AddDbContext<AppIdentityDBContext>(options =>
             //options.UseMySql(Configuration["ConnectionStrings:CrashesDbConnection"]));

            services.AddDbContext<CrashesDbContext>(options =>
            {
                options.UseMySql(Configuration["ConnectionStrings:CrashesDbConnection"]);
            });
             

            //Identity is for security/permissions
            services.AddDbContext<AppIdentityDBContext>(options =>
               options.UseMySql(Configuration["ConnectionStrings:IdentityConnection"]));
         

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            services.AddScoped<ICrashRepository, EFCrashRepository>();
            //services.AddScoped<ICrashRepository, EFCrashRepository>();


            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession();

            //services.AddScoped<Basket>(x => SessionBasket.GetBasket(x));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //uses wwwroot
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("categorypage", "{category}/Page{pageNum}", new { Controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "Paging",
                    pattern: "Page{pageNum}",
                    defaults: new { Controller = "Home", action = "CrashCardIndex", pageNum = 1 });

                endpoints.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "CrashCardIndex", pageNum = 1 });

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });

            IdentitySeedData.EnsurePopulated(app);

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });
        }
    }
}
