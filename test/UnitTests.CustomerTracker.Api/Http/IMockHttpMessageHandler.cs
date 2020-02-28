using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.CustomerTracker.Api.Http
{
    /// <summary>
    /// Inspired by: https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
    /// </summary>
    public interface IMockHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}