using Amazon.DynamoDBv2.DataModel;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data.Repositories
{
    public class UsersDynamoRepository : DynamoDbRepository<User>, IUsersDynamoRepository
    {
        public UsersDynamoRepository(IDynamoDBContext context) : base(context)
        {
        }
    }
}