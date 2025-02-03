using Newtonsoft.Json.Linq;
using PDF_Image_Gen.API.Models;

namespace PDF_Image_Gen.API.Service
{
    public interface IPdfService
    {
        //Task<string> ConvertHtmlToJsonAsync(string htmlContent);
        Task<PdfResponse> GeneratePdfFromHtml(PdfRequest request);
    }
}
