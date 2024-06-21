using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.DTO;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Services.Authentication.Models;

namespace API.Data.Repositories
{
    public class CoachesDynamoRepository : DynamoDbRepository<Coach>, ICoachesDynamoRepository
    {
        private readonly Table _table;
        private readonly IDavanaDynamoDBContext _context;
        public CoachesDynamoRepository(IDavanaDynamoDBContext context) : base(context)
        {
            _context = context;
            _table = context.Coaches;
        }

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

            return coach;
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