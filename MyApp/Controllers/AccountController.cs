using System;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Mvc;

namespace MyApp.Controllers
{
    public class AccountController : ServiceStackController
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect(Request.Query.TryGetValue("ReturnUrl", out var redirect) ? redirect.ToString() : "/");

            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
 
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}