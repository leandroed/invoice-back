using InvoiceApi.Protocols;
using InvoiceApi.Vendor;
using Serilog;

namespace InvoiceApi.Data;

/// <summary>
/// Vendor data class.
/// </summary>
public class VendorData
{
    private readonly string vendorUrl;
    private readonly IHttpCliFactory httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorData"/> class.
    /// </summary>
    public VendorData()
    {
        this.httpClientFactory = new HttpClientFactory();
        this.vendorUrl = this.VendorUrl;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorData"/> class.
    /// </summary>
    /// <param name="vendorUrl">Vendor url.</param>
    /// <param name="httpClientFactory">Http client factory.</param>
    public VendorData(string vendorUrl, IHttpCliFactory httpClientFactory)
    {
        this.vendorUrl = vendorUrl;
        this.httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Gets the vendor url in environment variables.
    /// </summary>
    /// <value>Vendor url.</value>
    public string VendorUrl
    {
        get
        {
            try
            {
                return Environment.GetEnvironmentVariable("VENDOR_URL") ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}.");
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Gets the vendor data.
    /// </summary>
    /// <returns>Invoices data.</returns>
    public string ReceiveData()
    {
        string result = string.Empty;

        if (string.IsNullOrEmpty(this.vendorUrl))
        {
            Log.Error("Error it's not possible get data, the url is invalid.");
            return result;
        }

        try
        {
            bool successResponse = false;
            Uri url = new Uri(this.vendorUrl);

            using HttpClient client = this.httpClientFactory.Create();
            HttpResponseMessage httpRequest = client.GetAsync(url).Result;
            result = httpRequest.Content.ReadAsStringAsync().Result;
            successResponse = httpRequest.IsSuccessStatusCode;

            if (!successResponse)
            {
                Log.Error("Error when trying to get the invoice content.");
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error on get json with invoices. {ex.Message}");
            return string.Empty;
        }

        return result;
    }
}
