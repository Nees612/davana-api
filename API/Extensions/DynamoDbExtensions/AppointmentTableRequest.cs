using Amazon.DynamoDBv2.Model;

namespace API.Extensions.DynamoDbExtensions
{
    public class AppointmentTableRequest
    {
        public static string TableName { get; } = "Appointments";
        public static CreateTableRequest Request { get; } = new CreateTableRequest
        {
            AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "S"
                    }
                    // ,
                //     new AttributeDefinition
                //     {
                //         AttributeName = "CoachId",
                //         AttributeType = "S"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "Date",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "Comment",
                //         AttributeType = "S"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "MeetingType",
                //         AttributeType = "S"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "UserId",
                //         AttributeType = "S"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "Approoved",
                //         AttributeType = "N"
                //     },


                //     new AttributeDefinition
                //     {
                //         AttributeName = "CreatedOn",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "CreatedBy",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "LastUpdatedOn",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "LastUpdatedBy",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "Active",
                //         AttributeType = "N"
                //     },
                //     new AttributeDefinition
                //     {
                //         AttributeName = "RowVersionStamp",
                //         AttributeType = "N"
                //     }
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