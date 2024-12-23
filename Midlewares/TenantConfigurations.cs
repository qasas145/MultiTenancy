namespace MultiTenancy.Midlewares;

public class TenantConfigurations 
{
    private readonly RequestDelegate _next;
    public TenantConfigurations(RequestDelegate next) {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context) {
        Console.WriteLine("before the request being processed");
        await _next(context);
        Console.WriteLine("after returnting with response");
    }
}
