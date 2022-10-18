using System.Diagnostics;
using Polly;
using Polly.Extensions.Http;
using Polly.Wrap;

namespace INSS.EIIR.Web.Configuration;

public class Policies
{
    private static int _timeOutInSeconds = 40;

    public static AsyncPolicyWrap<HttpResponseMessage> PolicyStrategy => Policy.WrapAsync(RetryPolicy, TimeoutPolicy);

    private static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy
    {
        get
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(_timeOutInSeconds, (context, timeSpan, task) =>
            {
                Debug.WriteLine($"[App|Policy]: Timeout delegate fired after {timeSpan.Seconds} seconds");
                return Task.CompletedTask;
            });
        }
    }

    private static IAsyncPolicy<HttpResponseMessage> RetryPolicy
    {
        get
        {
            var jitter = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitter.Next(0, 100)));
        }
    }
}