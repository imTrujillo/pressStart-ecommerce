using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ShopDbContext Context;

    public BaseRepository(ShopDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        try
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar {typeof(T).Name}: {ex.Message} - {ex.InnerException?.Message}");
            throw;
        }
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id must be greater than 0", nameof(id));
        
        var entity = await Context.Set<T>().FindAsync(id);

        if (entity == null) throw new CustomNotFoundException($"{typeof(T).Name} with Id {id} was not Found");
        
        return entity;
    }

    public IQueryable<T> GetAllQueryable()
    {
        return Context.Set<T>().AsQueryable();
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<bool> ExistAsync(int id)
    {
        return await Context.Set<T>().AnyAsync(e => e.Id == id);
    }
}