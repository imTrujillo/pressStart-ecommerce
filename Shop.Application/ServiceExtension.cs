using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;
using Shop.Application.Interfaces.Services;
using Shop.Application.Services;
using Shop.Application.Validators;
using Shop.Application.Validators.Shopping;

namespace Shop.Application;

public static class ServiceExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Services Dependency Injection
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProviderService, ProviderService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        
        //Services Validators Injection
        services.AddScoped<IValidator<RegisterCustomer>, CustomerValidator>();
        services.AddScoped<IValidator<RegisterEmployee>, EmployeeValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
        services.AddScoped<IValidator<ResetPassword>, ResetPasswordValidator>();
        
        services.AddScoped<IValidator<CreateCategoryDto>, CategoryValidator>();
        services.AddScoped<IValidator<CreateProviderDto>,  ProviderValidator>();
        services.AddScoped<IValidator<OrderDetailDto>, OrderDetailValidator>();
        services.AddScoped<IValidator<CreateOrderDto>, OrderValidator>();
        services.AddScoped<IValidator<CreateProductDto>, ProductValidator>();
            
        return services;
    }
}