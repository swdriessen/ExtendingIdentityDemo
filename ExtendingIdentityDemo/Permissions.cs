using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtendingIdentityDemo.Models;
using ExtendingIdentityDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExtendingIdentityDemo
{
    public static class PermissionNames
    {
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
            public const string Create = "Permissions.Dashboards.Create";
            public const string Edit = "Permissions.Dashboards.Edit";
            public const string Delete = "Permissions.Dashboards.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Pages
        {
            public const string Privacy = "Permissions.Pages.Privacy";
            public const string Moderators = "Permissions.Pages.Moderators";
        }

        public static class Feature
        {
            public const string Feature1 = "Permissions.Feature.Feature1";
            public const string Feature2 = "Permissions.Feature.Feature2";
        }

        public static IEnumerable<string> GetPermissions()
        {
            yield return Dashboards.View;
        }
    }

    public class CustomClaimTypes
    {
        public const string Permission = "permission";
    }

   

    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;


        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var user = await _userManager.GetUserAsync(context.User);

            if (user == null)
                return;

            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));



            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);

                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            //also check user claims?
            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Count > 0)
            {
                var permissions = userClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);

                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
            }

        }
    }
}
