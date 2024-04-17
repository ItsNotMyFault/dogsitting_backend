using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {


        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        DbSet<T> Build();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

    }

}
