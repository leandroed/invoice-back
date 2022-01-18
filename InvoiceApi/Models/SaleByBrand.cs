namespace InvoiceApi.Models;

/// <summary>
/// Sale by brand model.
/// </summary>
public class SaleByBrand
{
    /// <summary>
    /// Gets or sets the sale id.
    /// </summary>
    /// <value>Sale id.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>Price.</value>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets brand.
    /// </summary>
    /// <value>Brand.</value>
    public string Brand { get; set; }
}
