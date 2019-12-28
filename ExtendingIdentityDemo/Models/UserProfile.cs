using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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





        [Key]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
