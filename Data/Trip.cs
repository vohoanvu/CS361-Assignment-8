using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Assignment8.Data
{
    //unused
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
        public string? Id { get; set; }
    
        public string? Country { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid start date format.")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid end date format.")]
        public DateTime? EndDate { get; set; }

        public string? TripType { get; set; }

        [Required(ErrorMessage = "The Itinerary field is required.")]
        public required string Itinerary { get; set; }
    }

}
