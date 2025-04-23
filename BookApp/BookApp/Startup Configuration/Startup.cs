using BookApp.Data.EF.Access.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Startup_Configuration;

public class Startup
{
    public IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), action =>
                    {
                        action.CommandTimeout(30);
                    })
#if DEBUG
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
#endif
        );
        
        services.AddMvc(options => options.EnableEndpointRouting = false);

        services
            .AddServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        
        app.UseStatusCodePages();
        
        app.UseRouting();

        app.UseAuthorization();

        app.UseMvc(routes =>
        {
            routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
        });
    } 
}