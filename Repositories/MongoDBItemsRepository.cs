using System;
using System.Collections.Generic;
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
        private readonly FilterDefinitionBuilder<Item> itemsFilterBuilder = Builders<Item>.Filter;


        // MongoDB Client
        public MongoDBItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(new BsonDocument()).ToList();
        }

        public Item GetItem(Guid id)
        {
            var filter = itemsFilterBuilder.Eq(item => item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void UpdateItem(Item item)
        {
            var filter = itemsFilterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            itemsCollection.ReplaceOne(filter, item);
        }

        public void DeleteItem(Guid id)
        {
            var filter = itemsFilterBuilder.Eq(existingItem => existingItem.Id, id);
            itemsCollection.DeleteOne(filter);
        }
    }
}
