using API.Data.DTO;
using API.Data.Interfaces;
using API.Entities;
using API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class CoachRepository : Repository<Coach>, ICoachRepository
    {
        public DavanaContext Context { get; }
        public CoachRepository(DavanaContext context) : base(context)
        {
            Context = context;
        }

        public async Task<CoachDTO> GetCoach(int coachID)
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Coaches co WHERE co.id = {coachID}").Select(c => new CoachDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                MiddleName = c.MiddleName,
                LastName = c.LastName,
                EmailAddress = c.EmailAddress,
                PhoneNumber = c.PhoneNumber,
                Roles = c.Roles,
                Scopes = c.Scopes,
                ProfileImageURL = c.ProfileImageURL,
                AboutMe = c.AboutMe,
                ClosestWorkAddress = c.ClosestWorkAddress

            }).FirstOrDefaultAsync();

            return data ?? new CoachDTO();

        }

        public async Task<List<CoachDTO>> GetCoaches()
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Coaches co ").Select(c => new CoachDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                MiddleName = c.MiddleName,
                LastName = c.LastName,
                EmailAddress = c.EmailAddress,
                PhoneNumber = c.PhoneNumber,
                Roles = c.Roles,
                Scopes = c.Scopes,
                ProfileImageURL = c.ProfileImageURL,
                AboutMe = c.AboutMe,
                ClosestWorkAddress = c.ClosestWorkAddress

            }).ToListAsync();

            return data ?? new List<CoachDTO>();

        }

        public async Task<Coach> CheckCoachCredentials(Credentials signInCredentials)
        {
            _ = signInCredentials ?? throw new ArgumentException("Parameter cannot be null", nameof(signInCredentials));
            var data = await _dbSet.FromSqlInterpolated($@"
                                                                SELECT *
                                                                FROM Coaches co
                                                                WHERE co.EmailAddress = {signInCredentials.Emailaddress}
                                                                AND co.PasswordHash = {signInCredentials.PasswordHash}")
                            .FirstOrDefaultAsync();
            return data ?? new Coach();

        }

    }
}