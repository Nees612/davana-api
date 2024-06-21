namespace API.Services.Authentication.Models

{
    public class Credentials(string emailaddress, string passwordHash)
    {
        public string Emailaddress { get; set; } = emailaddress;
        public string PasswordHash { get; set; } = passwordHash;
    }
}