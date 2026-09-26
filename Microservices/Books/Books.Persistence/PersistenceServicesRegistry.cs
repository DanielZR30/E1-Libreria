using Books.Application.Contracts.Persistence;
using Books.Application.Contracts.Repositories;
using Books.Persistence.Repositories;
using Books.Persistence.Seeds;
using Books.Persistence.Seeds.Authors;
using Books.Persistence.Seeds.Books;
using Books.Persistence.Seeds.Categories;
using Books.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("MyConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = configuration["DB_CONNECTION_STRING"]
                        ?? Environment.GetEnvironmentVariable("ConnectionStrings__MyConnection")
                        ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                }

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var server = configuration["DB_SERVER"] ?? Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost,1433";
                    var database = configuration["DB_NAME"] ?? Environment.GetEnvironmentVariable("DB_NAME") ?? "LibraryDb";
                    var user = configuration["DB_USER"] ?? Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
                    var password = configuration["MSSQL_SA_PASSWORD"]
                        ?? configuration["DB_PASSWORD"]
                        ?? Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")
                        ?? Environment.GetEnvironmentVariable("DB_PASSWORD");

                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        connectionString = $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate=True;MultipleActiveResultSets=True";
                    }
                }

                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IBooksRepository, BooksRepository>();

            // Seeders
            services.AddScoped<IDataSeeder, AuthorSeeder>();
            services.AddScoped<IDataSeeder, CategorySeeder>();
            services.AddScoped<IDataSeeder, BookSeeder>();

            return services;
        }
    }
}
