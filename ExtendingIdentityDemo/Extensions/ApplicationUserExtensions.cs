using System.Security.Claims;
using System.Threading.Tasks;
using ExtendingIdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace ExtendingIdentityDemo.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static async Task<string> GetUserDisplayNameAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var currentUser = await userManager.GetUserAsync(user);

            return currentUser.DisplayName;
        }
    }
}
