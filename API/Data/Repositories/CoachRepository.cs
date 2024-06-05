using API.Data.Interfaces;
using API.Entities;
using API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class CoachRepository : Repository<Coach>, ICoachRepository
    {
        public CoachRepository(DavanaContext context) : base(context)
        {
        }

        public async Task<Coach> GetCoach(int coachID)
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Coaches co WHERE co.active = 1 and co.id = {coachID}").FirstOrDefaultAsync();
            return data ?? new Coach();

        }

        public async Task<List<Coach>> GetCoaches()
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Coaches co WHERE co.active = 1 ").ToListAsync();
            return data;

        }

        public async Task<Coach> CheckCoachCredentials(Credentials signInCredentials)
        {
            _ = signInCredentials ?? throw new ArgumentException("Parameter cannot be null", nameof(signInCredentials));
            var data = await _dbSet.FromSqlInterpolated($@"
                                                                SELECT *
                                                                FROM Coaches co
                                                                WHERE co.EmailAddress = {signInCredentials.Emailaddress}
                                                                AND co.PasswordHash = {signInCredentials.PasswordHash}")
                            .FirstAsync();
            return data;

        }

    }
}