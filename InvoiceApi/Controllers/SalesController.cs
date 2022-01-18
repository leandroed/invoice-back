using InvoiceApi.Data;
using InvoiceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.Controllers;

/// <summary>
/// Sales controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly SalesRepository salesRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesController"/> class.
    /// </summary>
    /// <param name="salesRepository">Sales repository.</param>
    public SalesController(SalesRepository salesRepository)
    {
        this.salesRepository = salesRepository;
    }

    /// <summary>
    /// Get the sales by brand.
    /// </summary>
    /// <returns>Ok brands | 500 error.</returns>
    [HttpGet("GetSalesByBrand")]
    public IActionResult GetSalesByBrand()
    {
        List<SaleByBrand> salesByBrands = this.salesRepository.ListSalesByBrand();
        return this.Ok(new { salesByBrands = salesByBrands });
    }

    /// <summary>
    /// Gets the sales taxes with emit date.
    /// </summary>
    /// <returns>Sales taxes with emit date.</returns>
    [HttpGet("GetSalesTaxByEmitDate")]
    public IActionResult GetSalesTaxByEmitDate()
    {
        List<Sale> sales = this.salesRepository.ListSales();
        return this.Ok(new { sales = sales });
    }
}
