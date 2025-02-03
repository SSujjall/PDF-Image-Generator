using Microsoft.AspNetCore.Mvc;
using PDF_Image_Gen.API.Models;
using PDF_Image_Gen.API.Service;

namespace PDF_Image_Gen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly ILogger<PdfController> _logger;

        public PdfController(IPdfService pdfService, ILogger<PdfController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        [HttpPost("generate-pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GeneratePdf(PdfRequest request)
        {
            try
            {
                var result = await _pdfService.GeneratePdfFromHtml(request);
                return File(result.FileContents, result.ContentType, result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF");
                return BadRequest(new ProblemDetails
                {
                    Title = "PDF Generation Failed",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}
