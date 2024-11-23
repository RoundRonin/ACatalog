using Microsoft.Extensions.DependencyInjection;
using DAL;
using DAL.Entities;
using DAL.Infrastructure;
using BLL;
using BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.OpenApi.Models;

namespace ArticleCatalog
{
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

                // Configure EF Core and PostgreSQL
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString));

                // Configure database repositories
                services.AddScoped<IRepository<Store>, StoreRepository>();
                services.AddScoped<IRepository<Product>, ProductRepository>();
                services.AddScoped<IInventoryRepository, InventoryRepository>();

                // Configure services
            }
            else if (repositoryType == "File")
            {
                var storeFilePath = Configuration.GetValue<string>("StoreFilePath");
                var productFilePath = Configuration.GetValue<string>("ProductFilePath");

                // Configure file-based repositories
                services.AddScoped<IRepository<Store>>(sp => new FileStoreRepository(storeFilePath));
                services.AddScoped<IRepository<Product>>(sp => new FileProductRepository(productFilePath));
                services.AddScoped<IInventoryRepository>(sp => new FileInventoryRepository(productFilePath));
            }
            else
            {
                throw new InvalidOperationException("Invalid repository type configured. Please choose either 'Database' or 'File'.");
            }
            
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInventoryService, InventoryService>();


            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
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
}
