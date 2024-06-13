using API.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DavanaContext _context;
        public readonly DbSet<T> _dbSet;
        public Repository(DavanaContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<ActionResult> Add(T entity)
        {
            _ = entity ?? throw new ArgumentException("Parameter cannot be null", nameof(entity));

            try
            {
                await Task.Run(() => { _dbSet.Add(entity); });
                return new OkObjectResult(entity);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<ActionResult> Remove(T entity)
        {
            _ = entity ?? throw new ArgumentException("Parameter cannot be null", nameof(entity));

            try
            {
                await Task.Run(() => { _dbSet.Remove(entity); });
                return new OkObjectResult(entity);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<ActionResult> Update(T entity)
        {
            _ = entity ?? throw new ArgumentException("Parameter cannot be null", nameof(entity));

            try
            {
                await Task.Run(() => { _dbSet.Update(entity); });
                return new OkObjectResult(entity);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<ActionResult> AddRange(IEnumerable<T> entities)
        {
            _ = entities ?? throw new ArgumentException("Parameter cannot be null", nameof(entities));

            try
            {
                await Task.Run(() => _dbSet.AddRange(entities));
                return new OkObjectResult(entities);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<ActionResult> RemoveRange(IEnumerable<T> entities)
        {
            _ = entities ?? throw new ArgumentException("Parameter cannot be null", nameof(entities));

            try
            {
                await Task.Run(() => { _dbSet.RemoveRange(entities); });
                return new OkObjectResult(entities);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<ActionResult> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new OkObjectResult("Changes are saved.");

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}