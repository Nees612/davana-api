using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories.Interfaces
{
    public interface IUsersDynamoRepository : IDynamoDBRepository<User>
    {
        Task<User?> GetUser(string id);
        Task<bool> PutUser(User User);
        Task<IEnumerable<User>> GetUsers();
        Task<User?> CheckUserCredentials(Credentials credentials);
    }
}