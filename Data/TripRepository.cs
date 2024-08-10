using MongoDB.Driver;

namespace Assignment8.Data
{
    public class TripRepository
    {
        private readonly IMongoCollection<Trip> _tripCollection;

        public TripRepository(MongoDbAccess mongoDbAccess)
        {
            var database = mongoDbAccess.GetDatabase("trips");
            _tripCollection = database.GetCollection<Trip>("trips"); // "trips" should match your MongoDB collection name
        }

        public async Task<Trip?> GetTripByIdAsync(string tripId)
        {
            var filter = Builders<Trip>.Filter.Eq(t => t.Id, tripId);
            return await _tripCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
