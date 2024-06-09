using API.Data.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Data.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<Appointment> GetAppointmentByID(int appointmentID);
        Task<List<AppointmentDTO>> GetAppointmentsByCoachID(int coachID);
        Task<List<AppointmentDTO>> GetAppointments();
        Task<Appointment> GetAppointmentsByIDHash(string IDHash);

    }
}