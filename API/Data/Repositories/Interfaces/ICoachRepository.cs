using API.Data.DTO;
using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories.Interfaces
{
    public interface ICoachRepository : IRepository<Coach>
    {
        Task<List<CoachDTO>> GetCoaches();
        Task<CoachDTO> GetCoach(int coachID);
        Task<Coach> CheckCoachCredentials(Credentials signInCredentials);

    }
}