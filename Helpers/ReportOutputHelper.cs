using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ITextFont = iTextSharp.text.Font;
using ITextImage = iTextSharp.text.Image;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Security;


namespace SchoolSystem.Helpers
{
    /// <summary>
    /// إخراج موحد للتقارير. لا تعتمد هذه الطبقة على واجهة محددة حتى تستخدمها جميع الشاشات.
    /// </summary>

    public static class ReportOutputHelper
    {
        // تعريف الدالة BaseColor لتنفيذ العملية المرتبطة بها.
        private static readonly BaseColor Navy = new BaseColor(22, 50, 79);
        // تعريف الدالة BaseColor لتنفيذ العملية المرتبطة بها.
        private static readonly BaseColor Accent = new BaseColor(15, 118, 110);
        // تعريف الدالة BaseColor لتنفيذ العملية المرتبطة بها.
        private static readonly BaseColor Border = new BaseColor(203, 213, 225);
        // تعريف الدالة BaseColor لتنفيذ العملية المرتبطة بها.
        private static readonly BaseColor Alternate = new BaseColor(248, 250, 252);
        // تعريف الدالة BaseColor لتنفيذ العملية المرتبطة بها.
        private static readonly BaseColor White = new BaseColor(255, 255, 255);

        // تعريف الدالة ExportToExcel لتنفيذ العملية المرتبطة بها.
        public static void ExportToExcel(DataTable table, string filePath, string title, string summary)
        {
            EnsureTable(table);
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("التقرير");


                sheet.RightToLeft = true;

                int columnCount = Math.Max(1, table.Columns.Count);
                int headerRow = 7;
                SchoolSettings branding = SchoolSettingsService.Current();
                AddExcelLogo(sheet, branding);
                string schoolName = GetBilingualSchoolName(branding);
                string organization = GetBilingualOrganizationLine(branding);
                string generatedBy = GetCurrentOperatorDisplayName();
                sheet.Range(1, 1, 1, columnCount).Merge();
                sheet.Cell(1, 1).Value = schoolName;
                StyleMergedHeader(sheet.Cell(1, 1), 15, Navy);

                sheet.Range(2, 1, 2, columnCount).Merge();
                sheet.Cell(2, 1).Value = organization;
                StyleMergedHeader(sheet.Cell(2, 1), 10, Accent);

                sheet.Range(3, 1, 3, columnCount).Merge();
                sheet.Cell(3, 1).Value = string.IsNullOrWhiteSpace(title) ? "تقرير النظام" : title;
                StyleMergedHeader(sheet.Cell(3, 1), 13, Navy);

                sheet.Range(4, 1, 4, columnCount).Merge();
                sheet.Cell(4, 1).Value = summary ?? string.Empty;
                StyleMergedHeader(sheet.Cell(4, 1), 10, Accent);

                sheet.Range(5, 1, 5, columnCount).Merge();
                sheet.Cell(5, 1).Value = "تاريخ الإنشاء: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                StyleMergedHeader(sheet.Cell(5, 1), 9, Navy);

                sheet.Range(6, 1, 6, columnCount).Merge();
                sheet.Cell(6, 1).Value = "المستخدم المنفذ: " + generatedBy;
                StyleMergedHeader(sheet.Cell(6, 1), 9, Accent);
                 

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
                            : XLAlignmentHorizontalValues.Right;
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

                for (int column = 1; column <= columnCount; column++)
                    sheet.Column(column).Width = Math.Min(34d, Math.Max(12d, sheet.Column(column).Width));
                sheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                sheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sheet.PageSetup.SetRowsToRepeatAtTop(1, headerRow);
                sheet.PageSetup.Margins.Top = 0.35;
                sheet.PageSetup.Margins.Bottom = 0.35;
                sheet.PageSetup.Margins.Left = 0.25;
                sheet.PageSetup.Margins.Right = 0.25;
                sheet.Row(1).Height = 38;
                sheet.Row(2).Height = 32;
                sheet.Row(headerRow).Height = 30;
                workbook.SaveAs(filePath);
            }
        }

        // تعريف الدالة ExportToPdf لتنفيذ العملية المرتبطة بها.
        public static void ExportToPdf(DataTable table, string filePath, string title, string summary)
        {
            EnsureTable(table);
            iTextSharp.text.Rectangle pageSize = table.Columns.Count > 10 ? PageSize.A3.Rotate() : PageSize.A4.Rotate();
            SchoolSettings branding = SchoolSettingsService.Current();
            string safeTitle = string.IsNullOrWhiteSpace(title) ? "تقرير النظام" : title;
            string safeSummary = string.IsNullOrWhiteSpace(summary) ? string.Empty : summary.Trim();
            safeSummary = safeSummary.Length == 0
                ? "المستخدم المنفذ: " + GetCurrentOperatorDisplayName()
                : safeSummary + " | المستخدم المنفذ: " + GetCurrentOperatorDisplayName();

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document document = new Document(pageSize, 28, 28, 150, 42))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                writer.PageEvent = new SchoolPdfPageEvent(branding, safeTitle, safeSummary);

                document.Open();

                document.AddTitle(safeTitle);

                document.AddAuthor(GetSchoolName(branding));

                document.AddSubject(safeSummary);

                BaseFont baseFont = CreateBaseFont();
                ITextFont headerFont = new ITextFont(baseFont, table.Columns.Count > 10 ? 7f : 8f, ITextFont.BOLD);
                ITextFont cellFont = new ITextFont(baseFont, table.Columns.Count > 10 ? 6f : 8f, ITextFont.NORMAL);

                bool tableRtl = true;
                PdfPTable grid = new PdfPTable(table.Columns.Count)
                {
                    WidthPercentage = 100,
                    RunDirection = tableRtl ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR,
                    HeaderRows = 1,
                    SplitLate = false,
                    SplitRows = true,
                    SpacingBefore = 4f,
                    SpacingAfter = 8f

                };
                grid.SetWidths(Enumerable.Repeat(1f, table.Columns.Count).ToArray());

                foreach (DataColumn column in table.Columns)
                    AddTextCell(grid, column.ColumnName, headerFont, Navy, Element.ALIGN_CENTER,
                        PdfWriter.RUN_DIRECTION_RTL);

                int pdfRowIndex = 0;
                foreach (DataRow row in table.Rows)
                {
                    BaseColor rowColor = pdfRowIndex % 2 == 1 ? Alternate : White;
                    foreach (object value in row.ItemArray)
                    {
                        string text = value == DBNull.Value ? string.Empty : Convert.ToString(value);
                        int direction = ContainsArabic(text) ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
                        int alignment = IsNumeric(value) ? Element.ALIGN_CENTER
                            : direction == PdfWriter.RUN_DIRECTION_RTL ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        AddTextCell(grid, text, cellFont, rowColor, alignment, direction);
                    }
                    pdfRowIndex++;
                }


                document.Add(grid);

                document.Close();
            }
        }

        // تعريف الصنف SchoolPdfPageEvent المسؤول عن وظائف طبقة Helpers.
        private sealed class SchoolPdfPageEvent : PdfPageEventHelper
        {

            private readonly SchoolSettings settings;

            private readonly string title;

            private readonly string summary;

            private readonly BaseFont baseFont;

            // تعريف الدالة SchoolPdfPageEvent لتنفيذ العملية المرتبطة بها.
            public SchoolPdfPageEvent(SchoolSettings settings, string title, string summary)
            {
                this.settings = settings;
                this.title = title;
                this.summary = summary;
                baseFont = CreateBaseFont();
            }

            // تعريف الدالة OnEndPage لتنفيذ العملية المرتبطة بها.
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                float width = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                PdfPTable header = new PdfPTable(1)
                {
                    TotalWidth = width,
                    LockedWidth = true,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
                };

                ITextFont schoolFont = new ITextFont(baseFont, 16f, ITextFont.BOLD, Navy);
                ITextFont englishFont = new ITextFont(baseFont, 8.5f, ITextFont.NORMAL, Accent);
                ITextFont detailFont = new ITextFont(baseFont, 8f, ITextFont.NORMAL, Navy);
                ITextFont titleFont = new ITextFont(baseFont, 12f, ITextFont.BOLD, White);
                ITextFont summaryFont = new ITextFont(baseFont, 8f, ITextFont.NORMAL, Navy);

                PdfPCell brandCell = new PdfPCell
                {
                    BackgroundColor = White,
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    BorderWidthBottom = 2f,
                    BorderColor = Accent,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 5f,
                    PaddingBottom = 6f,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
                };

                if (settings != null && settings.Logo != null && settings.Logo.Length > 0)
                {
                    try
                    {
                        ITextImage logo = ITextImage.GetInstance(settings.Logo);
                        logo.ScaleToFit(62f, 62f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        brandCell.AddElement(logo);
                    }
                    catch (Exception logoEx)
                    {
                        ApplicationLogger.LogException("إدراج شعار المدرسة في ترويسة PDF", logoEx);
                    }
                }

                string arabicSchool = settings == null ? string.Empty : settings.SchoolNameAr;
                string englishSchool = settings == null ? string.Empty : settings.SchoolNameEn;
                string arabicOrganization = settings == null ? string.Empty : BuildOrganizationLine(settings, true);
                string englishOrganization = settings == null ? string.Empty : BuildOrganizationLine(settings, false);

                if (!string.IsNullOrWhiteSpace(arabicSchool))
                    brandCell.AddElement(CreateCenteredParagraph(arabicSchool.Trim(), schoolFont, PdfWriter.RUN_DIRECTION_RTL));

                if (!string.IsNullOrWhiteSpace(englishSchool))
                    brandCell.AddElement(CreateCenteredParagraph(englishSchool.Trim(), englishFont, PdfWriter.RUN_DIRECTION_LTR));

                if (!string.IsNullOrWhiteSpace(arabicOrganization))
                    brandCell.AddElement(CreateCenteredParagraph(arabicOrganization, detailFont, PdfWriter.RUN_DIRECTION_RTL));

                if (!string.IsNullOrWhiteSpace(englishOrganization))
                    brandCell.AddElement(CreateCenteredParagraph(englishOrganization, detailFont, PdfWriter.RUN_DIRECTION_LTR));

                header.AddCell(brandCell);

                PdfPCell titleCell = new PdfPCell(new Phrase(title ?? "تقرير النظام", titleFont))
                {
                    BackgroundColor = Navy,
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    PaddingTop = 7f,
                    PaddingBottom = 7f
                };
                header.AddCell(titleCell);

                if (!string.IsNullOrWhiteSpace(summary))
                {
                    PdfPCell summaryCell = new PdfPCell(new Phrase(summary, summaryFont))
                    {
                        BackgroundColor = Alternate,
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                        PaddingTop = 5f,
                        PaddingBottom = 5f
                    };
                    header.AddCell(summaryCell);
                }

                header.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 12f, writer.DirectContent);

                string footerText = GetSchoolName(settings) + " | " + (title ?? "تقرير النظام") +
                                    " | المستخدم: " + GetCurrentOperatorDisplayName() +
                                    " | الصفحة " + writer.PageNumber;
                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = width,
                    LockedWidth = true,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
                };

                PdfPCell footerCell = new PdfPCell(new Phrase(footerText, detailFont))
                {
                    BackgroundColor = White,
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    BorderWidthTop = 1f,
                    BorderColor = Border,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    PaddingTop = 4f,
                    PaddingBottom = 1f
                };
                footerTable.AddCell(footerCell);
                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, 34f, writer.DirectContent);
            }

            private static Paragraph CreateCenteredParagraph(string text, ITextFont font, int direction)
            {
                Paragraph paragraph = new Paragraph(text ?? string.Empty, font)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                paragraph.SetLeading(0f, 1.12f);
                return paragraph;
            }
        }

        public static string GetCurrentOperatorDisplayName()
        {
            try
            {
                if (CurrentUser.User == null)
                    return "غير معروف";

                string name = (CurrentUser.User.FullName ?? string.Empty).Trim();
                string userName = (CurrentUser.User.UserName ?? string.Empty).Trim();

                if (name.Length > 0 && userName.Length > 0)
                    return name + " (" + userName + ")";
                if (name.Length > 0)
                    return name;
                if (userName.Length > 0)
                    return userName;

                return "غير معروف";
            }
            catch
            {
                return "غير معروف";
            }
        }

        // تعريف الدالة AddExcelLogo لتنفيذ العملية المرتبطة بها.
        private static void AddExcelLogo(IXLWorksheet sheet, SchoolSettings settings)
        {
            if (settings == null || settings.Logo == null || settings.Logo.Length == 0)
                return;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                using (MemoryStream stream = new MemoryStream(settings.Logo))
                {
                    sheet.AddPicture(stream)
                        .MoveTo(sheet.Cell(1, 1))
                        .Scale(0.35);
                }
            }

            catch (Exception logoEx)
            {
                // الشعار تكميلي؛ فشل إدراجه في Excel يُسجل ولا يوقف التصدير.
                ApplicationLogger.LogException("إدراج شعار المدرسة في ترويسة Excel", logoEx);
            }
        }

        // تعريف الدالة AddPdfLogo لتنفيذ العملية المرتبطة بها.
        private static void AddPdfLogo(Document document, SchoolSettings settings)
        {
            if (settings == null || settings.Logo == null || settings.Logo.Length == 0)
                return;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                ITextImage logo = ITextImage.GetInstance(settings.Logo);
                logo.ScaleToFit(72f, 72f);
                logo.Alignment = Element.ALIGN_CENTER;

                document.Add(logo);
            }

            catch (Exception logoEx)
            {
                // الشعار تكميلي؛ فشل إدراجه في PDF يُسجل ولا يوقف التقرير.
                ApplicationLogger.LogException("إدراج شعار المدرسة في PDF", logoEx);
            }
        }

        // تعريف الدالة DrawSchoolPrintHeader لتنفيذ العملية المرتبطة بها.
        public static int DrawSchoolPrintHeader(Graphics graphics, System.Drawing.Rectangle bounds, string title, bool rtl)
        {
            if (graphics == null)

                throw new ArgumentNullException("graphics");

            SchoolSettings branding = SchoolSettingsService.Current();
            int top = bounds.Top;
            int headerHeight = 138;
            StringFormat format = new StringFormat
            {
                Alignment = rtl ? StringAlignment.Far : StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = rtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter

            };
            using (format)
            using (System.Drawing.Font schoolFont = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold))
            using (System.Drawing.Font organizationFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular))
            using (System.Drawing.Font titleFont = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold))
            using (Pen linePen = new Pen(Color.FromArgb(22, 50, 79), 2F))
            using (Pen accentLinePen = new Pen(Color.FromArgb(180, 145, 50), 1F))
            using (Brush navyBrush = new SolidBrush(Color.FromArgb(22, 50, 79)))
            using (Brush accentBrush = new SolidBrush(Color.FromArgb(15, 118, 110)))
            {
                int logoSize = 66;
                int logoX = rtl ? bounds.Right - logoSize : bounds.Left;
                int textLeft = rtl ? bounds.Left : bounds.Left + logoSize + 12;
                int textRight = rtl ? bounds.Right - logoSize - 12 : bounds.Right;
                int textWidth = Math.Max(100, textRight - textLeft);

                if (branding != null && branding.Logo != null && branding.Logo.Length > 0)
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(branding.Logo))
                        using (System.Drawing.Image source = System.Drawing.Image.FromStream(stream))
                        using (Bitmap logo = new Bitmap(source))
                        {
                            graphics.DrawImage(logo, new System.Drawing.Rectangle(logoX, top, logoSize, logoSize));
                        }
                    }

                    catch (Exception logoEx)
                    {
                        // الشعار تكميلي؛ فشل رسمه في ترويسة الطباعة يُسجل ولا يوقف الطباعة.
                        ApplicationLogger.LogException("رسم شعار المدرسة في ترويسة الطباعة", logoEx);
                    }
                }

                string arabicSchool = branding == null ? string.Empty : branding.SchoolNameAr;
                string englishSchool = branding == null ? string.Empty : branding.SchoolNameEn;
                string arabicOrganization = branding == null ? string.Empty : BuildOrganizationLine(branding, true);
                string englishOrganization = branding == null ? string.Empty : BuildOrganizationLine(branding, false);
                using (StringFormat arabicFormat = CreatePrintFormat(true))
                using (StringFormat englishFormat = CreatePrintFormat(false))
                {
                    if (!string.IsNullOrWhiteSpace(arabicSchool))
                        graphics.DrawString(arabicSchool.Trim(), schoolFont, navyBrush,

                            new System.Drawing.Rectangle(textLeft, top, textWidth, 28), arabicFormat);
                    if (!string.IsNullOrWhiteSpace(englishSchool))
                        graphics.DrawString(englishSchool.Trim(), organizationFont, accentBrush,

                            new System.Drawing.Rectangle(textLeft, top + 29, textWidth, 22), englishFormat);
                    if (!string.IsNullOrWhiteSpace(arabicOrganization))
                        graphics.DrawString(arabicOrganization, organizationFont, accentBrush,

                            new System.Drawing.Rectangle(textLeft, top + 52, textWidth, 22), arabicFormat);
                    if (!string.IsNullOrWhiteSpace(englishOrganization))
                        graphics.DrawString(englishOrganization, organizationFont, accentBrush,

                            new System.Drawing.Rectangle(textLeft, top + 75, textWidth, 22), englishFormat);
                    graphics.DrawString("المستخدم المنفذ: " + GetCurrentOperatorDisplayName(), organizationFont, accentBrush,
                        new System.Drawing.Rectangle(bounds.Left, top + 98, bounds.Width, 20), arabicFormat);
                    graphics.DrawString(title ?? string.Empty, titleFont, navyBrush,
                        new System.Drawing.Rectangle(bounds.Left, top + 114, bounds.Width, 30), arabicFormat);
                }
                graphics.DrawLine(linePen, bounds.Left, top + headerHeight, bounds.Right, top + headerHeight);
                graphics.DrawLine(accentLinePen, bounds.Left, top + headerHeight + 3, bounds.Right, top + headerHeight + 3);
            }


            return top + headerHeight + 14;
        }

        // تعريف الدالة CreatePrintFormat لتنفيذ العملية المرتبطة بها.
        private static StringFormat CreatePrintFormat(bool rtl)
        {

            return new StringFormat
            {
                Alignment = rtl ? StringAlignment.Far : StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = rtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter

            };
        }

        // تعريف الدالة GetSchoolName لتنفيذ العملية المرتبطة بها.
        public static string GetSchoolName(SchoolSettings settings)
        {
            if (settings == null)
                return "نظام إدارة المدرسة";

            return !string.IsNullOrWhiteSpace(settings.SchoolNameAr)
                ? settings.SchoolNameAr
                : (settings.SchoolNameEn ?? "School System");
        }

        // تعريف الدالة GetBilingualSchoolName لتنفيذ العملية المرتبطة بها.
        public static string GetBilingualSchoolName(SchoolSettings settings)
        {
            if (settings == null)
                return "نظام إدارة المدرسة";
            string arabic = settings.SchoolNameAr ?? string.Empty;
            string english = settings.SchoolNameEn ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(arabic) && !string.IsNullOrWhiteSpace(english))
                return arabic.Trim() + "\n" + english.Trim();

            return !string.IsNullOrWhiteSpace(arabic) ? arabic.Trim() : english.Trim();
        }

        // تعريف الدالة GetBilingualOrganizationLine لتنفيذ العملية المرتبطة بها.
        public static string GetBilingualOrganizationLine(SchoolSettings settings)
        {
            if (settings == null)
                return string.Empty;
            string arabic = BuildOrganizationLine(settings, true);
            string english = BuildOrganizationLine(settings, false);
            if (!string.IsNullOrWhiteSpace(arabic) && !string.IsNullOrWhiteSpace(english))
                return arabic + "\n" + english;

            return !string.IsNullOrWhiteSpace(arabic) ? arabic : english;
        }

        // تعريف الدالة BuildOrganizationLine لتنفيذ العملية المرتبطة بها.
        private static string BuildOrganizationLine(SchoolSettings settings, bool arabic)
        {
            string organization = arabic ? settings.OrganizationNameAr : settings.OrganizationNameEn;
            string address = arabic ? settings.AddressAr : settings.AddressEn;
            string contact = string.Join(" | ", new[]
            {
                settings.Phone,
                settings.AdditionalPhone,
                settings.Email,
                settings.Website
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return string.Join(" | ", new[] { organization, address, contact }
                .Where(value => !string.IsNullOrWhiteSpace(value)));
        }

        // تعريف الدالة GetOrganizationLine لتنفيذ العملية المرتبطة بها.
        public static string GetOrganizationLine(SchoolSettings settings)
        {
            if (settings == null)
                return string.Empty;
            string organization = !string.IsNullOrWhiteSpace(settings.OrganizationNameAr)
                ? settings.OrganizationNameAr
                : settings.OrganizationNameEn;
            string address = !string.IsNullOrWhiteSpace(settings.AddressAr)
                ? settings.AddressAr
                : settings.AddressEn;
            string contact = string.Join(" | ", new[]
            {
                settings.Phone,
                settings.AdditionalPhone,
                settings.Email,
                settings.Website
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            string line = string.Join(" | ", new[]
            {
                organization,
                address,
                contact
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return line ?? string.Empty;
        }

        // تعريف الدالة ContainsArabic لتنفيذ العملية المرتبطة بها.
        public static bool ContainsArabic(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.Any(c => (c >= '\u0600' && c <= '\u06FF') || (c >= '\u0750' && c <= '\u077F') || (c >= '\u08A0' && c <= '\u08FF'));
        }

        // تعريف الدالة ContainsArabicColumns لتنفيذ العملية المرتبطة بها.
        private static bool ContainsArabicColumns(DataTable table)
        {

            return table != null && table.Columns.Cast<DataColumn>().Any(c => ContainsArabic(c.ColumnName));
        }

        // تعريف الدالة ContainsArabicValues لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة IsNumeric لتنفيذ العملية المرتبطة بها.
        private static bool IsNumeric(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            return decimal.TryParse(Convert.ToString(value), out _);
        }

        // تعريف الدالة EnsureTable لتنفيذ العملية المرتبطة بها.
        private static void EnsureTable(DataTable table)
        {
            if (table == null)

                throw new ArgumentNullException("table");

            if (table.Columns.Count == 0)

                throw new InvalidOperationException("لا توجد أعمدة في البيانات.");
        }

        // تعريف الدالة StyleMergedHeader لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddTextCell لتنفيذ العملية المرتبطة بها.
        private static void AddTextCell(PdfPTable table, string text, ITextFont font, BaseColor background, int alignment, int direction = PdfWriter.RUN_DIRECTION_DEFAULT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text ?? string.Empty, font))
            {
                BackgroundColor = background,
                HorizontalAlignment = alignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                RunDirection = direction,
                Padding = 5,
                BorderColor = Border

            };
            table.AddCell(cell);
        }

        // تعريف الدالة CreateBaseFont لتنفيذ العملية المرتبطة بها.
        private static BaseFont CreateBaseFont()
        {
            string fonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string[] candidates =
            {
                Path.Combine(fonts, "tahoma.ttf"),
                Path.Combine(fonts, "arial.ttf"),
                Path.Combine(fonts, "segoeui.ttf"),
                "/usr/share/fonts/truetype/noto/NotoNaskhArabic-Regular.ttf",
                "/usr/share/fonts/truetype/noto/NotoSansArabic-Regular.ttf",
                "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf",
                "/usr/share/fonts/truetype/freefont/FreeSerif.ttf"

            };
            string fontPath = candidates.FirstOrDefault(File.Exists);

            return string.IsNullOrEmpty(fontPath)
                ? BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
                : BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}
