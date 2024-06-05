namespace API.Services.Authentication
{
    public class Credentials
    {
        public string Emailaddress { get; set; }
        public string PasswordHash { get; set; }
        public Credentials(string emailaddress, string passwordHash)
        {
            PasswordHash = passwordHash;
            Emailaddress = emailaddress;
        }
    }
}