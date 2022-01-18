using InvoiceApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.Controllers;

/// <summary>
/// Brands controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class BrandController : ControllerBase
{
    private readonly ProductRepository productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandController"/> class.
    /// </summary>
    /// <param name="productRepository">Product repository.</param>
    public BrandController(ProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    /// <summary>
    /// Get the available brands.
    /// </summary>
    /// <returns>Ok brands | 500 error.</returns>
    [HttpGet(Name = "GetBrands")]
    public IActionResult Get()
    {
        List<string> brands = this.productRepository.GetBrands();
        return this.Ok(new { brands = brands });
    }
}
