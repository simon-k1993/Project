using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataAccess.Implementations;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.DataAccess;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Errors;

namespace WebApplication1.Extensions
{
    public static class AplicationServicesExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           services.AddEndpointsApiExplorer();
           services.AddSwaggerGen();
           services.AddDbContext<StoreContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

           services.AddScoped<IProductRepository, ProductsRepository>();
           services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //services.AddCors(opt =>
            //{
            //    opt.AddPolicy("CorsPolicy", policy =>
            //    {
            //        policy.AllowAnyHeader().WithOrigins("https://localhost:4200");
            //    });
            //});
            

            return services;
        }
    }
}
