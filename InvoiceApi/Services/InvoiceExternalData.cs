using System.Dynamic;
using DbLib.Database;
using InvoiceApi.Data;
using InvoiceApi.Models;
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

        string jsonUpdatedInvoices = JsonConvert.SerializeObject(updatedInvoices);
        List<InvoiceContent> listInvoicesContent = JsonConvert.DeserializeObject<List<InvoiceContent>>(jsonUpdatedInvoices);

        this.InsertProductsAndSales(listInvoicesContent);

        return true;
    }

    /// <summary>
    /// Insert products and sales of the invoices.
    /// </summary>
    /// <param name="listInvoicesContent">List with invoices content.</param>
    public void InsertProductsAndSales(List<InvoiceContent> listInvoicesContent)
    {
        IConnectionCommandsFactory connFactory = new ConnectionCommandsFactory();
        using IConnectionCommands connCommands = connFactory.Create();
        bool productsSuccess = true;
        bool salesSuccess = true;
        Log.Debug("Begin transaction.");
        connCommands.BeginTransaction();

        ProductRepository productRepository = new ProductRepository(connCommands);
        SalesRepository salesRepository = new SalesRepository(connCommands);

        foreach (InvoiceContent invoiceContent in listInvoicesContent)
        {
            productsSuccess &= productRepository.Insert(invoiceContent);
            if (!productsSuccess)
            {
                Log.Error("Error when trying to insert a product.");
                break;
            }

            salesSuccess &= salesRepository.Insert(invoiceContent);
            if (!salesSuccess)
            {
                Log.Error("Error when trying to insert a sale.");
                break;
            }
        }

        if (productsSuccess && salesSuccess)
        {
            Log.Debug("Commit transaction.");
            connCommands.CommitTransaction();
        }
        else
        {
            Log.Debug("Rollback transaction.");
            connCommands.RollbackTransaction();
        }

        connCommands.DisposeTransaction();
    }

    /// <summary>
    /// Evaluate the invoice Taxes.
    /// </summary>
    /// <param name="invoices">original invoices.</param>
    /// <returns>Altered list with invoices.</returns>
    public JArray EvaluateTax(JArray invoices)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
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
