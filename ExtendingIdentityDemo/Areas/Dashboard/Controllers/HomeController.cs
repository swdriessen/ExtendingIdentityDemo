using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtendingIdentityDemo.Areas.Dashboard.Controllers
{


    [Authorize(PermissionNames.Dashboards.View)]
    [Authorize(Roles="Admin")]
    [Area("Dashboard")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
