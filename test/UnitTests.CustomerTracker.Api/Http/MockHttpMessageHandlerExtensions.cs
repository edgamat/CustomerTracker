using System.Net;
using System.Net.Http;
using System.Threading;
using Moq;

namespace UnitTests.CustomerTracker.Api.Http
{
    public static class MockHttpMessageHandlerExtensions
    {
        public static Mock<IHttpClientFactory> GetMockHttpClientFactory(this Mock<IMockHttpMessageHandler> mockHandler, string name)
        {
            var handler = new MockHttpMessageHandler(mockHandler.Object);
            var httpClient = new HttpClient(handler);

            var httpClientFactory = new Mock<IHttpClientFactory>();

            httpClientFactory
                .Setup(x => x.CreateClient(name))
                .Returns(httpClient);

            return httpClientFactory;
        }

        public static void AddStatusResponse(this Mock<IMockHttpMessageHandler> handler, string url, HttpMethod method, HttpStatusCode statusCode)
        {
            var request = new HttpRequestMessage(method, url);

            AddStatusResponse(handler, request, statusCode);
        }

        public static void AddStatusResponse(this Mock<IMockHttpMessageHandler> handler, HttpRequestMessage request, HttpStatusCode statusCode)
        {
            handler
                .Setup(x => x.SendAsync(
                    It.Is<HttpRequestMessage>(y => y.RequestUri == request.RequestUri && y.Method == request.Method),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => {
                    return new HttpResponseMessage(statusCode);
                });
        }

        public static void AddJsonResponse(this Mock<IMockHttpMessageHandler> handler, string url, HttpMethod method, object response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var request = new HttpRequestMessage(method, url);

            AddJsonResponse(handler, request, response, statusCode);
        }

        public static void AddJsonResponse<TRequest, TResponse>(this Mock<IMockHttpMessageHandler> handler, string url, HttpMethod method, TRequest requestData, TResponse responseData, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = new ObjectContent(
                    typeof(TResponse),
                    responseData,
                    new System.Net.Http.Formatting.JsonMediaTypeFormatter())
            };

            AddJsonResponse(handler, request, responseData, statusCode);
        }

        public static void AddJsonResponse(this Mock<IMockHttpMessageHandler> handler, HttpRequestMessage request, object response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var mockQuoteResponse = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent(
                    typeof(object),
                    response,
                    new System.Net.Http.Formatting.JsonMediaTypeFormatter())
            };

            handler
                .Setup(x => x.SendAsync(
                    It.Is<HttpRequestMessage>(y => y.RequestUri == request.RequestUri && y.Method == request.Method),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => mockQuoteResponse);
        }
    }
}