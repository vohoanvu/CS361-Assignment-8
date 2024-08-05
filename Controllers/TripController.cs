using Assignment8.Data;
using Assignment8.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MongoDbController : ControllerBase
    {
        private readonly MongoDbAccess _mongoDbAccess;
        private readonly PdfGeneratorService _pdfGeneratorService;

        public MongoDbController(MongoDbAccess mongoDbAccess, PdfGeneratorService pdfGeneratorService)
        {
            _mongoDbAccess = mongoDbAccess;
            _pdfGeneratorService = pdfGeneratorService;
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

        [HttpPost("export-itinerary")]
        public IActionResult ExportItinerary([FromBody] TripPayload trip)
        {
            if (!ModelState.IsValid)
            {
                // Gather all validation errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Message = "Validation failed.", Errors = errors });
            }

            try
            {
                var pdfBytes = _pdfGeneratorService.GeneratePdfFromMarkdown(trip.Itinerary);

                return File(pdfBytes, "application/pdf", $"Itinerary_{trip.Id ?? "null_id"}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the PDF: {ex.Message}");
            }
        }
    }
}
