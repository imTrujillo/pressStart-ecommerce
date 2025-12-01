using Shop.Application.DTOs.Request.Shopping;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Services;

public interface IProviderService
{
    List<Provider> GetAllProviders();
    Task<Provider> GetByIdAsync(int providerId);
    Task<Provider> CreateProviderAsync(CreateProviderDto dto);
    Task<Provider> UpdateProviderAsync(CreateProviderDto dto, int providerId);
    Task<bool> DeleteProviderAsync(int providerId);
}