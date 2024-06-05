namespace API.Entities
{
    public class Coach : BaseEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PasswordHash { get; set; }
        public string[]? Roles {get; set;}
        public string[]? Scopes {get; set;}
        public string? PhoneNumber { get; set; }

    }
}