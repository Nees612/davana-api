using API.Data.Interfaces;
using API.Entities;
using API.Services.Authentication;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DavanaContext context) : base(context)
        {
        }

        public async Task<User> CheckUserCredentials(Credentials signInCredentials)
        {
            _ = signInCredentials ?? throw new ArgumentException("Parameter cannot be null", nameof(signInCredentials));

            var data = await _dbSet.FromSqlInterpolated($@"
                                                        SELECT *
                                                        FROM Users us
                                                        WHERE us.EmailAddress = {signInCredentials.Emailaddress}
                                                        AND us.PasswordHash = {signInCredentials.PasswordHash}").FirstAsync();
            return data;

        }

        public async Task<User> GetUser(int userID)
        {            

            var data = await _dbSet.FromSqlInterpolated($"SELECT * FROM Users us WHERE us.active = 1 and us.id = {userID}").FirstOrDefaultAsync();
            return data ?? new User();
        }

        public async Task<int> GetUserIDByIDHash(string IDHash)
        {
            var data = await _dbSet.FromSqlInterpolated($"SELECT * FROM Users us WHERE SHA(us.id) = {IDHash} and us.active = 1").FirstOrDefaultAsync();
            return data == null ? 0 : data.Id;
        }

        public async Task<List<User>> GetUsers()
        {
            var data = await _dbSet.FromSqlInterpolated($"SELECT * FROM Users us WHERE us.active = 1 ").ToListAsync();
            return data;
        }
    }
}