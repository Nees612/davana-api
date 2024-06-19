using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace API.Data
{
    public class DavanaDynamoDBContext : DynamoDBContext, IDynamoDBContext
    {
        public readonly IAmazonDynamoDB _client;
        public DavanaDynamoDBContext(IAmazonDynamoDB client, DynamoDBContextConfig config) : base(client, config)
        {
            _client = client;
        }

    }

    // [DynamoDBTable("tablewithhashkey")]
    // public class testtable
    // {
    //     [DynamoDBHashKey] //Partition key
    //     public string id
    //     {
    //         get; set;
    //     }

    //     [DynamoDBProperty("name")]
    //     public string name
    //     {
    //         get; set;
    //     }

    //     public override string ToString()
    //     {
    //         return $"The test ID and testdate: {id} - {name}";
    //     }

    // }

}