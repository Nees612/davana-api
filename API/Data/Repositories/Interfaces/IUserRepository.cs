using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int userID);
        Task<User> GetUserByEmailHash(string EmailHash);
        Task<int> GetUserIDByIDHash(string IDHash);
        Task<User> CheckUserCredentials(Credentials signInCredentials);
    }
}