

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<ApplicationDbContext>();


// 
TenantSettings options = new();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);
foreach (var tenant in options.Tenants)
{
    var connectionString =  tenant.ConnectionString ?? options.Defaults.DefaultConnectionString;

    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.SetConnectionString(connectionString);
    Console.WriteLine("connection string of {0} is {1}", tenant.Name, connectionString);
    if (dbContext.Database.GetPendingMigrations().Any()){
        dbContext.Database.Migrate();
    }
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
