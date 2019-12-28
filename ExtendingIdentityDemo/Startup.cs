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
using ExtendingIdentityDemo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExtendingIdentityDemo.Models;
using Microsoft.AspNetCore.Authorization;
using ExtendingIdentityDemo.Permissions;

namespace ExtendingIdentityDemo
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



            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });


            //moved to scaffolding
            //services.AddDefaultIdentity<ApplicationUser>(options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = true;

            //    //if (env.IsDevelopment())
            //    //{
            //    //added for testing
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredLength = 6;
            //    //}



            //    //abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_

            //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            //    options.User.RequireUniqueEmail = true;

            //})
            //.AddRoles<IdentityRole>()            
            //.AddEntityFrameworkStores<ApplicationDbContext>();

            //

            services.AddAuthorization(options =>
            {
                options.AddPermissionPolicies(
                    PermissionNames.Dashboards.View,
                    PermissionNames.Pages.Privacy,
                    PermissionNames.Pages.Moderators,
                    PermissionNames.Feature.Feature1,
                    PermissionNames.Feature.Feature2
                    );
            });


            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                //when removing role, update stamp and logout user
                options.ValidationInterval = TimeSpan.FromSeconds(0);
            });

            //when using asp.net identity is it good practice to call UpdateSecurityStampAsync on the UserManager in combination with a ValidationInterval of 0 when removing Role/Claim to block access right away?
            //

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("area_default", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
