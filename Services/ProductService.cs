
namespace MultiTenancy.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context) {
        _context = context;
    }
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.SingleAsync(p=>p.Id == id);
    }

}
