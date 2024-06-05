using API.Data.Interfaces;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DavanaContext context) : base(context)
        {
        }

        public async Task<List<Appointment>> GetAppointments()
        {
            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap WHERE ap.active = 1 ").ToListAsync();
            return data;
        }

        public async Task<List<Appointment>> GetAppointmentsByCoachID(int coachID)
        {

            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap WHERE ap.coachid = {coachID} and ap.active = 1").ToListAsync();
            return data;

        }

        public async Task<Appointment> GetAppointmentsByIDHash(string IDHash)
        {
            var data = await _dbSet.FromSql($"SELECT * FROM Appointments ap WHERE SHA(ap.id) = {IDHash} and ap.active = 1").FirstOrDefaultAsync();
            return data ?? new Appointment();
        }
    }
}