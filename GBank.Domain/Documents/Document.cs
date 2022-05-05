using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GBank.Domain.Documents
{
    //base of the documents
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}