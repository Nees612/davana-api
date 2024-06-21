using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data
{
    public class DavanaDynamoDBContext : DynamoDBContext, IDavanaDynamoDBContext
    {
        public DavanaDynamoDBContext(IAmazonDynamoDB client, DynamoDBContextConfig config) : base(client, config)
        {
            Users = Table.LoadTable(client, "Users");
            Coaches = Table.LoadTable(client, "Coaches");
            Appointments = Table.LoadTable(client, "Appointments");
        }

        public Table Users { get; set; }
        public Table Coaches { get; set; }
        public Table Appointments { get; set; }

    }
}