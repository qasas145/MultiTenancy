
using Microsoft.Extensions.Options;

namespace MultiTenancy.Services;

public class TenantService : ITenantService
{
    private Tenant _currentTenant;
    private HttpContext _httpContext;
    private TenantSettings _tenantSettings;
    public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor) {
        
        _tenantSettings = tenantSettings.Value;
        _httpContext = contextAccessor.HttpContext;

        if (_httpContext is not null) {

            if (_httpContext.Request.Headers.TryGetValue("tenant", out  var tenantId)) 
                SetCurrentTenant(tenantId);
            else 
                throw new Exception("no tenant provided");
        }

    }
    public string? GetConnectionString()
    {
        return _currentTenant is null ? _tenantSettings.Defaults.DefaultConnectionString : _currentTenant.ConnectionString;
    }

    public Tenant? GetCurrentTenant()
    {
        return _currentTenant;
    }

    public string? GetDatabaseProvider()
    {
        return _tenantSettings.Defaults.DBProvider;
    }
    private void SetCurrentTenant(string tenantId) {
        
        _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t=>t.TId == tenantId);
        if (_currentTenant is not Tenant)
            throw new Exception("invalid tenant id");
        if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            _currentTenant.ConnectionString = _tenantSettings.Defaults.DefaultConnectionString;
    }

}
