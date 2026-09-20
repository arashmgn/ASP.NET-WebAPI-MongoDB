using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthService.Infrastructure.Persistence;

public class UserRepository : MongoRepository<User>, IUserRepository
{
    public UserRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
        CreateIndexes();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Collection
            .Find(Builders<User>.Filter.Eq(u => u.Email, email))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await Collection
            .Find(Builders<User>.Filter.Eq(u => u.UserName, userName))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private void CreateIndexes()
    {
        var unique = new CreateIndexOptions { Unique = true };

        Collection.Indexes.CreateOne(new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Ascending(u => u.UserName), unique));

        Collection.Indexes.CreateOne(new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Ascending(u => u.Email), unique));
    }
}
