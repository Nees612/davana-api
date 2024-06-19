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
    public class DynamoDbRepository<T> : IDynamoDBRepository<T> where T : class
    {
        private IDynamoDBContext _context;

        public DynamoDbRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(T item)
        {
            await _context.DeleteAsync(item);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<T>(id);
        }

        public async Task SaveAsync(T item)
        {
            await _context.SaveAsync(item);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}