using Assignment8.Data;
using Assignment8.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private readonly PdfGeneratorService _pdfGeneratorService;
        private readonly MongoDbAccess _mongoDbAccess;
        private readonly TripRepository _tripRepository;

        public TripController(PdfGeneratorService pdfGeneratorService, MongoDbAccess mongoDbAccess, TripRepository tripRepository)
        {
            _pdfGeneratorService = pdfGeneratorService;
            _mongoDbAccess = mongoDbAccess;
            _tripRepository = tripRepository;
        }

        [HttpGet("test-mongo-connection")]
        public IActionResult TestConnection()
        {
            bool isConnected = _mongoDbAccess.TestConnection();

            if (isConnected)
            {
                return Ok("Successfully connected to MongoDB.");
            }
            else
            {
                return StatusCode(500, "Failed to connect to MongoDB.");
            }
        }

        [HttpPost("export/{tripId}")]
        public async Task<IActionResult> ExportTripPdf(string tripId)
        {
            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            
            if (trip == null)
            {
                return NotFound("Trip not found.");
            }

            if (string.IsNullOrEmpty(trip.Itinerary))
            {
                return BadRequest("The itinerary for this trip is empty or missing.");
            }

            byte[] pdfBytes;
            try
            {
                pdfBytes = _pdfGeneratorService.GeneratePdfFromMarkdown(trip.Itinerary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }

            return File(pdfBytes, "application/pdf", $"{trip.Country}_{trip.StartDate:yyyyMMdd}_{trip.EndDate:yyyyMMdd}.pdf");
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
