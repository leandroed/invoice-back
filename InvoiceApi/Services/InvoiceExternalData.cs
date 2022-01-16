using System.Dynamic;
using InvoiceApi.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace InvoiceApi.Services;

/// <summary>
/// Class to manipulate the invoices data.
/// </summary>
public class InvoiceExternalData
{
    private readonly VendorData vendorData;
    private readonly InvoiceRepository invoiceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorData"/> class.
    /// </summary>
    public InvoiceExternalData()
    {
        this.vendorData = new VendorData();
        this.invoiceRepository = new InvoiceRepository();
    }

    /// <summary>
    /// Receive invoices and persist.
    /// </summary>
    public bool ReceiveInvoices()
    {
        string content = this.vendorData.ReceiveData();

        if (string.IsNullOrEmpty(content))
        {
            Log.Information("Received a empty list with invoices in request.");
            return true;
        }

        JArray originalInvoices = JsonConvert.DeserializeObject<JArray>(content);
        if (!this.invoiceRepository.InsertOriginalContent(originalInvoices))
        {
            Log.Error("Error when trying to insert original invoice content.");
            return false;
        }

        JArray updatedInvoices = this.EvaluateTax(originalInvoices);
        if (!this.invoiceRepository.AddAlteredData(updatedInvoices))
        {
            Log.Error("Error when trying to insert altered invoice content.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Evaluate the invoice Taxes.
    /// </summary>
    /// <param name="invoices">original invoices.</param>
    /// <returns>Altered list with invoices.</returns>
    public JArray EvaluateTax(JArray invoices)
    {
        decimal tax = 0.15m;
        JArray result = new JArray();

        if (invoices == null)
        {
            Log.Information("Receive a empty list with invoices.");
            return result;
        }

        foreach (JObject invoice in invoices)
        {
            dynamic invoiceContent = JsonConvert.DeserializeObject<ExpandoObject>(invoice.ToString());

            foreach (var prdObj in invoiceContent.nfeProc.NFe.infNFe.det)
            {
                if (decimal.TryParse(prdObj.prod.preco, out decimal price))
                {
                    prdObj.prod.imposto = price * tax;
                }
                else
                {
                    prdObj.prod.imposto = 0;
                }
            }

            string altered = JsonConvert.SerializeObject(invoiceContent);
            result.Add(JsonConvert.DeserializeObject<JObject>(altered));
        }

        return result;
    }
}
