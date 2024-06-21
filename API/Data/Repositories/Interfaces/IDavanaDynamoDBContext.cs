using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace API.Data.Repositories.Interfaces
{
    public interface IDavanaDynamoDBContext : IDynamoDBContext
    {
        public Table Users { get; set; }
        public Table Coaches { get; set; }
        public Table Appointments { get; set; }
    }
}