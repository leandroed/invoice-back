namespace InvoiceApi.Models;

/// <summary>
/// Nfeproc class.
/// </summary>
public class NfeProc
{
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>Version.</value>
    public string Versao { get; set; }

    /// <summary>
    /// Gets or sets the xmlns.
    /// </summary>
    /// <value>Xmls.</value>
    public string Xmlns { get; set; }

    /// <summary>
    /// Gets or sets the nfe.
    /// </summary>
    /// <value>Nfe.</value>
    public NFe NFe { get; set; }
}
