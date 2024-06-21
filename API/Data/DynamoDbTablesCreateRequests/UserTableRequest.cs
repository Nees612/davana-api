using Amazon.DynamoDBv2.Model;

namespace API.Data.DynamoDbTablesCreateRequests
{
    public static class UserTableRequest
    {
        public static string TableName { get; } = "Users";
        public static CreateTableRequest Request { get; } = new CreateTableRequest
        {
            AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "S"
                    }

                },

            KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" //Partition key
                    }
                },

            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 6
            },
            TableName = TableName
        };
    }
}