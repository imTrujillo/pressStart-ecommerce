using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shop.Domain.Entities;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Infrastructure.PDFs;

public class InvoiceDocument : IDocument
{
    private readonly Invoice _invoice;
    public InvoiceDocument(Invoice invoice)
    {
        _invoice = invoice;
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {   
            page.Size(PageSizes.Letter);
            page.Margin(50);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent); 
            
            page.Footer().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .Text("PressStart")
                    .FontSize(20)
                    .SemiBold()
                    .FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{_invoice.IssueDate:dd/MM/yyyy}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Invoice #").SemiBold();
                    text.Span($"{_invoice.InvoiceNumber}");
                });
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(column =>
        {
            column.Spacing(20);
            
            column.Item().Element(ComposeCustomerInfo);
            
            column.Item().Element(ComposeTable);
            
            // Total
            column.Item().AlignRight().Text(text =>
            {
                text.Span("Grand total: ").SemiBold().FontSize(14);
                text.Span($"${_invoice.Total:N2}").FontSize(14);
            });
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);      // # 
                columns.RelativeColumn(4);        // Product
                columns.RelativeColumn(2);        // Unit price
                columns.RelativeColumn(1.5f);     // Quantity
                columns.RelativeColumn(2);        // Total
            });
            
            // Header de la tabla
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Product");
                header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                header.Cell().Element(CellStyle).AlignRight().Text("Total");
                
                static IContainer CellStyle(IContainer container)
                {
                    return container
                        .DefaultTextStyle(x => x.SemiBold())
                        .PaddingVertical(5)
                        .BorderBottom(1)
                        .BorderColor(Colors.Black);
                }
            });

            // ✅ FILAS DE PRODUCTOS CON NUMERACIÓN
            var index = 1;
            foreach (var detail in _invoice.Order.Details)
            {
                var total = detail.Price * detail.Quantity;
                
                table.Cell().Element(CellStyle).Text(index.ToString());
                table.Cell().Element(CellStyle).Text(detail.Product.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"${detail.Price:N2}");
                table.Cell().Element(CellStyle).AlignCenter().Text(detail.Quantity.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text($"${total:N2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
                }
                
                index++;
            }
        });
    }
    
    private void ComposeCustomerInfo(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(3);
        
            column.Item()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Medium)
                .PaddingBottom(5)
                .Text("For")
                .SemiBold()
                .FontSize(12);
        
            column.Item().Text($"Full Name: {_invoice.UserName}");
            column.Item().Text($"DUI: {_invoice.UserDui}");
            
            if (!string.IsNullOrEmpty(_invoice.UserPhone))
                column.Item().Text($"Phone: {_invoice.UserPhone}");
                
            column.Item().Text($"Email: {_invoice.UserEmail}");
                
            column.Item().Text($"Address: {_invoice.UserAddress}");
        });
    }
}