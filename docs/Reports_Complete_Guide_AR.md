# الدليل الشامل لمنظومة التقارير في SchoolSystem

## 1. الهدف من هذا الملف

هذا الملف يشرح كل ما يتعلق بمنظومة التقارير في مشروع **SchoolSystem**، بدايةً من تصميم شاشة التقارير، مروراً بالمكتبات المستخدمة، وطريقة جلب البيانات من SQL Server، وانتهاءً بعرض التقرير وتصديره إلى Excel وPDF وCSV وطباعته مباشرة.

تم إعداد هذا الدليل ليكون مرجعاً عملياً أثناء تطوير النظام ومرجعاً أكاديمياً أثناء مناقشة المشروع. يمكن استخدامه للإجابة عن أسئلة مثل: لماذا استخدمت هذه المكتبة؟ كيف ينتقل الطلب من الواجهة إلى قاعدة البيانات؟ كيف أضيف تقريراً جديداً؟ كيف أعدل شكل PDF؟ وكيف أضيف صلاحية جديدة للتصدير أو الطباعة؟

> الفكرة الأساسية: شاشة التقرير لا تتصل بقاعدة البيانات مباشرة، ولا تنشئ ملفات Excel وPDF بنفسها. كل مسؤولية موضوعة في طبقة مستقلة.

## 2. مواقع الملفات المهمة

| الملف | المسؤولية |
|---|---|
| `UI/ReportCenterForm.cs` | منطق شاشة التقارير والفلاتر والأحداث والتصدير والطباعة |
| `UI/ReportCenterForm.Designer.cs` | التصميم المرئي لعناصر الشاشة |
| `Models/ReportRequest.cs` | كائن يحمل اختيارات المستخدم بين الطبقات |
| `Services/ReportService.cs` | التحقق من الطلب والصلاحيات وتمريره للمستودع |
| `DataAccess/ReportRepository.cs` | بناء استعلامات SQL وتنفيذها وإرجاع `DataTable` |
| `Helpers/ReportOutputHelper.cs` | التصدير الموحد إلى Excel وPDF |
| `SchoolSystem.csproj` | مراجع المكتبات وإدراج ملفات التقارير في المشروع |
| `Security/CurrentUser.cs` | التحقق من الصلاحيات قبل تنفيذ العمليات |
| `Security/PermissionKeys.cs` | مفاتيح صلاحيات التقارير |
| `DataAccess/DbConnection.cs` | إنشاء اتصال SQL Server |

## 3. المعمارية العامة

```text
المستخدم
   │ يختار النوع والفلاتر ويضغط تحميل
   ▼
ReportCenterForm
   │ يبني ReportRequest
   ▼
ReportService
   │ يتحقق من الصلاحية وصحة الطلب
   ▼
ReportRepository
   │ يبني SQL بمعاملات Parameters
   ▼
SQL Server
   │ يعيد النتائج
   ▼
DataTable
   ├── DataGridView للعرض
   ├── ReportOutputHelper إلى Excel
   ├── ReportOutputHelper إلى PDF
   ├── StreamWriter إلى CSV
   └── PrintDocument إلى الطابعة
```

هذه المعمارية تطبق مبدأ **Separation of Concerns**، أي فصل المسؤوليات. الواجهة تعرض البيانات، والخدمة تنفذ التحقق، والمستودع يتعامل مع SQL، ومساعد الإخراج يتعامل مع الملفات.

## 4. المكتبات المستخدمة ولماذا استخدمت

| المكتبة أو التقنية | الاستخدام | سبب الاختيار |
|---|---|---|
| `System.Windows.Forms` | بناء الشاشة والأزرار والقوائم و`DataGridView` | جزء أساسي من WinForms في .NET Framework |
| `System.Data` | `DataTable` و`DataRow` و`DataColumn` | شكل مناسب لاستقبال نتائج الاستعلام وعرضها وتصديرها |
| `System.Data.SqlClient` | `SqlConnection` و`SqlCommand` و`SqlDataAdapter` | الاتصال بـ SQL Server وتنفيذ الاستعلامات |
| `ClosedXML` | إنشاء وتنسيق `.xlsx` | يوفر API سهلة لإنشاء Excel دون كتابة XML يدوياً |
| `ClosedXML.Report` | مرجع مساعد في منظومة Excel | موجود ضمن مراجع المشروع، بينما التنفيذ الحالي يستخدم ClosedXML مباشرة |
| `DocumentFormat.OpenXml` | ملفات Excel الحديثة واعتماد تابع لمنظومة ClosedXML | يساند البنية الداخلية لملفات Office Open XML |
| `iTextSharp.LGPLv2.Core` | إنشاء PDF والجداول والخطوط | يسمح بالتحكم في الصفحة والخلايا والاتجاه RTL |
| `System.Drawing` | رسم النص والخلايا أثناء الطباعة | يستخدم مع `PrintDocument` للرسم المباشر |
| `System.Drawing.Printing` | الطباعة والمعاينة وتقسيم الصفحات | مدمج مع Windows Forms |
| `StreamWriter` و`UTF8Encoding` | إنشاء CSV | لا يحتاج إلى مكتبة خارجية ويدعم UTF-8 والعربية |
| `Krypton.Toolkit` | مظهر الواجهة والأزرار | مستخدم لتنسيق shell الواجهة وليس لإنشاء الملفات |

المشروع يستهدف **.NET Framework 4.7.2**، وتظهر هذه المراجع في `SchoolSystem.csproj`، ومن أهمها `ClosedXML` و`ClosedXML.Report` و`DocumentFormat.OpenXml` و`iTextSharp.LGPLv2.Core` و`System.Drawing` و`System.Windows.Forms`.

## 5. تصميم شاشة ReportCenterForm

الشاشة هي `UserControl` حتى يتم تحميلها داخل `MainForm` مثل بقية وحدات النظام. التصميم في `ReportCenterForm.Designer.cs` مقسم إلى أجزاء:

| الجزء | الوظيفة |
|---|---|
| لوحة العنوان | اسم مركز التقارير ووصف مختصر |
| `TableLayoutPanel` للفلاتر | ترتيب عناصر البحث في أعمدة وصفوف ثابتة |
| فلاتر التقرير | النوع والعام الدراسي والصف والشعبة والحالة والتاريخ |
| مربع البحث | البحث الفوري في بيانات التقرير |
| `FlowLayoutPanel` للأزرار | ترتيب أزرار التحميل والتصدير والطباعة باتجاه RTL |
| `DataGridView` | عرض النتيجة في الشاشة |
| لوحة الملخص | عدد السجلات والإجماليات المالية |
| `PrintDocument` | تعريف الطباعة |
| `PrintPreviewDialog` | معاينة التقرير قبل الطباعة |

استخدام `TableLayoutPanel` يقلل التداخل عند تغيير حجم الشاشة، واستخدام `FlowLayoutPanel` يجعل ترتيب الأزرار مرناً. ويتم تطبيق الشكل المركزي عبر `UIHelper` بدلاً من تكرار الألوان والخطوط داخل كل شاشة.

## 6. كيف تبدأ العملية من الواجهة؟

عند فتح الشاشة يتم تحميل أنواع التقارير والصفوف والشعب والحالات والتواريخ الافتراضية. وعند الضغط على زر التحميل ينفذ النظام `LoadReportAsync`.

```csharp
private async void btnLoad_Click(object sender, EventArgs e)
{
    await LoadReportAsync();
}
```

داخل عملية التحميل تحدث المراحل التالية:

```csharp
if (!EnsureReportAction("View", "ليس لديك صلاحية عرض التقارير."))
    return;

if (!ValidateReportFilters())
    return;

ReportRequest request = BuildRequest();
currentReportData = await Task.Run(
    () => reportService.GetReportData(request));

dataGridViewReport.DataSource = currentReportData;
FormatReportGrid();
BuildSummary(currentReportData);
```

استخدام `Task.Run` يمنع تجميد واجهة المستخدم أثناء تنفيذ الاستعلام، خصوصاً عندما يكون التقرير كبيراً.

## 7. كائن ReportRequest

`ReportRequest` هو DTO، أي كائن لنقل البيانات فقط. لا يقرأ قاعدة البيانات ولا يعرض التقرير. وظيفته حمل اختيارات المستخدم:

```csharp
public class ReportRequest
{
    public string ReportType { get; set; }
    public string AcademicYear { get; set; }
    public int? ClassID { get; set; }
    public string Section { get; set; }
    public string Status { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string SearchText { get; set; }
}
```

استخدام هذا الكائن أفضل من إرسال متغيرات كثيرة منفصلة؛ لأنه يجعل عقدة التقرير واضحة ويسهل إضافة فلتر جديد لاحقاً.

## 8. دور ReportService

الخدمة في `Services/ReportService.cs` لا تبني واجهة ولا ترسم ملفاً. وظيفتها الوسيطة هي:

1. التأكد من امتلاك المستخدم صلاحية عرض التقارير.
2. التأكد من أن الطلب ليس فارغاً.
3. التحقق من اسم التقرير والتواريخ.
4. التحقق من العام الدراسي.
5. استدعاء `ReportRepository`.
6. إعادة `DataTable` للواجهة.

```csharp
public DataTable GetReportData(ReportRequest request)
{
    CurrentUser.DemandPermission(
        PermissionKeys.ReportsView,
        "ليس لديك صلاحية عرض التقارير.");

    ValidateRequest(request);
    return repository.GetReportData(request);
}
```

وجود التحقق داخل الخدمة مهم؛ لأن إخفاء الزر من الواجهة وحده ليس حماية كافية. الخدمة تمنع الوصول حتى لو استدعى كود آخر الوظيفة مباشرة.

## 9. دور ReportRepository

المستودع في `DataAccess/ReportRepository.cs` مسؤول عن ربط التقرير بقاعدة البيانات. يستقبل `ReportRequest` ويحدد الدالة المناسبة حسب `ReportType`.

```csharp
public DataTable GetReportData(ReportRequest request)
{
    if (request.ReportType == "تقرير الطلاب")
        return GetStudentsReport(request);

    if (request.ReportType == "تقرير المعلمين")
        return GetTeachersReport(request);

    if (request.ReportType == "تقرير الدرجات")
        return GetMarksReport(request);

    if (request.ReportType == "تقرير الحركة المالية")
        return GetFinancialMovementReport(request);

    return CreateMessageTable("نوع التقرير غير معروف.");
}
```

كل تقرير له استعلام مستقل، ولذلك يمكن تعديل تقرير واحد دون التأثير على بقية التقارير.

## 10. ربط SQL Server

يستخدم المستودع `SqlConnection` و`SqlCommand` و`SqlDataAdapter`:

```csharp
using (SqlConnection conn = DbConnection.GetConnection())
using (SqlCommand cmd = new SqlCommand(query, conn))
{
    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;
    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20)
        .Value = academicYear.Trim();

    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
    {
        DataTable table = new DataTable();
        adapter.Fill(table);
        return table;
    }
}
```

استخدام `Parameters` يمنع دمج إدخال المستخدم داخل نص SQL مباشرة، ويقلل مخاطر SQL Injection ويحسن التعامل مع النصوص العربية والتواريخ.

## 11. التقارير المتوفرة

| الاسم في الواجهة | الوظيفة |
|---|---|
| تقرير الطلاب | بيانات الطلاب والصفوف والشعب والحالة وبيانات ولي الأمر |
| تقرير المعلمين | بيانات المعلمين والحالة وبيانات الاتصال |
| تقرير القبول والتسجيل | الطلبات والتواريخ والرسوم والمدفوع والمتبقي |
| تقرير توزيع الفصول | الطالب والصف والشعبة والعام الدراسي وتاريخ التوزيع |
| تقرير حضور المعلمين | الحضور والانصراف والتأخير وساعات العمل |
| تقرير العقود والرواتب | العقود والرواتب والبدلات والخصومات والصافي |
| تقرير المستخدمين والصلاحيات | المستخدمون والأدوار والصلاحيات |
| تقرير الرسوم | الرسوم والمدفوع والمتبقي |
| تقرير الدرجات | المواد والدرجات والنتائج |
| تقرير الحركة المالية | سندات القبض والصرف والحركة المالية |

## 12. مثال ربط تقرير الطلاب

يستخرج التقرير بيانات الطالب ويربطه بالصف:

```sql
SELECT
    s.StudentID AS [رقم الطالب],
    s.StudentNumber AS [الرقم الأكاديمي],
    s.FullName AS [اسم الطالب],
    c.ClassName AS [الصف],
    s.Section AS [الشعبة],
    s.AcademicYear AS [العام الدراسي],
    s.Status AS [الحالة]
FROM Students s
LEFT JOIN Classes c ON s.ClassID = c.ClassID
WHERE 1 = 1
```

ثم تضاف الفلاتر عند الحاجة:

```sql
AND s.ClassID = @ClassID
AND s.Section = @Section
AND s.Status = @Status
AND (
    s.FullName LIKE @Search
    OR s.StudentNumber LIKE @Search
    OR s.GuardianPhone LIKE @Search
)
```

استخدام `WHERE 1 = 1` يجعل إضافة شروط اختيارية أسهل برمجياً.

## 13. عرض البيانات في DataGridView

بعد رجوع البيانات:

```csharp
dataGridViewReport.DataSource = currentReportData;
```

ثم يطبق النظام:

- القراءة فقط.
- اختيار الصف كاملاً.
- منع إضافة وحذف الصفوف.
- ضبط المحاذاة.
- تنسيق العناوين.
- إخفاء رؤوس الصفوف غير الضرورية.
- تطبيق النمط الموحد من `UIHelper`.

`DataGridView` ليس مصدر البيانات؛ هو طبقة عرض فقط. المصدر هو `DataTable` الناتج من SQL Server.

## 14. تصميم Excel

منطق Excel في `Helpers/ReportOutputHelper.cs`، والدالة الرئيسية هي:

```csharp
ReportOutputHelper.ExportToExcel(
    dt,
    filePath,
    title,
    summary);
```

داخل الدالة:

```csharp
using (XLWorkbook workbook = new XLWorkbook())
{
    IXLWorksheet sheet = workbook.Worksheets.Add("Report");
    workbook.SaveAs(filePath);
}
```

التنسيق المستخدم يشمل عنواناً مدمجاً، ملخصاً، تاريخ الإنشاء، رؤوس أعمدة بلون داكن، صفوفاً متناوبة، تجميد صف العناوين، وضبط عرض الأعمدة.

تم دعم RTL في Excel عن طريق:

```csharp
sheet.RightToLeft = ContainsArabic(title)
    || ContainsArabic(summary)
    || ContainsArabicColumns(table)
    || ContainsArabicValues(table);
```

وتتم محاذاة القيم حسب نوعها:

```text
الأرقام: وسط
العربية: يمين
الإنجليزية: يسار
```

## 15. تصميم PDF

منطق PDF موجود في نفس `ReportOutputHelper`، باستخدام `iTextSharp.LGPLv2.Core`.

يختار النظام حجم الصفحة حسب عدد الأعمدة:

```csharp
Rectangle pageSize = table.Columns.Count > 10
    ? PageSize.A3.Rotate()
    : PageSize.A4.Rotate();
```

ثم ينشئ المستند والجدول والخلايا:

```csharp
using (Document document = new Document(pageSize, 28, 28, 42, 34))
{
    PdfWriter.GetInstance(document, stream);
    document.Open();
    document.Add(grid);
    document.Close();
}
```

لدعم العربية يستخدم النظام خطاً من Windows مثل `Tahoma` أو `Arial` أو `Segoe UI`، ثم ينشئ `BaseFont` باستخدام `IDENTITY_H`. كما يحدد اتجاه النص عبر `RUN_DIRECTION_RTL` إذا وجد نصاً عربياً.

## 16. تصميم CSV

يتم إنشاء CSV دون مكتبة خارجية:

```csharp
using (StreamWriter sw = new StreamWriter(
    filePath,
    false,
    new UTF8Encoding(true)))
```

ويتم وضع كل قيمة داخل علامات اقتباس. إذا احتوت القيمة على علامة اقتباس، يتم مضاعفتها:

```csharp
value = value.Replace("\"", "\"\"");
return "\"" + value + "\"";
```

استخدام UTF-8 مع BOM يساعد Excel على قراءة العربية بشكل صحيح.

## 17. الطباعة المباشرة والمعاينة

الطباعة لا تعتمد على Excel أو PDF، بل تستخدم `PrintDocument` و`PrintPreviewDialog`.

```csharp
printRowIndex = 0;
printPreviewDialog.ShowDialog();
```

وعند إنشاء الصفحة ينفذ Windows الحدث:

```csharp
private void PrintDocument_PrintPage(
    object sender,
    PrintPageEventArgs e)
```

يتم رسم العنوان ورؤوس الأعمدة والبيانات بواسطة `Graphics.DrawString` و`Graphics.FillRectangle`. وعند وصول الصفحة إلى نهايتها:

```csharp
e.HasMorePages = true;
```

ويستمر النظام من قيمة `printRowIndex` في الصفحة التالية.

## 18. الصلاحيات

الصلاحيات موزعة حسب العملية:

| العملية | الحماية |
|---|---|
| عرض التقرير | `View` أو `ReportsView` |
| تصدير Excel | `ExportExcel` |
| تصدير PDF | `ExportPDF` |
| تصدير CSV | `ExportCsv` |
| الطباعة | `Print` |

مثال:

```csharp
if (!EnsureReportAction(
        "ExportExcel",
        "ليس لديك صلاحية تصدير التقارير إلى Excel."))
    return;
```

بهذا يمكن إعطاء مستخدم صلاحية عرض فقط، ومنع التصدير والطباعة، أو إعطائه جميع الصلاحيات حسب دوره.

## 19. البحث الفوري

مربع البحث مربوط بحدث `TextChanged`:

```csharp
txtSearch.TextChanged += txtSearch_TextChanged;
```

وعند كتابة أي حرف أو رقم يعاد تحميل التقرير. ينتقل النص داخل `ReportRequest`، ثم يستخدم في SQL مع `LIKE`:

```sql
AND (
    FullName LIKE @Search
    OR Phone LIKE @Search
    OR Email LIKE @Search
)
```

هذا يسمح بالبحث من أول حرف ودعم أكثر من كلمة أو رقم بحسب الحقول التي يحددها كل تقرير.

## 20. كيف تضيف تقريراً جديداً؟

لإضافة تقرير جديد اتبع الخطوات التالية:

### الخطوة الأولى: إضافة الاسم إلى الواجهة

في `LoadStaticData` داخل `ReportCenterForm.cs`:

```csharp
cmbReportType.Items.Add("تقرير الحافلات");
```

### الخطوة الثانية: إضافة دالة في المستودع

داخل `ReportRepository.cs`:

```csharp
private DataTable GetBusReport(ReportRequest request)
{
    string query = @"
        SELECT
            BusID AS [رقم الحافلة],
            BusNumber AS [رقم الحافلة النصي],
            DriverName AS [السائق],
            Status AS [الحالة]
        FROM Buses
        WHERE 1 = 1";

    if (!string.IsNullOrWhiteSpace(request.Status)
        && request.Status != "الكل")
        query += " AND Status = @Status";

    if (!string.IsNullOrWhiteSpace(request.SearchText))
        query += " AND (BusNumber LIKE @Search OR DriverName LIKE @Search)";

    return ExecuteQuery(query, request);
}
```

### الخطوة الثالثة: ربط الاسم بالدالة

داخل `GetReportData`:

```csharp
if (request.ReportType == "تقرير الحافلات")
    return GetBusReport(request);
```

### الخطوة الرابعة: إضافة الصلاحية إذا كانت مطلوبة

إذا كان التقرير يحتاج صلاحية مستقلة، أضف مفتاحاً في `PermissionKeys.cs`، ثم اربطه في `CurrentUser` أو شاشة الصلاحيات.

### الخطوة الخامسة: اختبار التقرير

اختبر اختيار التقرير، التحميل، البحث، الفلاتر، البيانات الفارغة، Excel، PDF، CSV، والطباعة.

## 21. كيف تعدل شكل Excel؟

التعديلات تكون في `ReportOutputHelper.ExportToExcel`.

لتغيير لون العنوان:

```csharp
private static readonly BaseColor Navy =
    new BaseColor(31, 41, 55);
```

لتغيير اسم الورقة:

```csharp
IXLWorksheet sheet = workbook.Worksheets.Add("تقرير الطلاب");
```

لتغيير خط الخلايا:

```csharp
cell.Style.Font.FontName = "Tahoma";
cell.Style.Font.FontSize = 11;
```

لتغيير لون الصفوف المتناوبة:

```csharp
cell.Style.Fill.BackgroundColor =
    XLColor.FromArgb(248, 250, 252);
```

## 22. كيف تعدل شكل PDF؟

التعديلات تكون في `ExportToPdf` أو `AddTextCell`.

لتغيير حجم الصفحة:

```csharp
Rectangle pageSize = PageSize.A4.Rotate();
```

لتغيير حجم خط العنوان:

```csharp
ITextFont titleFont =
    new ITextFont(baseFont, 18f, ITextFont.BOLD);
```

لتغيير لون رأس الجدول:

```csharp
private static readonly BaseColor Navy =
    new BaseColor(31, 41, 55);
```

لتغيير المسافة داخل الخلية:

```csharp
Padding = 5
```

## 23. كيف تعدل الطباعة؟

التعديلات تكون داخل `PrintDocument_PrintPage` في `ReportCenterForm.cs`.

لتغيير ارتفاع الصف:

```csharp
int rowHeight = 24;
```

لتغيير حجم خط البيانات:

```csharp
new System.Drawing.Font("Tahoma", 7)
```

لتغيير عنوان النظام:

```csharp
e.Graphics.DrawString(
    "نظام إدارة المدرسة",
    titleFont,
    Brushes.Black,
    rectangle,
    rtlFormat);
```

## 24. كيف تضيف عموداً جديداً؟

إضافة العمود تتم غالباً في استعلام التقرير:

```sql
s.NationalId AS [الرقم الوطني]
```

بعد ذلك سيظهر العمود تلقائياً في `DataGridView` وExcel وPDF وCSV والطباعة؛ لأن جميع صيغ الإخراج تعتمد على أعمدة `DataTable` بشكل عام، ولا تحتاج إلى تعريف العمود يدوياً في كل مخرج.

هذه من أهم مزايا التصميم الحالي: **إضافة عمود إلى نتيجة التقرير تنعكس على كل صيغ العرض والتصدير والطباعة**.

## 25. كيف تضيف فلتر جديداً؟

لإضافة فلتر مثل الجنس:

1. أضف `ComboBox` في `ReportCenterForm.Designer.cs`.
2. حمّل القيم في `LoadStaticData`.
3. أضف الخاصية `Gender` إلى `ReportRequest`.
4. اجمع القيمة داخل `BuildRequest`.
5. أضف الشرط في الاستعلام:

```sql
AND s.Gender = @Gender
```

6. أضف المعامل في `ExecuteQuery` أو دالة بناء المعاملات.
7. اختبر الكل والقيمة المفردة والقيمة الفارغة.

## 26. التعامل مع البيانات الفارغة والأخطاء

قبل التصدير والطباعة يتم استدعاء `EnsureData`:

```csharp
if (dt == null || dt.Rows.Count == 0)
{
    ShowWarning("لا توجد بيانات للتصدير أو الطباعة.");
    return false;
}
```

كما يتحقق النظام من وجود الجداول ويعيد رسالة مفهومة إذا كان جدول التقرير غير موجود، بدلاً من ترك المستخدم أمام خطأ SQL غامض.

## 27. إجابة جاهزة للمناقشة

> صممت التقارير باستخدام معمارية طبقية. شاشة `ReportCenterForm` مسؤولة عن الفلاتر والعرض فقط. عند اختيار نوع التقرير، تجمع الشاشة البيانات داخل `ReportRequest`. بعدها تنتقل البيانات إلى `ReportService` الذي يطبق التحقق والصلاحيات، ثم إلى `ReportRepository` الذي يبني استعلام SQL Server باستخدام Parameters ويعيد النتيجة داخل `DataTable`. نفس الـ DataTable يعرض في DataGridView، ثم يرسل إلى `ReportOutputHelper` للتصدير إلى Excel باستخدام ClosedXML أو PDF باستخدام iTextSharp، بينما CSV يستخدم StreamWriter بترميز UTF-8، والطباعة تعتمد على PrintDocument وPrintPreviewDialog. فصلت التصدير عن الشاشة حتى لا أكرر الكود، وأضفت دعم RTL للواجهة وExcel وPDF، وصلاحيات منفصلة للعرض والطباعة والتصدير.

## 28. أسئلة متوقعة وإجاباتها

| السؤال | الإجابة |
|---|---|
| لماذا استخدمت `DataTable`؟ | لأنه يتعامل بسهولة مع نتائج SQL ذات الأعمدة المتغيرة، ويرتبط مباشرة بـ DataGridView ومساعدات التصدير. |
| لماذا وضعت SQL داخل Repository؟ | حتى لا تختلط واجهة المستخدم مع الوصول إلى البيانات، ولتسهيل الصيانة والاختبار. |
| لماذا أنشأت `ReportService`؟ | لفصل التحقق والصلاحيات عن SQL وعن واجهة المستخدم. |
| لماذا استخدمت `ReportRequest`؟ | لتجميع الفلاتر في كائن واحد واضح بدلاً من تمرير متغيرات كثيرة. |
| لماذا ClosedXML؟ | لتنسيق Excel بسهولة وإنشاء عناوين وجداول وألوان واتجاه RTL. |
| لماذا iTextSharp؟ | لإنشاء PDF والتحكم في الصفحة والجداول والخطوط واتجاه العربية. |
| كيف دعمت العربية؟ | بخطوط Windows وUnicode و`IDENTITY_H` و`RUN_DIRECTION_RTL` و`RightToLeft`. |
| كيف منعت SQL Injection؟ | باستخدام `SqlParameter` وعدم دمج قيم المستخدم مباشرة في SQL. |
| كيف تتم الطباعة؟ | عبر `PrintDocument` وحدث `PrintPage` مع `HasMorePages` للتقسيم. |
| هل يستطيع المستخدم التصدير إذا كان يستطيع العرض فقط؟ | لا، لأن العرض والتصدير والطباعة لها صلاحيات مستقلة. |
| كيف تضيف تقريراً؟ | أضيف اسمه في الشاشة، ودالة SQL في Repository، وربطاً في GetReportData، ثم أختبر كل المخرجات. |
| ماذا يحدث عند إضافة عمود؟ | يظهر تلقائياً في DataGridView وExcel وPDF وCSV والطباعة لأن كل المخرجات تعتمد على DataTable. |
| كيف تدعم البحث؟ | من حدث TextChanged، ثم يمر SearchText إلى SQL باستخدام LIKE وParameter. |
| لماذا لا يتجمد البرنامج أثناء التحميل؟ | لأن استدعاء الخدمة يتم داخل `Task.Run` مع `async/await`. |

## 29. قائمة اختبار أي تقرير جديد

| الاختبار | المطلوب |
|---|---|
| فتح الشاشة | لا يوجد استثناء أو تداخل |
| اختيار النوع | يظهر التقرير الصحيح |
| تحميل بدون فلاتر | تظهر كل السجلات المسموح بها |
| بحث بحرف | تظهر النتائج فوراً |
| بحث بكلمة أو رقم | يتم البحث في الحقول المحددة |
| فلتر الصف | تظهر بيانات الصف فقط |
| فلتر الشعبة | تظهر بيانات الشعبة فقط |
| فلتر التاريخ | لا تظهر سجلات خارج الفترة |
| بيانات فارغة | تظهر رسالة مفهومة |
| Excel | يفتح الملف وتظهر العربية والتنسيق |
| PDF | يظهر الجدول والعربية والاتجاه الصحيح |
| CSV | يفتح في Excel دون تشوه العربية |
| الطباعة | تظهر المعاينة ولا تختفي الصفوف بين الصفحات |
| الصلاحيات | يمنع العرض أو التصدير أو الطباعة عند عدم السماح |
| RTL | لا يحدث انعكاس أو قص للنص العربي |

## 30. خلاصة مختصرة جداً

منظومة التقارير مبنية على المسار التالي:

```text
واجهة التقارير
→ ReportRequest
→ ReportService
→ ReportRepository
→ SQL Server
→ DataTable
→ DataGridView / Excel / PDF / CSV / Print
```

المكتبات الرئيسية هي:

```text
ClosedXML       = Excel
 iTextSharp      = PDF
 System.Windows.Forms = الواجهة وDataGridView
 System.Drawing.Printing = الطباعة
 System.Data.SqlClient = SQL Server
 StreamWriter    = CSV
 Krypton Toolkit = تنسيق الواجهة
```

وعند طلب تعديل من الدكتور، حدد أولاً هل التعديل متعلق بالبيانات أم بالفلاتر أم بالشكل أم بالصلاحيات أم بمخرج معين، ثم عدل الطبقة المسؤولة فقط، وبعدها اختبر العرض والتصدير والطباعة والصلاحيات.

## 31. أوامر فتح الملف ومزامنته

بعد وضع الملف في المستودع، يمكن فتحه من جهاز Windows بعد جلب آخر نسخة:

```powershell
git switch main
git pull --ff-only origin main
```

ثم افتح:

```text
docs/Reports_Complete_Guide_AR.md
```

ومن الأفضل قبل المناقشة مراجعة الملفات التالية مع هذا الدليل:

```text
UI/ReportCenterForm.cs
Services/ReportService.cs
DataAccess/ReportRepository.cs
Helpers/ReportOutputHelper.cs
Models/ReportRequest.cs
```

## 32. مراجع الكود داخل المشروع

هذا الدليل مبني على التنفيذ الفعلي في الملفات التالية:

- `UI/ReportCenterForm.cs`
- `UI/ReportCenterForm.Designer.cs`
- `Services/ReportService.cs`
- `DataAccess/ReportRepository.cs`
- `Helpers/ReportOutputHelper.cs`
- `Models/ReportRequest.cs`
- `SchoolSystem.csproj`

> ملاحظة: بناء Visual Studio الفعلي واختبار الطباعة والخطوط وSQL Server يجب تنفيذه على جهاز Windows المستهدف، لأن بيئة Linux لا توفر مترجم .NET Framework 4.7.2 أو مصمم WinForms.
