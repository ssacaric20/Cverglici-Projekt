using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartMenza.Business.Security.Services;
using SmartMenza.Business.Security.Services.Interfaces;
using SmartMenza.Business.Services;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Settings;
using SmartMenza.Data.Data;
using SmartMenza.Data.Repositories.Implementations;
using SmartMenza.Data.Repositories.Interfaces;




namespace SmartMenza.Business.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

            // Postavke
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            services.Configure<AzureStorageSettings>(config.GetSection("AzureStorage"));

            // Azure Blob
            services.AddSingleton<BlobServiceClient?>(sp =>
            {
                var s = sp.GetRequiredService<IOptions<AzureStorageSettings>>().Value;

                if (string.IsNullOrWhiteSpace(s.ConnectionString))
                    return null;

                return new BlobServiceClient(s.ConnectionString);
            });

            // Repozitoriji
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IDailyMenuRepository, DailyMenuRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<INutritionGoalRepository, NutritionGoalRepository>();
            services.AddScoped<IDailyFoodIntakeRepository, DailyFoodIntakeRepository>();


            // Servisi
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IDailyMenuService, DailyMenuService>();
            services.AddScoped<IImageService, AzureBlobImageService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IFoodAnalyzer, RuleBasedFoodAnalyzerService>();
            services.AddScoped<INutritionGoalService, NutritionGoalService>();
            services.AddScoped<INutritionGoalStatisticsService, NutritionGoalStatisticsService>();
            services.AddScoped<IDailyFoodIntakeService, DailyFoodIntakeService>();

            return services;
        }
    }
}
