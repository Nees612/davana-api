using API.Data.DTO;
using API.Entities;

namespace API.Data.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<Appointment> GetAppointmentByID(int appointmentID);
        Task<List<AppointmentDTO>> GetAppointmentsByCoachID(int coachID);
        Task<List<AppointmentDTO>> GetAppointments();
        Task<Appointment> GetAppointmentsByIDHash(string IDHash);

    }
}