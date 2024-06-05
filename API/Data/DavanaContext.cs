
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DavanaContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}