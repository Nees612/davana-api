using API.Entities;
using API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Data.Interfaces
{
    public interface ICoachRepository : IRepository<Coach>
    {        
        Task<List<Coach>> GetCoaches();
        Task<Coach> GetCoach(int coachID);
        Task<Coach> CheckCoachCredentials(Credentials signInCredentials);

    }
}