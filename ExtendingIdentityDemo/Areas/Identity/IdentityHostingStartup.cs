using System;
using ExtendingIdentityDemo.Data;
using ExtendingIdentityDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using ExtendingIdentityDemo.Extensions;

[assembly: HostingStartup(typeof(ExtendingIdentityDemo.Areas.Identity.IdentityHostingStartup))]
namespace ExtendingIdentityDemo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.UseDevelopmentPasswordOptions();

                    options.SignIn.RequireConfirmedAccount = true;

                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                    options.User.RequireUniqueEmail = true;

                })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            });
        }
    }
}