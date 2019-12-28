using System;
using Microsoft.AspNetCore.Authorization;

namespace ExtendingIdentityDemo.Permissions
{
    public static class Extensions
    {
        public static AuthorizationOptions AddPermissionPolicies(this AuthorizationOptions options, params string[] permissions)
        {
            foreach (var permission in permissions)
            {
                options.AddPolicy(permission, builder => {
                    builder.AddRequirements(new PermissionRequirement(permission));
                });
            }

            return options;
        }
    }
}
