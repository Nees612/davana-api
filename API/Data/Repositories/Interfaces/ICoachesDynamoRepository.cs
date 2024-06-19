
using Amazon.DynamoDBv2.DocumentModel;
using API.Entities;

namespace API.Data.Repositories.Interfaces
{
    public interface ICoachesDynamoRepository : IDynamoDBRepository<Coach>
    {
        Task<Document> PutItem(Coach coach);
    }
}