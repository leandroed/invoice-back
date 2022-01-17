namespace InvoiceApi.Models;

/// <summary>
/// Dest class.
/// </summary>
public class Dest
{
    /// <summary>
    /// Gets or sets the cpf.
    /// </summary>
    /// <value>CPF.</value>
    public string CPF { get; set; }

    /// <summary>
    /// Gets or sets the xnome.
    /// </summary>
    /// <value>Xnome</value>
    public string XNome { get; set; }

    /// <summary>
    /// Gets or sets the destiny address.
    /// </summary>
    /// <value>Destiny address.</value>
    public EnderDest EnderDest { get; set; }

    /// <summary>
    /// Gets or sets the IndIEDest.
    /// </summary>
    /// <value>IndIEDest</value>
    public string IndIEDest { get; set; }
}
