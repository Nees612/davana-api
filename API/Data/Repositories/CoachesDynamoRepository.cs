using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.DTO;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories
{
    public class CoachesDynamoRepository(IDavanaDynamoDBContext context) : DynamoDbRepository<Coach>(context), ICoachesDynamoRepository
    {
        private readonly Table _table = context.Coaches;
        private readonly IDavanaDynamoDBContext _context = context;

        public async Task<Coach> CheckCoachCredentials(Credentials credentials)
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
            Coach coach = _context.FromDocument<Coach>(result[0]);

            var verificationResult = BCrypt.Net.BCrypt.Verify(credentials.PasswordHash, coach.PasswordHash);

            return verificationResult ? coach : new Coach { };
        }

        public async Task<Coach> GetCoach(string id)
        {
            GetItemOperationConfig config = new()
            {
                AttributesToGet = CoachDTO.AttributesToGet,
                ConsistentRead = true
            };

            var result = await _table.GetItemAsync(id, config);
            Coach coach = _context.FromDocument<Coach>(result);

            return coach;
        }

        public async Task<IEnumerable<Coach>> GetCoaches()
        {
            ScanFilter scanFilter = new();

            scanFilter.AddCondition("active", ScanOperator.Equal, 1);

            ScanOperationConfig config = new()
            {
                AttributesToGet = CoachDTO.AttributesToGet,
                Filter = scanFilter,
                Select = SelectValues.SpecificAttributes
            };

            Search search = _table.Scan(config);

            var result = await search.GetNextSetAsync();

            IEnumerable<Coach> coaches = _context.FromDocuments<Coach>(result);

            return coaches;
        }

        public async Task<bool> PutCoach(Coach coach)
        {
            var doc = new Document();

            coach.Id = Guid.NewGuid().ToString();
            coach.PasswordHash = BCrypt.Net.BCrypt.HashPassword(coach.PasswordHash);

            doc["Id"] = coach.Id;
            doc["firstName"] = coach.FirstName;
            doc["middleName"] = coach.MiddleName;
            doc["lastName"] = coach.LastName;
            doc["emailAddress"] = coach.EmailAddress;
            doc["passwordHash"] = coach.PasswordHash;
            doc["roles"] = coach.Roles;
            doc["scopes"] = coach.Scopes;
            doc["phoneNumber"] = coach.PhoneNumber;
            doc["active"] = coach.Active;
            doc["createdOn"] = coach.CreatedOn;
            doc["rowVersionStamp"] = coach.RowVersionStamp;

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