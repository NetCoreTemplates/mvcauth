# mvcauth

.NET Core 2.2 MVC Website integrated with ServiceStack Auth

[![](https://raw.githubusercontent.com/ServiceStack/Assets/master/csharp-templates/mvcauth.png)](http://mvcauth.web-templates.io/)

> Login

[![](https://raw.githubusercontent.com/ServiceStack/Assets/master/csharp-templates/mvcauth-login.png)](http://mvcauth.web-templates.io/)

> Browse [source code](https://github.com/NetCoreTemplates/mvcauth), view live demo [mvcauth.web-templates.io](http://mvcauth.web-templates.io) and install with the `web` dotnet tool:

    $ dotnet tool install -g web

    $ web new mvcidentity ProjectName

### OAuth Setup

Replace the `oauth.*` App settings with your own in `appsettings.Development.json` for local development and `appsettings.json` for production deployments.

 - Twitter - [Create Twitter App](https://dev.twitter.com/apps) with `{BaseUrl}/auth/twitter` referrer
 - Facebook - [Create Facebook App](https://developers.facebook.com/apps) with `{BaseUrl}/auth/facebook` referrer
 - Google - [Create Google App](https://console.developers.google.com/apis/credentials) with `{BaseUrl}/auth/google` referrer
 - Microsoft - [Create Microsoft App](https://apps.dev.microsoft.com) with `{BaseUrl}/auth/microsoft` referrer
