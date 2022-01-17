namespace InvoiceApi.Models;

/// <summary>
/// Emit class.
/// </summary>
public class Emit
{
    /// <summary>
    /// Gets or sets the cnpj.
    /// </summary>
    /// <value>Cnpj.</value>
    public string CNPJ { get; set; }

    /// <summary>
    /// Gets or sets the XNome.
    /// </summary>
    /// <value>XNome.</value>
    public string XNome { get; set; }

    /// <summary>
    /// Gets or sets the Xfant.
    /// </summary>
    /// <value>Xfant.</value>
    public string XFant { get; set; }

    /// <summary>
    /// Gets or sets the EnderEmit.
    /// </summary>
    /// <value>EnderEmit.</value>
    public EnderEmit EnderEmit { get; set; }

    /// <summary>
    /// Gets or sets the IE.
    /// </summary>
    /// <value>IE.</value>
    public string IE { get; set; }

    /// <summary>
    /// Gets or sets the CRT.
    /// </summary>
    /// <value>CRT.</value>
    public string CRT { get; set; }
}
