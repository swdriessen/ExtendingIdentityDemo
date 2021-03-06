﻿using System;
using System.Collections.Generic;
using System.Text;
using ExtendingIdentityDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExtendingIdentityDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<ExtendingIdentityDemo.Models.UserProfile> UserProfile { get; set; }
    }
}
