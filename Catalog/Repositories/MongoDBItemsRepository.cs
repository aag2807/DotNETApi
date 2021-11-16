using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDBItemsRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuiler = Builders<Item>.Filter;

        public MongoDBItemsRepository(IMongoClient client)
        {
            IMongoDatabase database = client.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }


        public async Task CreateItemAsync(Item item)
        {
           await itemsCollection.InsertOneAsync(item);
        }

         public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuiler.Eq(item => item.Id, id);
            await itemsCollection.DeleteOneAsync(filter);
        }


        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuiler.Eq(item => item.Id, id);
            return  await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            List<Item> items = await itemsCollection.Find(new BsonDocument()).ToListAsync();
            return items;
        }

        public async Task UpdateItemAsync(Guid id, Item item)
        {
            var filter = filterBuiler.Eq(item => item.Id, id);
            await itemsCollection.ReplaceOneAsync(filter, item);
            
        }

  
    }
}
