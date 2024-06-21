using Amazon.DynamoDBv2.DocumentModel;

namespace API.Data.Repositories.Interfaces
{
    public interface IDynamoDBRepository<T> : IDisposable where T : class
    {
        Task SaveAsync(T item);
        Task DeleteByIdAsync(T item);

    }

}