﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ssi.TrackTruck.Bussiness.Auth;

namespace Ssi.TrackTruck.Bussiness.DAL.Entities
{
    [BsonIgnoreExtraElements(true)]
    public class DbUser : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string UsernameLowerCase { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }

        public DbUser()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
