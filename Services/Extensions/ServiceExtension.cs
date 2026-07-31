using App.Repositories;
using App.Repositories.Products;
using App.Services.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Services.Categories;
using App.Services.ExceptionHandlers;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace App.Services.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddFluentValidationAutoValidation();

            // Register validators from this assembly (Service project) so FluentValidation can resolve IValidator<T>
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
