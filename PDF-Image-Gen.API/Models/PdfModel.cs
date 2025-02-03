namespace PDF_Image_Gen.API.Models
{
    public class PdfRequest
    {
        public required string HtmlContent { get; set; }
        public string? Title { get; set; }
    }

    public class PdfResponse
    {
        public byte[] FileContents { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "document.pdf";
        public string ContentType { get; set; } = "application/pdf";
    }
}
