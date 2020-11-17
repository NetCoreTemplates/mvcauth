# mvcauth

.NET 5.0 MVC Website integrated with ServiceStack Auth

![](https://raw.githubusercontent.com/ServiceStack/Assets/master/csharp-templates/mvcauth.png)

> Login

![](https://raw.githubusercontent.com/ServiceStack/Assets/master/csharp-templates/mvcauth-login.png)

> Browse [source code](https://github.com/NetCoreTemplates/mvcauth) and install with the `web` dotnet tool:

    $ dotnet tool install -g x

    $ x new mvcauth ProjectName

### OAuth Setup

Replace the `oauth.*` App settings with your own in `appsettings.Development.json` for local development and `appsettings.json` for production deployments.

 - Twitter - [Create Twitter App](https://dev.twitter.com/apps) with `{BaseUrl}/auth/twitter` referrer
 - Facebook - [Create Facebook App](https://developers.facebook.com/apps) with `{BaseUrl}/auth/facebook` referrer
 - Google - [Create Google App](https://console.developers.google.com/apis/credentials) with `{BaseUrl}/auth/google` referrer
 - Microsoft - [Create Microsoft App](https://apps.dev.microsoft.com) with `{BaseUrl}/auth/microsoft` referrer
