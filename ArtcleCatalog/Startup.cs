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
using DotNetEnv;
using ACatalog.Middleware;

namespace ACatalog
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string repositoryType = Configuration.GetValue<string>("RepositoryType");

            if (repositoryType == "Database")
            {
                ConfigureDatabaseServices(services);
            }
            else if (repositoryType == "File")
            {
                ConfigureFileServices(services);
            }
            else
            {
                throw new InvalidOperationException("Invalid repository type configured. Please choose either 'Database' or 'File'.");
            }

            ConfigureCommonServices(services);
        }

        private void ConfigureDatabaseServices(IServiceCollection services)
        {
            // Get .env from the root of the project
            var rootEnvPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, ".env");
            Env.Load(rootEnvPath);
            // Construct the connection string from environment variables
            var connectionString = $"Host={Environment.GetEnvironmentVariable("HOST")};" +
                                   $"Port={Environment.GetEnvironmentVariable("PORT")};" +
                                   $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" +
                                   $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};" +
                                   $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

            // Configure EF Core and PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<DbContext, ApplicationDbContext>();

            // Configure database repositories
            services.AddScoped<IRepository<Store>, StoreRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Inventory>, InventoryRepository>();

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        private void ConfigureFileServices(IServiceCollection services)
        {
            var storeFilePath = Configuration.GetValue<string>("StoreFilePath");
            var productFilePath = Configuration.GetValue<string>("ProductFilePath");

            if (storeFilePath == null || productFilePath == null)
            {
                throw new Exception("Configuration file is not set");
            }

            // Configure file-based repositories
            var fileStoreRepository = new FileStoreRepository(storeFilePath);
            var fileProductRepository = new FileProductRepository(productFilePath);
            var fileInventoryRepository = new FileInventoryRepository(productFilePath, fileProductRepository);

            services.AddScoped<IRepository<Store>>(sp => fileStoreRepository);
            services.AddScoped<IRepository<Product>>(sp => fileProductRepository);
            services.AddScoped<IRepository<Inventory>>(sp => fileInventoryRepository);

            services.AddScoped<IStoreRepository>(sp => fileStoreRepository);
            services.AddScoped<IProductRepository>(sp => fileProductRepository);
            services.AddScoped<IInventoryRepository>(sp => fileInventoryRepository);
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            
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
