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
        public async Task<PdfResponse> GeneratePdfFromHtml()
        {
            try
            {
                var fullHtmlContent = @"
                                        <!DOCTYPE html>
                                        <html>
                                        <head>
                                            <meta charset='UTF-8'>
                                            <title>Payment Receipt</title>
                                            <style>
                                                .receipt-container {
                                                    max-width: 700px;
                                                    margin: 20px auto;
                                                    padding: 20px;
                                                    border: 3px solid #0d6efd;
                                                    border-radius: 10px;
                                                    background-image: url('https://res.cloudinary.com/dpzfiivmk/image/upload/v1738217428/Ecom/emcqktlp9rit28jnquie.png');
                                                    background-size: cover; 
                                                    background-position: center; 
                                                    background-repeat: no-repeat; 
                                                    background-attachment: fixed; 
                                                }
                                                .header {
                                                    display: flex;
                                                    align-items: center;
                                                    justify-content: space-between;
                                                    text-align: center;
                                                    margin-bottom: 20px;
                                                    padding-left: 2rem;
                                                    padding-right: 2rem;
                                                }
                                                .header .company-details {
                                                    flex-grow: 1;
                                                    text-align: center;
                                                }
                                                .logo {
                                                    width: 100px;
                                                }
                                                .payment-receipt {
                                                    text-align: center;
                                                    background: #0d6efd;
                                                    color: white;
                                                    padding: 10px;
                                                    font-weight: bold;
                                                    border-radius: 5px;
                                                    margin-bottom: 1.5rem;
                                                    width: 40%;
                                                    margin-left: auto;
                                                    margin-right: auto;
                                                }
                                                .tables {
                                                    width: 100%;
                                                    background-color: white;
                                                    border-collapse: collapse;
                                                }
                                                .tables td {
                                                    padding: 8px 15px;
                                                }
                                                .footer {
                                                    display: flex;
                                                    justify-content: space-between;
                                                    align-items: center;
                                                    margin-top: 20px;
                                                    font-weight: bold;
                                                    width: 100%;
                                                }
                                                .footer p {
                                                    flex: 1;
                                                    text-align: left;
                                                }
                                                .signature {
                                                    border-top: 1px dotted black;
                                                    text-align: center;
                                                    width: 100px;
                                                    flex: 1;
                                                    text-align: right;
                                                }
                                                .download-button {
                                                    text-align: center;
                                                    margin-top: 20px;
                                                }
                                                .download-button button {
                                                    padding: 10px 20px;
                                                    border: 1px solid #0d6efd;
                                                    background-color: white;
                                                    color: #0d6efd;
                                                    font-weight: bold;
                                                    cursor: pointer;
                                                    border-radius: 5px;
                                                }
                                                .download-button button:hover {
                                                    background-color: #0d6efd;
                                                    color: white;
                                                }
                                            </style>
                                        </head>
                                        <body>
                                            <div class='receipt-container'>
                                                <div class='header'>
                                                    <img src='https://placehold.co/800x800' alt='Company Logo' class='logo'>
                                                    <div class='company-details'>
                                                        <h4>Pay Nepal Pvt. Ltd.</h4>
                                                        <p>
                                                            Banepa, Nepal<br />
                                                            Tel: 11111, 1112222, 3333333<br />
                                                            Email: support@pay.com<br />
                                                            www.papa.com
                                                        </p>
                                                    </div>
                                                    <img src='https://placehold.co/800x800' alt='Service Logo' class='logo'>
                                                </div>

                                                <div class='payment-receipt rounded rounded-pill'>Payment Receipt</div>

                                                <table class='tables'>
                                                    <tr>
                                                        <td><strong>Date & Time:</strong> 1/15/2024 10:35:17 AM</td>
                                                        <td><strong>Amount:</strong> NPR. 1.00</td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Transaction ID:</strong> 00104247</td>
                                                        <td><strong>Reward Point:</strong> 0</td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Subscriber No.:</strong> 1231231231</td>
                                                        <td><strong>Status:</strong> <span style='color: red;'>Failed</span></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Product:</strong> Ncell Prepaid</td>
                                                        <td><strong>Payment Method:</strong> Wallet</td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Company:</strong> NCELL</td>
                                                        <td><strong>Remarks:</strong> Transaction Failed</td>
                                                    </tr>
                                                </table>

                                                <div class='footer'>
                                                    <p>Thank you for using papa pay.</p>
                                                    <div class='signature'>Authorized Signature</div>
                                                </div>
                                            </div>
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
                    FileName = $"document.pdf",
                    ContentType = "application/pdf"
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to generate PDF", ex);
            }
        }
    }
}