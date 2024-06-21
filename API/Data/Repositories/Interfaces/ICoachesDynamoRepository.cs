using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories.Interfaces
{
    public interface ICoachesDynamoRepository : IDynamoDBRepository<Coach>
    {
        Task<Coach> GetCoach(string id);
        Task<bool> PutCoach(Coach coach);
        Task<IEnumerable<Coach>> GetCoaches();
        Task<Coach> CheckCoachCredentials(Credentials credentials);

    }
}