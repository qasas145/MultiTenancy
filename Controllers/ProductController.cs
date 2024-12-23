
namespace MultiTenancy.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service) {
        _service = service;
    }
    public async Task<IActionResult> CreateProduct(Product product) {
        await _service.CreateAsync(product);
        return Ok(product);
    }
    public async Task<IActionResult> GetProductById(int id) {
        var product = await _service.GetByIdAsync(id);
        if (product is Product)
            return Ok(product);
        return NotFound();
    }
    public async Task<IActionResult> GetAllProducts() {
        return Ok(await _service.GetAllAsync());
    }
}
