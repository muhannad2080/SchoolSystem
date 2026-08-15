using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ITextFont = iTextSharp.text.Font;

namespace SchoolSystem.Helpers
{
    /// <summary>
    /// إخراج موحد للتقارير. لا تعتمد هذه الطبقة على واجهة محددة حتى تستخدمها جميع الشاشات.
    /// </summary>
    public static class ReportOutputHelper
    {
        private static readonly BaseColor Navy = new BaseColor(31, 41, 55);
        private static readonly BaseColor Accent = new BaseColor(37, 99, 235);
        private static readonly BaseColor Border = new BaseColor(203, 213, 225);
        private static readonly BaseColor Alternate = new BaseColor(248, 250, 252);
        private static readonly BaseColor White = new BaseColor(255, 255, 255);

        public static void ExportToExcel(DataTable table, string filePath, string title, string summary)
        {
            EnsureTable(table);
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Report");
                sheet.RightToLeft = ContainsArabic(title) || ContainsArabic(summary)
                    || ContainsArabicColumns(table) || ContainsArabicValues(table);

                int columnCount = Math.Max(1, table.Columns.Count);
                int headerRow = 6;
                sheet.Range(1, 1, 1, columnCount).Merge();
                sheet.Cell(1, 1).Value = title ?? "School System Report";
                StyleMergedHeader(sheet.Cell(1, 1), 16, Navy);

                sheet.Range(2, 1, 2, columnCount).Merge();
                sheet.Cell(2, 1).Value = summary ?? string.Empty;
                StyleMergedHeader(sheet.Cell(2, 1), 10, Accent);

                sheet.Range(3, 1, 3, columnCount).Merge();
                sheet.Cell(3, 1).Value = "Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                StyleMergedHeader(sheet.Cell(3, 1), 9, Navy);

                for (int column = 0; column < table.Columns.Count; column++)
                {
                    IXLCell cell = sheet.Cell(headerRow, column + 1);
                    cell.Value = table.Columns[column].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontName = "Tahoma";
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(Navy.R, Navy.G, Navy.B);
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.WrapText = true;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.FromArgb(Border.R, Border.G, Border.B);
                }

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    for (int column = 0; column < table.Columns.Count; column++)
                    {
                        IXLCell cell = sheet.Cell(headerRow + row + 1, column + 1);
                        object value = table.Rows[row][column];
                        cell.Value = value == DBNull.Value ? string.Empty : Convert.ToString(value);
                        cell.Style.Font.FontName = "Tahoma";
                        cell.Style.Alignment.Horizontal = IsNumeric(value)
                            ? XLAlignmentHorizontalValues.Center
                            : (ContainsArabic(Convert.ToString(value)) ? XLAlignmentHorizontalValues.Right : XLAlignmentHorizontalValues.Left);
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.WrapText = true;
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.OutsideBorderColor = XLColor.FromArgb(Border.R, Border.G, Border.B);
                        if (row % 2 == 1)
                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(Alternate.R, Alternate.G, Alternate.B);
                    }
                }

                if (table.Rows.Count > 0)
                {
                    sheet.Range(headerRow, 1, headerRow + table.Rows.Count, columnCount).CreateTable();
                }

                sheet.SheetView.FreezeRows(headerRow);
                sheet.Columns().AdjustToContents();
                sheet.Row(1).Height = 28;
                sheet.Row(2).Height = 22;
                sheet.Row(headerRow).Height = 30;
                workbook.SaveAs(filePath);
            }
        }

        public static void ExportToPdf(DataTable table, string filePath, string title, string summary)
        {
            EnsureTable(table);
            iTextSharp.text.Rectangle pageSize = table.Columns.Count > 10 ? PageSize.A3.Rotate() : PageSize.A4.Rotate();
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document document = new Document(pageSize, 28, 28, 42, 34))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();

                BaseFont baseFont = CreateBaseFont();
                ITextFont titleFont = new ITextFont(baseFont, 16f, ITextFont.BOLD);
                ITextFont summaryFont = new ITextFont(baseFont, 9f, ITextFont.NORMAL);
                ITextFont headerFont = new ITextFont(baseFont, table.Columns.Count > 10 ? 7f : 8f, ITextFont.BOLD);
                ITextFont cellFont = new ITextFont(baseFont, table.Columns.Count > 10 ? 6f : 8f, ITextFont.NORMAL);

                PdfPTable heading = new PdfPTable(1);
                heading.WidthPercentage = 100;
                heading.RunDirection = ContainsArabic(title) || ContainsArabic(summary)
                    ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
                AddTextCell(heading, title ?? "School System Report", titleFont, Navy, Element.ALIGN_CENTER);
                AddTextCell(heading, summary ?? string.Empty, summaryFont, White, Element.ALIGN_CENTER);
                AddTextCell(heading, "Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), summaryFont, White, Element.ALIGN_CENTER);
                document.Add(heading);
                document.Add(new Paragraph(" "));

                PdfPTable grid = new PdfPTable(table.Columns.Count);
                grid.WidthPercentage = 100;
                grid.RunDirection = ContainsArabicColumns(table) ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
                float[] widths = Enumerable.Repeat(1f, table.Columns.Count).ToArray();
                grid.SetWidths(widths);

                foreach (DataColumn column in table.Columns)
                    AddTextCell(grid, column.ColumnName, headerFont, Navy, Element.ALIGN_CENTER);

                foreach (DataRow row in table.Rows)
                {
                    foreach (object value in row.ItemArray)
                    {
                        string text = value == DBNull.Value ? string.Empty : Convert.ToString(value);
                        int direction = ContainsArabic(text) ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
                        AddTextCell(grid, text, cellFont, White, IsNumeric(value) ? Element.ALIGN_CENTER : direction == PdfWriter.RUN_DIRECTION_RTL ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT, direction);
                    }
                }

                document.Add(grid);
                document.Add(new Paragraph(" "));
                Paragraph footer = new Paragraph("SchoolSystem | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), summaryFont);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);
                document.Close();
            }
        }

        public static bool ContainsArabic(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return value.Any(c => (c >= '\u0600' && c <= '\u06FF') || (c >= '\u0750' && c <= '\u077F') || (c >= '\u08A0' && c <= '\u08FF'));
        }

        private static bool ContainsArabicColumns(DataTable table)
        {
            return table != null && table.Columns.Cast<DataColumn>().Any(c => ContainsArabic(c.ColumnName));
        }

        private static bool ContainsArabicValues(DataTable table)
        {
            if (table == null)
                return false;

            foreach (DataRow row in table.Rows)
            {
                foreach (object value in row.ItemArray)
                {
                    if (value != null && value != DBNull.Value && ContainsArabic(Convert.ToString(value)))
                        return true;
                }
            }

            return false;
        }

        private static bool IsNumeric(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;
            return decimal.TryParse(Convert.ToString(value), out _);
        }

        private static void EnsureTable(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (table.Columns.Count == 0)
                throw new InvalidOperationException("لا توجد أعمدة في البيانات.");
        }

        private static void StyleMergedHeader(IXLCell cell, int size, BaseColor color)
        {
            cell.Style.Font.FontName = "Tahoma";
            cell.Style.Font.FontSize = size;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(color.R, color.G, color.B);
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Alignment.WrapText = true;
        }

        private static void AddTextCell(PdfPTable table, string text, ITextFont font, BaseColor background, int alignment, int direction = PdfWriter.RUN_DIRECTION_DEFAULT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text ?? string.Empty, font));
            cell.BackgroundColor = background;
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.RunDirection = direction;
            cell.Padding = 5;
            cell.BorderColor = Border;
            table.AddCell(cell);
        }

        private static BaseFont CreateBaseFont()
        {
            string fonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string[] candidates =
            {
                Path.Combine(fonts, "tahoma.ttf"),
                Path.Combine(fonts, "arial.ttf"),
                Path.Combine(fonts, "segoeui.ttf")
            };
            string fontPath = candidates.FirstOrDefault(File.Exists);
            return string.IsNullOrEmpty(fontPath)
                ? BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
                : BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}
