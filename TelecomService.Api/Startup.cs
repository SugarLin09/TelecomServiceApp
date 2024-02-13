
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TelecomService.Infrastructure.Data;
using TelecomService.Application;
using TelecomService.Infrastructure;

namespace TelecomService.Api
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
            services.AddRouting(options => options.LowercaseUrls = true)
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Telecom Service API",
                    Version = "v1",
                    Description = "API for Telecom Service",
                });
            })
            .AddApplication()
            .AddInfrastructure();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));


            // Add CORS services
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder.WithOrigins("https://jolly-sea-0259a8c00.4.azurestaticapps.net") 
                           .AllowAnyHeader()
                           .AllowAnyMethod());
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection()
            .UseSwagger();
            
            app.UseDeveloperExceptionPage()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Telecome Service API V1");
                c.RoutePrefix = "swagger";
            });

            // Use CORS policy
            app.UseCors("AllowSpecificOrigin");

            app.UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
