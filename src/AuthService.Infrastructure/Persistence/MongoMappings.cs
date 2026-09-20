using AuthService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace AuthService.Infrastructure.Persistence;

public static class MongoMappings
{
    private static readonly object Lock = new();
    private static bool _registered;

    public static void Register()
    {
        lock (Lock)
        {
            if (_registered)
                return;

            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.MapIdProperty(u => u.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                map.MapProperty(u => u.UserName).SetElementName("username");
                map.MapProperty(u => u.Email).SetElementName("email");
                map.MapProperty(u => u.PasswordHash).SetElementName("passwordHash");
                map.MapProperty(u => u.Roles).SetElementName("roles");
            });

            _registered = true;
        }
    }
}
