using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DAL.Infrastructure;
using DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ACatalog.Tests;
public class TestStartup
{
    public IConfiguration Configuration { get; }

    public TestStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
        });

        // Seed the in-memory database
        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            SeedTestData(context);
        }
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        context.Stores.Add(new Store { Id = "1", Name = "Test Store 1" });
        context.Stores.Add(new Store { Id = "2", Name = "Test Store 2" });
        context.SaveChanges();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
