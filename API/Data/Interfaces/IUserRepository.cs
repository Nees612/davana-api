using API.Entities;
using API.Services.Authentication;

namespace API.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int userID);
        Task<User> CheckUserCredentials(Credentials signInCredentials);
        Task<int> GetUserIDByIDHash(string IDHash);
    }
}