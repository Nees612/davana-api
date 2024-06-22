using System.ComponentModel;
using Amazon.DynamoDBv2.DataModel;

namespace API.Entities
{

    [DynamoDBTable("Users")]
    public class User : BaseEntity
    {
        [DynamoDBHashKey]
        public string? Id { get; set; }
        [DynamoDBProperty("firstName")]
        public string? FirstName { get; set; }
        [DynamoDBProperty("middleName")]
        public string? MiddleName { get; set; } = "";
        [DynamoDBProperty("lastName")]
        public string? LastName { get; set; }
        [DynamoDBProperty("emailAddress")]
        public string? EmailAddress { get; set; }
        [DynamoDBProperty("passwordHash")]
        public string? PasswordHash { get; set; }
        [DynamoDBProperty("roles")]
        public string[]? Roles { get; set; } = ["user"];
        [DynamoDBProperty("scopes")]
        public string[]? Scopes { get; set; } = ["booking"];
        [DynamoDBProperty("phoneNumber")]
        public string? PhoneNumber { get; set; } = "";
        [DynamoDBProperty("emailVerified")]
        public int EmailVerified { get; set; } = 0;
    }
}