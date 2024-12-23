namespace MultiTenancy.Settings;

public class TenantSettings
{
    public Defaults Defaults{get;set;}
    public List<Tenant> Tenants{get;set;}=new();
}
