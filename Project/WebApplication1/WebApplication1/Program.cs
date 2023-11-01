using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.DataAccess.Implementations;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.Errors;
using WebApplication1.Extensions;
using WebApplication1.Middleware;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddAplicationServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionMiddlewere>();

        app.UseStatusCodePagesWithReExecute("/errors/{0}");



            app.UseSwagger();
            app.UseSwaggerUI();


        app.UseStaticFiles();

        app.UseAuthorization();
        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<StoreContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(context);
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "An error occured during migration");
        }

        app.Run();
    }
}