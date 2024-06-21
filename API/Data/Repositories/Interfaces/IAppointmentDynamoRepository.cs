using API.Entities;

namespace API.Data.Repositories.Interfaces
{
    public interface IAppointmentDynamoRepository : IDynamoDBRepository<Appointment>
    {
        Task<bool> PutAppointment(Appointment appointment);
        Task<Appointment> GetAppointment(string appointmentID);
        Task<IEnumerable<Appointment>> GetAppointments();
        Task<IEnumerable<Appointment>> GetAppointmentsByCoachID(string coachID, bool skipBooked = true);
        Task<Appointment> GetAppointmentsByIDHash(string IDHash);

    }
}