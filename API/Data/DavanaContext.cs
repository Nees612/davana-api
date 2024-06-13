
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{

    /// Needs to be replaced with Dynamo DB Client and connection  write a code wich acts like a reposítory but uses dynamo db
    public class DavanaContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}