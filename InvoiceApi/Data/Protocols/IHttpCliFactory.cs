namespace InvoiceApi.Protocols;

/// <summary>
/// Http client factory.
/// </summary>
public interface IHttpCliFactory
{
    /// <summary>
    /// Creates a http client instance.
    /// </summary>
    /// <returns>Http client.</returns>
    HttpClient Create();
}
