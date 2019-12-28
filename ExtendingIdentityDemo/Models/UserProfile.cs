using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtendingIdentityDemo.Models
{
    public class UserProfile
    {
        public UserProfile()
        {

        }

        public string Location { get; set; }
        public string Information { get; set; }






        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
