using Microsoft.AspNetCore.Mvc;

namespace API.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {

        Task<ActionResult> Add(T entity);
        Task<ActionResult> Remove(T entity);
        Task<ActionResult> Update(T entity);
        Task<ActionResult> AddRange(IEnumerable<T> entities);
        Task<ActionResult> RemoveRange(IEnumerable<T> entities);
        Task<ActionResult> SaveChangesAsync();
    }
}