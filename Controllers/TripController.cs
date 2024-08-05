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

        public TripController(PdfGeneratorService pdfGeneratorService)
        {
            _pdfGeneratorService = pdfGeneratorService;
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
