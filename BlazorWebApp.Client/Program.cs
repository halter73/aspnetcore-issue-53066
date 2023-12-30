using BlazorWebApp.Client;
using BlazorWebApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

//builder.Services.AddTransient<CookieHandler>();
builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
builder.Services.AddTransient<RemoteApiService>();

builder.Services.AddHttpClient<RemoteApiService>(client =>
{
    client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));
//builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri("https://localhost:7078/"))
//    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
