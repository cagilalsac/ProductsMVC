using Microsoft.AspNetCore.Http;

namespace CORE.APP.Services.HTTP
{
    /// <summary>
    /// Provides a concrete implementation of <see cref="HttpServiceBase"/> for HTTP operations.
    /// This class can be extended to add custom HTTP-related service methods as needed.
    /// </summary>
    public class HttpService : HttpServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class with the specified HTTP context accessor and HTTP client factory.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        /// <param name="httpClientFactory">Factory for creating <see cref="System.Net.Http.HttpClient"/> instances.</param>
        public HttpService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
            : base(httpContextAccessor, httpClientFactory)
        {
        }
    }
}
