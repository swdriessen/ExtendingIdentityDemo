using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExtendingIdentityDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExtendingIdentityDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Moderator()
        {
            return View();
        }


        [Authorize(Permissions.Pages.Privacy)]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Permissions.Feature.Feature1)]
        public IActionResult Feature1()
        {
            return View();
        }

        [Authorize(Permissions.Feature.Feature2)]
        public IActionResult Feature2()
        {
            return View();
        }





        public async Task<IActionResult> AddFeature1()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            await userManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Permissions.Feature.Feature1));

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddFeature2()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            await userManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Permissions.Feature.Feature2));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFeature1()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            await userManager.RemoveClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Permissions.Feature.Feature1));

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveFeature2()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            await userManager.RemoveClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Permissions.Feature.Feature2));

            return RedirectToAction("Index");
        }
        //public IActionResult AddPersmissionViewDashboard()
        //{
        //    var manager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();



        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> RemovePersmissionViewDashboardAsync()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");
            var adminRole = await roleManager.FindByNameAsync("Admin");

            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Dashboards.View));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Dashboards.Create));


            //userManager.claim


            return RedirectToAction("Index");
        }


















        public async Task<IActionResult> AddClaims()
        {
            var manager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRole = await roleManager.FindByNameAsync("Admin");
            
            var result = await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Dashboards.View));
                       

            return RedirectToAction("Index");
        }












        public async Task<IActionResult> CreateRoles()
        {

            var manager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRolesAsync(manager, roleManager);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddModerator()
        {

            var manager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateModeratorRolesAsync(manager, roleManager);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveModerator()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            if (admin != null)
            {
                var result = await userManager.RemoveFromRoleAsync(admin, "Moderator");
                if (result.Succeeded)
                {                    
                    await userManager.UpdateSecurityStampAsync(admin);
                }
            }

            //var role = await roleManager.FindByNameAsync("Moderator");

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private async Task CreateRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Manager", "Moderator", "ProMember", "Member" };

            //initializing custom roles 

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            if (admin != null)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
        private async Task CreateModeratorRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var admin = await userManager.FindByEmailAsync("bas_driessen@hotmail.com");

            if (admin != null)
            {

                var role = await roleManager.FindByNameAsync("Moderator");

                var result = await userManager.AddToRoleAsync(admin, role.Name);

                if (result.Succeeded)
                {
                    //do not call it when adding roles, promt user to sign in again to process it
                    //await userManager.UpdateSecurityStampAsync(admin);
                }

            }
        }
    }
}
