using Amazon.DynamoDBv2.Model;

namespace API.Extensions.DynamoDbExtensions
{
    public static class CoachTableDefinition
    {
        public static string TableName { get; } = "Coaches";
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
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "FirstName",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "MiddleName",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "LastName",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "EmailAddress",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "PasswordHash",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "Roles",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "Scopes",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "AppointmentGUIDs",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "PhoneNumber",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "ProfileImageURL",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "AboutMe",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "ClosestWorkAddress",
                    //     AttributeType = "S"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "CreatedOn",
                    //     AttributeType = "N"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "CreatedBy",
                    //     AttributeType = "N"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "LastUpdatedOn",
                    //     AttributeType = "N"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "LastUpdatedBy",
                    //     AttributeType = "N"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "Active",
                    //     AttributeType = "N"
                    // },
                    // new AttributeDefinition
                    // {
                    //     AttributeName = "RowVersionStamp",
                    //     AttributeType = "N"
                    // }
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