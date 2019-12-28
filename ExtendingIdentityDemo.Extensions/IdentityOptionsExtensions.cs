using System;
using Microsoft.AspNetCore.Identity;

namespace ExtendingIdentityDemo.Extensions
{
    public static class IdentityOptionsExtensions
    {
        public static IdentityOptions UseDevelopmentPasswordOptions(this IdentityOptions options)
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            return options;
        }
    }
}
