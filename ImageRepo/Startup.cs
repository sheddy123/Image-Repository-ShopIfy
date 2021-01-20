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
using ImageRepo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageRepo.Repository;
using ImageRepo.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ImageRepo
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
           
            
            

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    options => {
                        //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        options.AccessDeniedPath = "/Home/AccessDenied";
                        options.LoginPath = "/Home/Login";
                        options.LogoutPath = "/Home/Logout";
                    }
                );
            //services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    //options.Cookie.HttpOnly = true;
            //    //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //    //options.SlidingExpiration = true;
            //    options.AccessDeniedPath = "/Home/AccessDenied";
            //    options.LoginPath = "/Home/Login";
            //    options.LogoutPath = "/Home/Logout";
            //});
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpClient();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //app.UseRouting();
            //app.UseCors(x => x
            //.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader());

            //app.UseSession();
            
            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
