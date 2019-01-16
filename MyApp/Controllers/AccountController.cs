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
            return View(SessionAs<CustomUserSession>());
        }
        
        [HttpPost]
        public ActionResult Login(string userName, string password, bool rememberMe, string redirect = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var authService = ResolveService<AuthenticateService>())
                    {
                        var response = authService.Authenticate(new Authenticate
                        {
                            provider = CredentialsAuthProvider.Name,
                            UserName = userName,
                            Password = password,
                            RememberMe = rememberMe,
                        });
                    }

                    return Redirect(string.IsNullOrEmpty(redirect) ? "/" : redirect);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("Index", SessionAs<CustomUserSession>());
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View(SessionAs<CustomUserSession>());
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
            return View(SessionAs<CustomUserSession>());
        }
 
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}