namespace InvoiceApi.Models;

/// <summary>
/// Sale model.
/// </summary>
public class Sale
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>id.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the Cdprod.
    /// </summary>
    /// <value>Cdprod.</value>
    public string Cdprod { get; set; }

    /// <summary>
    /// Gets or sets the Price.
    /// </summary>
    /// <value>Price.</value>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the Ucom.
    /// </summary>
    /// <value>Ucom.</value>
    public string Ucom { get; set; }

    /// <summary>
    /// Gets or sets the Vuncom.
    /// </summary>
    /// <value>Vuncom.</value>
    public decimal Vuncom { get; set; }

    /// <summary>
    /// Gets or sets the Vprod.
    /// </summary>
    /// <value>Vprod.</value>
    public decimal Vprod { get; set; }

    /// <summary>
    /// Gets or sets the Utrib.
    /// </summary>
    /// <value>Utrib.</value>
    public string Utrib { get; set; }

    /// <summary>
    /// Gets or sets the Qtrib.
    /// </summary>
    /// <value>Qtrib.</value>
    public decimal Qtrib { get; set; }

    /// <summary>
    /// Gets or sets the Vuntrib.
    /// </summary>
    /// <value>Vuntrib.</value>
    public decimal Vuntrib { get; set; }

    /// <summary>
    /// Gets or sets the Tax.
    /// </summary>
    /// <value>Tax.</value>
    public decimal Tax { get; set; }

    /// <summary>
    /// Gets or sets the Dtemi.
    /// </summary>
    /// <value>Dtemi.</value>
    public DateTimeOffset Dtemi { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>Product.</value>
    public string Product { get; set; }

    /// <summary>
    /// Gets or sets the brand.
    /// </summary>
    /// <value>Brand.</value>
    public string Brand { get; set; }

    /// <summary>
    /// Gets or sets Dhemi.
    /// </summary>
    /// <value>Dhemi.</value>
    public DateTime Dhemi { get; set; }
}
