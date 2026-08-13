# سجل التدقيق الأولي لإعادة تصميم SchoolSystem

## نقطة البداية

- المستودع: `muhannad2080/SchoolSystem`
- الفرع الأساسي عند الفحص: `main`
- آخر commit قبل التدخل: `7272810542727c4ab62a9f0dbf3d0663e80c33b6` — إضافة تقرير الجاهزية النهائية للإصدار
- فرع العمل المنشأ: `improve-ui-usability-stability`
- إطار العمل: .NET Framework 4.7.2 / Windows Forms / SQL Server
- عدد النوافذ المسماة `*Form.cs`: 22، وتشمل `MainForm` وواجهات UI المتخصصة.
- لم يظهر أي مرجع حالي إلى `Krypton` أو `Krypton.Toolkit` في ملفات المصدر أو المشروع عند الفحص الأول.

## الموجود مسبقًا

يوجد في `Helpers/UIHelper.cs` نظام ألوان موحد، خطوط Tahoma، دعم RTL، تنسيق أساسي للحاويات والحقول والجداول والأزرار، ودوال تحقق للأرقام والعشري ومنع الأرقام، إضافة إلى رسائل عربية وتسجيل الاستثناءات. يجب توسيعه بطريقة متوافقة بدل استبداله أو كسر المراجع الحالية.

يوجد بالفعل توحيد أولي لـ `UIHelper.ApplyTheme` وتنسيق `DataGridView`، مع ألوان مثل `PrimaryColor`, `SuccessColor`, `DangerColor`, `SearchColor`, `PrintColor`, `BackgroundColor` و`CardColor`.

## قيود التنفيذ

- الحفاظ على Repository/Service/DataAccess architecture.
- عدم وضع SQL في Forms.
- عدم تغيير قاعدة البيانات أو حذف البيانات والعلاقات.
- الحفاظ على `LoginForm`, `PasswordHasher`, `CurrentUser`, و`PermissionKeys`.
- دمج Krypton Toolkit فقط مع WinForms القياسي وUIHelper.
- تنفيذ Build بعد كل مرحلة ثم commit/push للفرع المطلوب.
- عدم دمج الفرع في `main`.

## أولويات التنفيذ

1. توثيق كامل للواجهات والخدمات والأحداث قبل التعديل.
2. دمج Krypton Toolkit والتحقق من توافقه مع أسلوب البناء الحالي.
3. تطوير UIHelper ليشمل Krypton controls مع RTL والتحقق والتصدير الآمن.
4. إعادة تصميم Login/Main/Dashboard، ثم الوحدات الوظيفية على مجموعات.
5. اختبار البناء، الأزرار، التحقق، الصلاحيات، والاستجابة للأحجام المختلفة.

## ملاحظات أولية

- `MainForm` يستخدم مزيجًا من `LoadFormInPanel` و`LoadUserControl`، لذا يجب تجنب تغيير أنواع الواجهات أو أسمائها دون فحص المراجع.
- `LoginForm` لديه منطق دخول وحالة تحميل وإخفاء كلمة مرور يجب الحفاظ عليه، مع تحسين المظهر فقط بعد فهم الـ Designer.
- لم يُثبت بعد وجود Krypton Toolkit في `packages.config` أو المشروع؛ يلزم فحص إعدادات NuGet ثم إضافة المرجع بطريقة تناسب xbuild و.NET Framework 4.7.2.
