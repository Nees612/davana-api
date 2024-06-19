using Amazon.DynamoDBv2.DataModel;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data.Repositories
{
    public class AppointmentDynamoRepository : DynamoDbRepository<Appointment>, IAppointmentDynamoRepository
    {
        public AppointmentDynamoRepository(IDynamoDBContext context) : base(context)
        {
        }
    }
}