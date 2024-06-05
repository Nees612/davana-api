using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Data.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByCoachID(int coachID);
        Task<List<Appointment>> GetAppointments();
        Task<Appointment> GetAppointmentsByIDHash(string IDHash);
    }
}