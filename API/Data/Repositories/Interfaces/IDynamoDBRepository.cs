using Amazon.DynamoDBv2.DocumentModel;

namespace API.Data.Repositories.Interfaces
{
    public interface IDynamoDBRepository<T> : IDisposable where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task SaveAsync(T item);
        Task DeleteByIdAsync(T item);

    }

}