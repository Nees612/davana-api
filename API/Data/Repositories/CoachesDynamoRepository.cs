using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data.Repositories
{
    public class CoachesDynamoRepository : DynamoDbRepository<Coach>, ICoachesDynamoRepository
    {
        private readonly Table _table;
        public CoachesDynamoRepository(IAmazonDynamoDB client, IDynamoDBContext context) : base(context)
        {
            _table = Table.LoadTable(client, "Coaches");
        }

        public async Task<Document> PutItem(Coach coach)
        {
            var doc = new Document();
            doc["Id"] = coach.Id.ToString();
            doc["firstName"] = coach.FirstName;
            doc["middleName"] = coach.MiddleName;
            doc["lastName"] = coach.LastName;
            doc["emailAddress"] = coach.EmailAddress;
            doc["passwordHash"] = coach.PasswordHash;
            doc["roles"] = coach.Roles;
            doc["scopes"] = coach.Scopes;
            doc["phoneNumber"] = coach.PhoneNumber;

            return await _table.PutItemAsync(doc);
        }

    }
}