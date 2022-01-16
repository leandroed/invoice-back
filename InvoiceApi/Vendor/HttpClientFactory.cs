using InvoiceApi.Protocols;

namespace InvoiceApi.Vendor;

/// <summary>
/// Http Client factory class.
/// </summary>
public class HttpClientFactory : IHttpCliFactory
{
    /// <summary>
    /// Creates a http client instance.
    /// </summary>
    /// <returns>Http client.</returns>
    public HttpClient Create()
    {
        return new HttpClient();
    }
}
