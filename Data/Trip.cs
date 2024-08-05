using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assignment8.Data
{
    public class Trip
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }

        [BsonElement("tripType")]
        public string? TripType { get; set; }

        [BsonElement("__v")]
        public int Version { get; set; }

        [BsonElement("itinerary")]
        public string? Itinerary { get; set; }
    }

    public class TripPayload
    {
        public string Id { get; set; }
        public string? Country { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? TripType { get; set; }
        public required string Itinerary { get; set; }
    }
}
