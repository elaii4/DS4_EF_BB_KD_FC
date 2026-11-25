using System;
using System.Collections.Generic;
using System.IO;
using Api.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Api.Services;

// PdfReportService
// Generador de reportes en PDF con PdfSharpCore.
// Replica el formato de la vista de lista con columnas ID, Nombre Completo y Edad.
public static class PdfReportService
{
    public static byte[] GeneratePeopleReport(IEnumerable<Person> people, string serverName, string databaseName, string? searchFilter = null)
    {
        using var document = new PdfDocument();
        document.Info.Title = "Reporte - Directorio de Personas";
        document.Info.Author = "DS4 Hub - Sistema Desacoplado";
        document.Info.Subject = "Listado de Personas";

        var page = document.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        page.Orientation = PdfSharpCore.PageOrientation.Portrait;
        var gfx = XGraphics.FromPdfPage(page);

        // Fuentes compatibles estándar de PdfSharpCore
        var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
        var fontSubtitle = new XFont("Arial", 10, XFontStyle.Regular);
        var fontMeta = new XFont("Arial", 9, XFontStyle.Regular);
        var fontMetaBold = new XFont("Arial", 9, XFontStyle.Bold);
        var fontTableHeader = new XFont("Arial", 10, XFontStyle.Bold);
        var fontTableRow = new XFont("Arial", 9, XFontStyle.Regular);
        var fontTableId = new XFont("Arial", 9, XFontStyle.Bold);

        // Paleta de colores minimalista profesional
        var colorDark = XColor.FromArgb(15, 23, 42);       // #0f172a
        var colorSlate = XColor.FromArgb(71, 85, 105);     // #475569
        var colorLightBg = XColor.FromArgb(248, 250, 252);  // #f8fafc
        var colorBorder = XColor.FromArgb(226, 232, 240);   // #e2e8f0

        var brushDark = new XSolidBrush(colorDark);
        var brushSlate = new XSolidBrush(colorSlate);
        var brushWhite = XBrushes.White;
        var penBorder = new XPen(colorBorder, 1);
        var penHeaderLine = new XPen(colorDark, 1.5);

        double marginX = 40;
        double currentY = 40;
        double printableWidth = page.Width - (2 * marginX);

        // 1. Encabezado
        gfx.DrawString("DIRECTORIO DE PERSONAS", fontTitle, brushDark, marginX, currentY);
        currentY += 18;
        gfx.DrawString("Reporte oficial de registros en sistema desacoplado (.NET 8 & PostgreSQL)", fontSubtitle, brushSlate, marginX, currentY);
        currentY += 14;

        // Línea divisoria
        gfx.DrawLine(penHeaderLine, marginX, currentY, marginX + printableWidth, currentY);
        currentY += 12;

        // 2. Caja informativa de metadatos del servidor
        double metaBoxHeight = 44;
        var rectMeta = new XRect(marginX, currentY, printableWidth, metaBoxHeight);
        gfx.DrawRoundedRectangle(new XSolidBrush(colorLightBg), rectMeta, new XSize(8, 8));
        gfx.DrawRoundedRectangle(penBorder, rectMeta, new XSize(8, 8));

        var peopleList = new List<Person>(people);

        double metaCol1 = marginX + 18;
        double metaCol2 = marginX + 270;
        double metaY1 = currentY + 16;
        double metaY2 = currentY + 32;

        // Columna 1: Datos del entorno / servidor
        gfx.DrawString("Servidor Activo:", fontMeta, brushSlate, metaCol1, metaY1);
        gfx.DrawString(serverName, fontMetaBold, brushDark, metaCol1 + 85, metaY1);

        gfx.DrawString("Base de Datos:", fontMeta, brushSlate, metaCol1, metaY2);
        gfx.DrawString(databaseName, fontMetaBold, brushDark, metaCol1 + 85, metaY2);

        // Columna 2: Emisión y recuento
        gfx.DrawString("Fecha de Emisión:", fontMeta, brushSlate, metaCol2, metaY1);
        gfx.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), fontMetaBold, brushDark, metaCol2 + 95, metaY1);

        gfx.DrawString("Total Registros:", fontMeta, brushSlate, metaCol2, metaY2);
        gfx.DrawString($"{peopleList.Count} persona(s)", fontMetaBold, brushDark, metaCol2 + 95, metaY2);

        currentY += metaBoxHeight + 20;

        // 3. Tabla de Datos idéntica a la vista de lista web (ID, Nombre Completo, Edad)
        double colIdWidth = 70;
        double colAgeWidth = 110;
        double colNameWidth = printableWidth - colIdWidth - colAgeWidth;

        double tableX = marginX;
        double headerHeight = 26;

        var headerRect = new XRect(tableX, currentY, printableWidth, headerHeight);
        gfx.DrawRoundedRectangle(new XSolidBrush(colorDark), headerRect, new XSize(4, 4));

        var formatCenter = new XStringFormat { Alignment = XStringAlignment.Center, LineAlignment = XLineAlignment.Center };
        var formatLeft = new XStringFormat { Alignment = XStringAlignment.Near, LineAlignment = XLineAlignment.Center };

        gfx.DrawString("ID", fontTableHeader, brushWhite, new XRect(tableX, currentY, colIdWidth, headerHeight), formatCenter);
        gfx.DrawString("NOMBRE COMPLETO", fontTableHeader, brushWhite, new XRect(tableX + colIdWidth + 12, currentY, colNameWidth, headerHeight), formatLeft);
        gfx.DrawString("EDAD", fontTableHeader, brushWhite, new XRect(tableX + colIdWidth + colNameWidth, currentY, colAgeWidth, headerHeight), formatCenter);

        currentY += headerHeight;

        double rowHeight = 24;
        int rowIndex = 0;
        int pageNumber = 1;

        if (peopleList.Count == 0)
        {
            var noDataRect = new XRect(tableX, currentY, printableWidth, 35);
            gfx.DrawRectangle(new XSolidBrush(colorLightBg), noDataRect);
            gfx.DrawRectangle(penBorder, noDataRect);
            gfx.DrawString("No se encontraron registros para generar el reporte.", fontTableRow, brushSlate, noDataRect, formatCenter);
            currentY += 35;
        }
        else
        {
            foreach (var person in peopleList)
            {
                if (currentY + rowHeight > page.Height - 50)
                {
                    DrawFooter(gfx, page, pageNumber);
                    page = document.AddPage();
                    page.Size = PdfSharpCore.PageSize.A4;
                    gfx = XGraphics.FromPdfPage(page);
                    pageNumber++;
                    currentY = 40;

                    headerRect = new XRect(tableX, currentY, printableWidth, headerHeight);
                    gfx.DrawRoundedRectangle(new XSolidBrush(colorDark), headerRect, new XSize(4, 4));
                    gfx.DrawString("ID", fontTableHeader, brushWhite, new XRect(tableX, currentY, colIdWidth, headerHeight), formatCenter);
                    gfx.DrawString("NOMBRE COMPLETO", fontTableHeader, brushWhite, new XRect(tableX + colIdWidth + 12, currentY, colNameWidth, headerHeight), formatLeft);
                    gfx.DrawString("EDAD", fontTableHeader, brushWhite, new XRect(tableX + colIdWidth + colNameWidth, currentY, colAgeWidth, headerHeight), formatCenter);
                    currentY += headerHeight;
                }

                var rowBrush = (rowIndex % 2 == 0) ? XBrushes.White : new XSolidBrush(colorLightBg);
                var rowRect = new XRect(tableX, currentY, printableWidth, rowHeight);
                gfx.DrawRectangle(rowBrush, rowRect);
                gfx.DrawLine(penBorder, tableX, currentY + rowHeight, tableX + printableWidth, currentY + rowHeight);

                // Columna ID
                gfx.DrawString($"#{person.Id}", fontTableId, brushDark, new XRect(tableX, currentY, colIdWidth, rowHeight), formatCenter);

                // Columna Nombre Completo
                gfx.DrawString(person.Name, fontTableRow, brushDark, new XRect(tableX + colIdWidth + 12, currentY, colNameWidth - 12, rowHeight), formatLeft);

                // Columna Edad
                gfx.DrawString($"{person.Age} años", fontTableRow, brushSlate, new XRect(tableX + colIdWidth + colNameWidth, currentY, colAgeWidth, rowHeight), formatCenter);

                currentY += rowHeight;
                rowIndex++;
            }
        }

        DrawFooter(gfx, page, pageNumber);

        using var stream = new MemoryStream();
        document.Save(stream, false);
        return stream.ToArray();
    }

    private static void DrawFooter(XGraphics gfx, PdfPage page, int pageNumber)
    {
        double marginX = 40;
        double footerY = page.Height - 30;
        var fontFooter = new XFont("Arial", 8, XFontStyle.Regular);
        var brushSlate = new XSolidBrush(XColor.FromArgb(148, 163, 184));
        var penSubtle = new XPen(XColor.FromArgb(226, 232, 240), 0.8);

        gfx.DrawLine(penSubtle, marginX, footerY - 8, page.Width - marginX, footerY - 8);
        gfx.DrawString("DS4 Hub • Reporte generado con PdfSharp (.NET 8)", fontFooter, brushSlate, marginX, footerY);

        var formatRight = new XStringFormat { Alignment = XStringAlignment.Far, LineAlignment = XLineAlignment.Near };
        gfx.DrawString($"Página {pageNumber}", fontFooter, brushSlate, new XRect(page.Width - marginX - 100, footerY - 8, 100, 15), formatRight);
    }
}
