using AngleSharp.Html.Parser;
using Markdig;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Assignment8.Services
{
    public class PdfGeneratorService
    {
        public byte[] GeneratePdfFromMarkdown(string markdownContent)
        {
            // Convert markdown to HTML
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var htmlContent = Markdown.ToHtml(markdownContent, pipeline);

            // Generate PDF using QuestPDF
            var pdfBytes = GeneratePdfFromHtml(htmlContent);

            return pdfBytes;
        }

        private byte[] GeneratePdfFromHtml(string htmlContent)
        {
            using var stream = new MemoryStream();

            // Parse the HTML content
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlContent);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Generated Itinerary").SemiBold().FontSize(20).AlignCenter();

                    page.Content().Column(column =>
                    {
                        // Iterate through the parsed HTML elements and add them to the PDF
                        foreach (var element in document.Body.Children)
                        {
                            switch (element.TagName)
                            {
                                case "P":
                                    column.Item().Text(element.TextContent);
                                    break;

                                case "H1":
                                    column.Item().Text(element.TextContent).SemiBold().FontSize(20);
                                    break;

                                case "H2":
                                    column.Item().Text(element.TextContent).SemiBold().FontSize(18);
                                    break;

                                case "H3":
                                    column.Item().Text(element.TextContent).SemiBold().FontSize(16);
                                    break;

                                case "UL":
                                    foreach (var listItem in element.Children)
                                    {
                                        if (listItem.TagName == "LI")
                                        {
                                            column.Item().Text($"• {listItem.TextContent}");
                                        }
                                    }
                                    break;

                                case "OL":
                                    int index = 1;
                                    foreach (var listItem in element.Children)
                                    {
                                        if (listItem.TagName == "LI")
                                        {
                                            column.Item().Text($"{index}. {listItem.TextContent}");
                                            index++;
                                        }
                                    }
                                    break;

                                // You can add more cases here to support additional HTML elements.
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            })
            .GeneratePdf(stream);

            return stream.ToArray();
        }
    }
}
