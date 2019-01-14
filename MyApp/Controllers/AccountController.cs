using System;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Mvc;

namespace MyApp.Controllers
{
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(SessionAs<AuthUserSession>());
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password, bool autoLogin, string redirect = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var authService = ResolveService<RegisterService>())
                    {
                        var response = authService.Post(new Register
                        {
                            Email = email,
                            FirstName = firstName,
                            LastName = lastName,
                            DisplayName = $"{firstName} {lastName}",
                            Password = password,
                            AutoLogin = autoLogin,                            
                        });
                    }

                    return Redirect(string.IsNullOrEmpty(redirect) ? "/" : redirect);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }
    }
}