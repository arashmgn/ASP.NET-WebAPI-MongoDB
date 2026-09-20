using AuthService.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthService.Infrastructure.Persistence;

public class MongoRepository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> Collection;

    public MongoRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        return await Collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return false;

        var result = await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), entity, cancellationToken: cancellationToken);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return false;

        var result = await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId), cancellationToken);
        return result.DeletedCount > 0;
    }
}
