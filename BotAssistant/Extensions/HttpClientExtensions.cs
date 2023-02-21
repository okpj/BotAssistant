using Polly;

namespace BotAssistant.Extensions;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder AddPolicyHandlerFallback(this IHttpClientBuilder builder)
    {
        var fallbackAsync = Policy<HttpResponseMessage>
         .HandleResult(r => r.IsSuccessStatusCode is false)
         .FallbackAsync(FallbackAction, OnFallbackAsync);

        return builder.AddPolicyHandler(fallbackAsync);
    }

    private static async Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
    {
        var httpResult = response.Result;
        var content = await httpResult.Content.ReadAsStringAsync();
        Log.Error("ERROR Request - {@RequestUri}; StatusCode: {@StatusCode}; Response: {@Content}", httpResult.RequestMessage?.RequestUri,
            httpResult.StatusCode, content);
    }
    private static Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
    {
        return Task.FromResult(responseToFailedRequest.Result);
    }
}
