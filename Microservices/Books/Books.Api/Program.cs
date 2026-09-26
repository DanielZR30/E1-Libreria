using Books.Application;
using Books.Persistence;
using Books.Persistence.Seeds;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Biblioteca API - Catálogo de Libros (Seguimiento 1)",
        Version = "v1",
        Description = "API desarrollada bajo Clean Architecture, DDD, CQRS y EF Core para consultar el catálogo de libros, autores y categorías."
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI clásico
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
        c.RoutePrefix = "swagger";
    });

    // Interfaz moderna y estilizada: Scalar API Reference
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Catálogo de Libros - Seguimiento 1")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    // Redireccionar raíz al dashboard moderno
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

try
{
    await DataBaseSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "No se pudo ejecutar el seeder en el arranque.");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
