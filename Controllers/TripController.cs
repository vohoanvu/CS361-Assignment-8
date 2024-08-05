using Assignment8.Data;
using Microsoft.AspNetCore.Mvc;

namespace Assignment8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MongoDbController : ControllerBase
    {
        private readonly MongoDbAccess _mongoDbAccess;

        public MongoDbController(MongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            bool isConnected = _mongoDbAccess.TestConnection();
            if (isConnected)
            {
                return Ok("Successfully connected to MongoDB!");
            }
            else
            {
                return StatusCode(500, "Failed to connect to MongoDB.");
            }
        }
    }
}
