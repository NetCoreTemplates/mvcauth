using System.Collections.Generic;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;

namespace MyApp
{
    [Priority(-100)] // Run before ConfigureDb
    public class ConfigureAuth : IConfigureAppHost
    {
        public void Configure(IAppHost appHost)
        {
            // TODO: Replace OAuth App settings in: appsettings.Development.json
            var AppSettings = appHost.AppSettings;
            appHost.Plugins.Add(new AuthFeature(() => new CustomUserSession(), 
                new IAuthProvider[] {
                    new NetCoreIdentityAuthProvider(AppSettings) { // Adapter to enable ServiceStack Auth in MVC
                        AdminRoles = { "Manager" }, // Automatically Assign additional roles to Admin Users
                    },
                    new CredentialsAuthProvider(AppSettings),     // Sign In with Username / Password credentials 
                    new FacebookAuthProvider(AppSettings), /* Create Facebook App at: https://developers.facebook.com/apps */
                    new GoogleAuthProvider(AppSettings),   /* Create App https://console.developers.google.com/apis/credentials */
                    new MicrosoftGraphAuthProvider(AppSettings),   /* Create App https://apps.dev.microsoft.com */
                }));

            appHost.Plugins.Add(new RegistrationFeature()); //Enable /register Service

            //override the default registration validation with your own custom implementation
            appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();
        }
    }
    
    // Add any additional metadata properties you want to store in the Users Typed Session
    public class CustomUserSession : AuthUserSession
    {
    }
    
    // Custom Validator to add custom validators to built-in /register Service requiring DisplayName and ConfirmPassword
    public class CustomRegistrationValidator : RegistrationValidator
    {
        public CustomRegistrationValidator()
        {
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.ConfirmPassword).NotEmpty();
            });
        }
    }
}