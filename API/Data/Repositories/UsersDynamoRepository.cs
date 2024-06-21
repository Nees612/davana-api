using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.DTO;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories
{
    public class UsersDynamoRepository(IDavanaDynamoDBContext context) : DynamoDbRepository<User>(context), IUsersDynamoRepository
    {
        private readonly Table _table = context.Users;
        private readonly IDavanaDynamoDBContext _context = context;

        public async Task<User> CheckUserCredentials(Credentials credentials)
        {
            ScanFilter scanFilter = new();

            scanFilter.AddCondition("emailAddress", ScanOperator.Equal, credentials.Emailaddress);
            scanFilter.AddCondition("passwordHash", ScanOperator.Equal, credentials.PasswordHash);

            ScanOperationConfig config = new()
            {
                Filter = scanFilter,
                Select = SelectValues.AllAttributes,
                Limit = 1
            };

            Search search = _table.Scan(config);
            var result = await search.GetNextSetAsync();
            User user = _context.FromDocument<User>(result[0]);

            return user;
        }

        public async Task<User> GetUser(string id)
        {
            GetItemOperationConfig config = new()
            {
                AttributesToGet = UserDTO.AttributesToGet,
                ConsistentRead = true
            };

            var result = await _table.GetItemAsync(id, config);
            User user = _context.FromDocument<User>(result);

            return user;
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PutUser(User user)
        {
            var doc = new Document();

            doc["Id"] = user.Id;
            doc["firstName"] = user.FirstName;
            doc["middleName"] = user.MiddleName;
            doc["lastName"] = user.LastName;
            doc["emailAddress"] = user.EmailAddress;
            doc["passwordHash"] = user.PasswordHash;
            doc["roles"] = user.Roles;
            doc["scopes"] = user.Scopes;
            doc["phoneNumber"] = user.PhoneNumber;
            doc["emailVerified"] = user.EmailVerified;
            doc["active"] = user.Active;
            doc["createdOn"] = user.CreatedOn;
            doc["rowVersionStamp"] = user.RowVersionStamp;

            try
            {
                await _table.PutItemAsync(doc);
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
    }
}