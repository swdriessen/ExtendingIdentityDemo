using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ExtendingIdentityDemo.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string DisplayName { get; set; }
    }
}
