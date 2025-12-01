using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Services;

public class ProviderService : IProviderService
{
    private readonly IProviderRepository _providerRepository;

    public ProviderService(IProviderRepository providerRepository)
    {
        _providerRepository =  providerRepository;
    }

    public List<Provider> GetAllProviders()
    {
        var providers = _providerRepository.GetAllQueryable().ToList();
        return providers;
    }

    public async Task<Provider> GetByIdAsync(int providerId)
    {
        var provider = await _providerRepository.GetByIdAsync(providerId);
        return provider;
    }

    public async Task<Provider> CreateProviderAsync(CreateProviderDto dto)
    {
        var provider = new Provider()
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        
        var createdProvider = await _providerRepository.AddAsync(provider);
        return createdProvider;
    }

    public async Task<Provider> UpdateProviderAsync(CreateProviderDto dto, int providerId)
    {
        var currentProvider = await _providerRepository.GetByIdAsync(providerId);
        
        currentProvider.Name = dto.Name;
        currentProvider.Email = dto.Email;
        currentProvider.PhoneNumber = dto.PhoneNumber;
        currentProvider.IsActive = dto.IsActive;
        currentProvider.UpdateAt = DateTime.UtcNow;

        var updateProvider = await _providerRepository.UpdateAsync(currentProvider);
        return updateProvider;
    }

    public async Task<bool> DeleteProviderAsync(int providerId)
    {
        return await _providerRepository.DeleteAsync(providerId);
    }
}