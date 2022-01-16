using InvoiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.Controllers;

/// <summary>
/// Invoices process controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class InvoicesProcessController : ControllerBase
{
    private readonly InvoiceExternalData invoiceExternalData;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoicesProcessController"/> class.
    /// </summary>
    /// <param name="invoiceExternalData">Invoice extenal data services.</param>
    public InvoicesProcessController(InvoiceExternalData invoiceExternalData)
    {
        this.invoiceExternalData = invoiceExternalData;
    }

    /// <summary>
    /// Receive and process external invoices.
    /// </summary>
    /// <returns>Ok success | 500 error.</returns>
    [HttpGet(Name = "GetProcessNewInvoices")]
    public IActionResult Get()
    {
        if (!this.invoiceExternalData.ReceiveInvoices())
        {
            return this.StatusCode(500);
        }

        return this.Ok(new { response = "Notas recebidas e processadas com sucesso!" });
    }
}
