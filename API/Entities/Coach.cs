using Amazon.DynamoDBv2.DataModel;

namespace API.Entities
{

    [DynamoDBTable("Coaches")]
    public class Coach : BaseEntity
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }

        [DynamoDBProperty("firstName")]
        public string? FirstName { get; set; }
        [DynamoDBProperty("middleName")]
        public string? MiddleName { get; set; }
        [DynamoDBProperty("lastName")]
        public string? LastName { get; set; }

        [DynamoDBProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        [DynamoDBProperty("passwordHash")]
        public string? PasswordHash { get; set; }

        [DynamoDBProperty("roles")]
        public string[]? Roles { get; set; }

        [DynamoDBProperty("scopes")]
        public string[]? Scopes { get; set; }

        [DynamoDBProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [DynamoDBProperty("profileUrl")]
        public string? ProfileImageURL { get; set; }

        [DynamoDBProperty("aboutMe")]
        public string? AboutMe { get; set; }

        [DynamoDBProperty("closestWorkAddress")]
        public string? ClosestWorkAddress { get; set; }

    }
}