namespace InvoiceApi.Models;

/// <summary>
/// InfNfe class.
/// </summary>
public class InfNFe
{
    /// <summary>
    /// Gets or sets the Id.
    /// </summary>
    /// <value>Id.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>Version.</value>
    public string Versao { get; set; }

    /// <summary>
    /// Gets or sets the Ide.
    /// </summary>
    /// <value>Ide.</value>
    public Ide Ide { get; set; }

    /// <summary>
    /// Gets or sets the emit.
    /// </summary>
    /// <value>Emit.</value>
    public Emit Emit { get; set; }

    /// <summary>
    /// Gets or sets the dest.
    /// </summary>
    /// <value>Dest.</value>
    public Dest Dest { get; set; }

    /// <summary>
    /// Gets or sets the det.
    /// </summary>
    /// <value>Det.</value>
    public List<Det> Det { get; set; }
}
