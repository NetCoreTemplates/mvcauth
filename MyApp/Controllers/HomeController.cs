using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Mvc;

namespace MyApp.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return View(SessionAs<AuthUserSession>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult RequiresAuth()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        public IActionResult RequiresRole()
        {
            return View();
        }
    }
}
