using iText.Html2pdf;
using iText.Kernel.Geom;
using PDF_Image_Gen.API.Models;
using iText.Kernel.Pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace PDF_Image_Gen.API.Service
{
    public class PdfService : IPdfService
    {
        private readonly string _defaultStyles;

        public PdfService()
        {
            //_defaultStyles = @"
            //    .receipt-container {
            //        max-width: 700px;
            //        margin: 20px auto;
            //        padding: 20px;
            //        border: 3px solid #0d6efd;
            //        border-radius: 10px;
            //        background-color: #f8f9fa;
            //    }
            //    .header {
            //        display: flex;
            //        align-items: center;
            //        justify-content: space-between;
            //        text-align: center;
            //        margin-bottom: 20px;
            //        padding-left: 2rem;
            //        padding-right: 2rem;
            //    }
            //    .header .company-details {
            //        flex-grow: 1;
            //        text-align: center;
            //    }
            //    .logo {
            //        width: 100px;
            //    }
            //    .payment-receipt {
            //        text-align: center;
            //        background: #0d6efd;
            //        color: white;
            //        padding: 10px;
            //        font-weight: bold;
            //        border-radius: 5px;
            //        margin-bottom: 1.5rem;
            //        width: 40%;
            //        margin-left: auto;
            //        margin-right: auto;
            //    }
            //    .table {
            //        background-color: white;
            //        width: 100%;
            //        border-collapse: collapse;
            //    }
            //    .table td {
            //        padding: 8px 15px;
            //    }
            //    .footer {
            //        display: flex;
            //        justify-content: space-between;
            //        align-items: center;
            //        margin-top: 20px;
            //        font-weight: bold;
            //        width: 100%;
            //    }
            //    .footer p {
            //        flex: 1;
            //        text-align: left;
            //    }
            //    .signature {
            //        border-top: 1px dotted black;
            //        text-align: center;
            //        width: 100px;
            //        flex: 1;
            //        text-align: right;
            //    }
            //    body {
            //        font-family: Arial, sans-serif;
            //        line-height: 1.6;
            //        margin: 20px;
            //    }
            //    h4 {
            //        margin: 0;
            //        padding: 0;
            //    }
            //    p {
            //        margin: 5px 0;
            //    }";
        }

        public async Task<PdfResponse> GeneratePdfFromHtml(PdfRequest request)
        {
            try
            {
                var cssContent = ExtractCssFromHtml(request.HtmlContent);

                var htmlContentWithoutCss = RemoveCssFromHtml(request.HtmlContent);

                var fullHtmlContent = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <title>{request.Title ?? "Payment Receipt"}</title>
                        <style>
                            {cssContent}
                        </style>
                    </head>
                    <body>
                        {htmlContentWithoutCss}
                    </body>
                    </html>";

                using var memoryStream = new MemoryStream();

                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                pdf.SetDefaultPageSize(PageSize.A4);

                var converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(fullHtmlContent, pdf, converterProperties);

                return new PdfResponse
                {
                    FileContents = memoryStream.ToArray(),
                    FileName = $"{(request.Title ?? "receipt")}.pdf",
                    ContentType = "application/pdf"
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to generate PDF", ex);
            }
        }

        // Method to extract CSS content inside <style> tags
        private string ExtractCssFromHtml(string htmlContent)
        {
            var match = Regex.Match(htmlContent, @"<style[^>]*>(.*?)</style>", RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        // Method to remove <style> tags from HTML content
        private string RemoveCssFromHtml(string htmlContent)
        {
            return Regex.Replace(htmlContent, @"<style[^>]*>.*?</style>", string.Empty, RegexOptions.Singleline);
        }
    }
}