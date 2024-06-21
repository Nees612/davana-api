using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data
{
    public class DavanaDynamoDBContext(IAmazonDynamoDB client, DynamoDBContextConfig config) : DynamoDBContext(client, config), IDavanaDynamoDBContext
    {
        public Table Users { get; set; } = Table.LoadTable(client, "Users");
        public Table Coaches { get; set; } = Table.LoadTable(client, "Coaches");
        public Table Appointments { get; set; } = Table.LoadTable(client, "Appointments");

    }
}