namespace MultiTenancy.Models;

public class Product : IMustHaveTenant
{
    public int Id{get;set;}
    public string Name{get;set;} = null!;
    public string Descritpion {get;set;} = null!;
    public int Rate{get;set;}
    public string? TenantId {get;set;}

}
