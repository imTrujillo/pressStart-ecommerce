using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    IQueryable<T> GetAllQueryable();
    Task<bool> DeleteAsync(int id);
    Task<T> UpdateAsync(T entity);
    Task<bool> ExistAsync(int id);
}