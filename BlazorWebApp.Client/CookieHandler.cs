using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace BlazorWebApp.Client;
public class CookieHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
        return await base.SendAsync(request, cancellationToken);
    }
}

//public class CookieHandler : DelegatingHandler
//{
//    private readonly IAccessTokenProvider tokenProvider;

//    public CookieHandler(IAccessTokenProvider tokenProvider)
//    {
//        this.tokenProvider = tokenProvider;
//    }

//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

//        var tokenResult = await tokenProvider.RequestAccessToken();

//        if (tokenResult.TryGetToken(out var token))
//        {
//            request.Headers.Authorization =
//                new AuthenticationHeaderValue("Bearer", token.Value);

//            //request.Content.Headers.TryAddWithoutValidation(
//            //    "x-custom-header", "value");

//            //var response = await Http.SendAsync(requestMessage);
//            //var responseStatusCode = response.StatusCode;

//            //responseBody = await response.Content.ReadAsStringAsync();
//        }

//        return await base.SendAsync(request, cancellationToken);
//    }
//}
