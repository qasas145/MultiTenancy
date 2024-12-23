

namespace MultiTenancy.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ITenantService _tenantService;
    private string TenantId{get;set;}


    public ApplicationDbContext(ITenantService tenantService) {
        this._tenantService = tenantService;
        TenantId = _tenantService.GetCurrentTenant()?.TId;

    }
    public DbSet<Product> Products{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString)) {
            var dbProvider = _tenantService.GetDatabaseProvider();
            if (dbProvider?.ToLower() == "mssql") {
                optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasQueryFilter(p=>p.TenantId == TenantId);
        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e=>e.State == EntityState.Added))
        {
            entry.Entity.TenantId = TenantId;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
