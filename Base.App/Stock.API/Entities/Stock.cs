using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Attributes;
using static MongoDB.Driver.WriteConcern;

namespace Stock.API.Entities
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 0)]
        public string Id { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 1)]
        public string ProductId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        [BsonElement(Order = 2)]
        public int Couunt { get; set; }
    }
}
