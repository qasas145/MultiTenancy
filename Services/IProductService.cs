namespace MultiTenancy.Services;

public interface IProductService
{
    Task<Product> CreateAsync(Product product);
    Task<Product> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Product>> GetAllAsync();
}
