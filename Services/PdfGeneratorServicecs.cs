using DinkToPdf;
using DinkToPdf.Contracts;
using Markdig;

namespace Assignment8.Services
{
    public class PdfGeneratorService
    {
        private readonly IConverter _converter;

        public PdfGeneratorService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdfFromMarkdown(string markdownContent)
        {
            // Convert markdown to HTML
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var htmlContent = Markdown.ToHtml(markdownContent, pipeline);

            // Create a new HTML-to-PDF document
            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" },
                    }
                }
            };

            // Convert the HTML content to PDF
            return _converter.Convert(pdfDoc);
        }
    }
}
