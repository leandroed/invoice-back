namespace InvoiceApi.Models;

/// <summary>
/// InvoiceContent model class.
/// </summary>
public class InvoiceContent
{
    /// <summary>
    /// Gets or sets the invoice id.
    /// </summary>
    /// <value>Invoice Id.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets NFe.
    /// </summary>
    /// <value>NFe.</value>
    public NfeProc NfeProc { get; set; }
}
