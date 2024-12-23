namespace MultiTenancy.Configurations;

public static class ConfigureServices 
{
    public static IServiceCollection AddTenancy(this IServiceCollection services, 
    ConfigurationManager configuration) {
     
        
        services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));
        services.AddScoped<ITenantService, TenantService>();

        TenantSettings options = new();
        configuration.GetSection(nameof(TenantSettings)).Bind(options);

        foreach (var tenant in options.Tenants)
        {
            var connectionString =  tenant.ConnectionString ?? options.Defaults.DefaultConnectionString;

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetConnectionString(connectionString);

            // print statement to know where we are
            Console.WriteLine("connection string of {0} is {1}", tenant.Name, connectionString);

            if (dbContext.Database.GetPendingMigrations().Any()){
                dbContext.Database.Migrate();
            }
        }
        return services;
    }
}
