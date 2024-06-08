using API.Data.DTO;
using API.Entities;
using API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Data.Interfaces
{
    public interface ICoachRepository : IRepository<Coach>
    {        
        Task<List<CoachDTO>> GetCoaches();
        Task<CoachDTO> GetCoach(int coachID);
        Task<Coach> CheckCoachCredentials(Credentials signInCredentials);

    }
}