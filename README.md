# AdfsOAuth
ASP.NET Core middleware that enables an application to support the Microsoft ADFS's OAuth 2.0 authentication workflow.
OAuth2 protocol is supported since ADFS 3.0 (Windows Server 2012 R2).

## Client registration on the server
Create the relying party on "AD FS Management", and configure claims...

Then, you have to add your client app using powershell
```PowerShell
Import-Module ADFS
$relyingPartyName = "WebApp"
$appUri = "https://localhost:44303"
 
$clientId = [guid]::NewGuid()
$redirectUri = "$appUri/signin-adfs"
Add-AdfsClient -Name $relyingPartyName -ClientId $clientId -RedirectUri $redirectUri
Write-Host "Client Id: $clientId"
```

## Usage
In your startup.cs
```C#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    ...
    app.UseAdfsOAuthAuthentication(new AdfsOAuthOptions("ad.adtest.local")  // Your ADFS server hostname
    {
        ClientUri = "https://localhost:44303",                  // Registered on your ADFS server as Relying party trust
        ClientId = "21495a95-35ef-48ec-80b4-bce8ac430b9f"       // AdfsClient's ClientId registered on your ADFS server
    });
    ...
}

```

By default, the redirect URI is "[*ClientUri*]/signin-adfs". It can be changed in the options.

## A word of warning
As signing certificates are auto-renewed by default by ADFS server, and there is  - for now - no easy way to retrieve the signing certificate from the server, the token issued by the server is **not verfied**.

## References / alternatives
* [Using ADFS 3.0 with MVC 6 (ASP.NET 5)](http://www.carbon60.com/blog/using-adfs-3-0-with-mvc-6-asp-net-5)
* [Authenticating ASP.NET 5 to AD FS OAuth](https://vcsjones.com/2015/05/04/authenticating-asp-net-5-to-ad-fs-oauth/)