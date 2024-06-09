using API.Data.DTO;
using API.Data.Interfaces;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DavanaContext context) : base(context)
        {
        }

        public async Task<Appointment> GetAppointmentByID(int appointmentID)
        {
            var data = await _dbSet.FromSqlInterpolated($"SELECT * FROM Appointments ap WHERE ap.id = {appointmentID}").FirstOrDefaultAsync();
            return data ?? new Appointment();
        }

        public async Task<List<AppointmentDTO>> GetAppointments()
        {
            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap")
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    CoachId = a.CoachId,
                    UserId = a.UserId,
                    Date = a.Date,
                    Comment = a.Comment,
                    MeetingType = a.MeetingType,
                    Approoved = a.Approoved
                }

                ).ToListAsync();
            return data ?? new List<AppointmentDTO>();
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByCoachID(int coachID)
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap WHERE ap.coachid = {coachID} and ap.userid = 0")
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    CoachId = a.CoachId,
                    UserId = a.UserId,
                    Date = a.Date,
                    Comment = a.Comment,
                    MeetingType = a.MeetingType,
                    Approoved = a.Approoved
                }

            ).OrderBy(a => a.Date).ToListAsync();
            return data ?? new List<AppointmentDTO>();


        }

        public async Task<Appointment> GetAppointmentsByIDHash(string IDHash)
        {
            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap WHERE SHA(ap.id) = {IDHash} and ap.active = 1")
                .FirstOrDefaultAsync();
            return data ?? new Appointment();
        }
    }
}