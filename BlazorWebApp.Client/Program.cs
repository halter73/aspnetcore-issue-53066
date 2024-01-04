using BlazorWebApp.Client;
using BlazorWebApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddTransient<RemoteApiService>();

builder.Services.AddHttpClient<RemoteApiService>(client =>
{
    client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
}).AddHttpMessageHandler<CookieHandler>();

//builder.Services
//    .AddTransient<CookieHandler>()
//    .AddScoped(sp => sp
//        .GetRequiredService<IHttpClientFactory>()
//        .CreateClient("API"))
//    .AddHttpClient("API", client => 
//        client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/"))
//    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
