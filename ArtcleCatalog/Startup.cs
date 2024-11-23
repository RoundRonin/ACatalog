using Microsoft.Extensions.DependencyInjection;
using DAL.Entities;
using DAL.Infrastructure;
using BLL.Infrastructure;

namespace ArticleCatalog;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Read repository type from configuration
        var repositoryType = Configuration.GetValue<string>("RepositoryType");

        if (repositoryType == "Database")
        {
            // Validate database connection string
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not provided or is invalid.");
            }

            // Configure EF Core and database repositories
            services.AddScoped<IRepository<Store>, StoreRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Inventory>, InventoryRepository>();
        }
        else if (repositoryType == "File")
        {
            // Configure file-based repositories
            services.AddScoped<IRepository<Store>, FileStoreRepository>();
            services.AddScoped<IRepository<Product>, FileProductRepository>();
            services.AddScoped<IRepository<Inventory>, FileInventoryRepository>();
        }
        else
        {
            throw new InvalidOperationException("Invalid repository type configured. Please choose either 'Database' or 'File'.");
        }

        // Service configuration
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInventoryService, InventoryService>();

        services.AddSwaggerGen();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Add Swagger for API documentation
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
        });
    }
}
