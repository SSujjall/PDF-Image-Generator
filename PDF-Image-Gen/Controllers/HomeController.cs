using System.Diagnostics;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using PDF_Image_Gen.Models;

namespace PDF_Image_Gen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePDF([FromBody] string base64Image)
        {
            try
            {
                // Remove data:image/png;base64, prefix if present
                base64Image = base64Image.Replace("data:image/png;base64,", "");

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Create PDF
                using (MemoryStream ms = new MemoryStream())
                {
                    using (iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        document.Open();

                        // Convert byte array to iTextSharp Image
                        using (var imageStream = new MemoryStream(imageBytes))
                        {
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

                            // Scale image to fit page width while maintaining aspect ratio
                            float pageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                            float scaleFactor = pageWidth / image.Width;
                            image.ScalePercent(scaleFactor * 100);

                            document.Add(image);
                        }
                        document.Close();
                    }
                    return File(ms.ToArray(), "application/pdf", "receipt.pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF");
                return BadRequest("Error generating PDF");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
