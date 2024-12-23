
namespace MultiTenancy.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service) {
        _service = service;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(Product product) {
        await _service.CreateAsync(product);
        return Ok(product);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id) {
        var product = await _service.GetByIdAsync(id);
        if (product is Product)
            return Ok(product);
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProducts() {
        return Ok(await _service.GetAllAsync());
    }
}
