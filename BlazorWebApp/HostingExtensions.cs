using BlazorWebApp.Plumbing;
using BlazorWebApp.Services;
using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;

namespace BlazorWebApp;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
            .AddCookie("cookie", options =>
            {
                options.Cookie.Name = "__Host-blazor";
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.EventsType = typeof(CookieEvents);
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://demo.duendesoftware.com";

                // confidential client using code flow + PKCE
                options.ClientId = "interactive.confidential.short";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.ResponseMode = "query";

                options.MapInboundClaims = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                // request scopes + refresh tokens
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("api");
                options.Scope.Add("offline_access");

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";

                options.EventsType = typeof(OidcEvents);
            });

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        // adds access token management
        builder.Services.AddOpenIdConnectAccessTokenManagement();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        // register events to customize authentication handlers
        builder.Services.AddTransient<CookieEvents>();
        builder.Services.AddTransient<OidcEvents>();

        // not allowed to programmatically use HttpContext in Blazor Server.
        // that's why tokens cannot be managed in the login session
        builder.Services.AddSingleton<IUserTokenStore, ServerSideTokenStore>();

        // registers HTTP client that uses the managed user access token
        builder.Services.AddTransient<RemoteApiService>();
        builder.Services.AddHttpClient<RemoteApiService>(client =>
        {
            client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
        });

        builder.Services.AddControllersWithViews();
        
        builder.Services.AddSingleton<WeatherForecastService>();
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultControllerRoute();
        
        return app;
    }
}