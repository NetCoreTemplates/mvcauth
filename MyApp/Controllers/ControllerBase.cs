using System;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Mvc;

namespace MyApp.Controllers
{
    public abstract class ControllerBase : ServiceStackController
    {
        /// <summary>
        /// Accessible from both /Home/Login and /Account/Login pages 
        /// </summary>
        [HttpPost]
        public ActionResult Login(string userName, string password, string redirect = null)
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
                            RememberMe = true,
                        });
                    }

                    return Redirect(string.IsNullOrEmpty(redirect) ? "/" : redirect);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("Index", SessionAs<AuthUserSession>());
        }
    }
}