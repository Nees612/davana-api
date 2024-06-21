using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.Repositories.Interfaces;

namespace API.Data.Repositories
{
    public class DynamoDbRepository<T>(IDavanaDynamoDBContext context) : IDynamoDBRepository<T> where T : class
    {
        private IDavanaDynamoDBContext _context = context;

        public async Task DeleteByIdAsync(T item)
        {
            await _context.DeleteAsync(item);
        }

        public async Task SaveAsync(T item)
        {
            await _context.SaveAsync(item);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}