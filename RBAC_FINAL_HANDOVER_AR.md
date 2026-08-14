# تقرير تسليم إصلاح Authentication وAuthorization وRBAC

## الحالة

تم تنفيذ إصلاح شامل ومتدرج لنظام المصادقة والتفويض والتحكم بالوصول في تطبيق `SchoolSystem`، وتم رفع جميع الدفعات إلى فرع `main` في GitHub. آخر حالة مؤكدة هي أن `HEAD` المحلي و`origin/main` عند commit `16112d8`، والمستودع نظيف بعد البناء.

## السبب الجذري

كان التصميم يعتمد على تخزين الدور والصلاحيات كسلاسل نصية داخل سجل `Users` بدل جداول RBAC مستقلة. وكانت صلاحيات الدور تُملأ افتراضيًا فقط عندما تكون خانة `Permissions` فارغة؛ لذلك فإن حساب المدير الذي يحمل قائمة ناقصة لا يحصل تلقائيًا على بقية الصلاحيات. كما وُجد اعتماد أمني على اسم الدور النصي `مدير النظام`، ومطابقة جزئية للصلاحيات في `UsersForm` باستخدام `StartsWith`، وحماية واجهة غير كافية لبعض عمليات الخدمات.

## الإصلاحات المنفذة

| المجال | الإصلاح |
|---|---|
| القاموس المركزي | توحيد الأدوار والمفاتيح عبر `PermissionKeys`، وتطبيع الأسماء والقيم وإزالة التكرار. |
| مدير النظام | جعل دور `مدير النظام` يستخدم القائمة المركزية الكاملة بدل قائمة يدوية ناقصة أو bypass مبني على اسم المستخدم. |
| الجلسة | تطبيع المستخدم عند إنشاء الجلسة، مسح الجلسة عند الخروج والإغلاق المباشر، ومنع بقاء جلسة المدير عند الانتقال لمستخدم آخر. |
| الخدمات | فرض `DemandPermission` و`DemandAny` داخل الخدمات الأكاديمية، المعلمين، الحضور، الدرجات، الفصول، المواد، التقارير، لوحة التحكم، الرسوم، المكتبة، النقل، العقود وإدارة المستخدمين. |
| إدارة المستخدمين | منع حذف المستخدم الحالي، ومنع تعطيل أو حذف آخر مدير نظام، وفرض `Users.Manage` على عمليات الإدارة بعد التهيئة الأولى. |
| UsersForm | تطبيع الدور والصلاحيات عند الحفظ والتحميل، واستبدال المطابقة الجزئية بمطابقة مفتاح دقيقة. |
| قاعدة البيانات | إضافة `Databass/Migration_RBAC_Hardening.sql` idempotent لتوحيد Admin/Administrator وإعادة كامل صلاحيات المدير وإكمال الصلاحيات الفارغة للأدوار المعروفة. |
| الاختبارات | إضافة `RBAC_ACCEPTANCE_TEST_AR.md` بتسلسل اختبار المدير والمستخدم المحدود وتسجيل الخروج وإعادة التشغيل وعزل الجلسة. |

## سلسلة الدفعات المرفوعة

| Commit | الوصف |
|---|---|
| `fff940e` | Harden centralized RBAC session handling |
| `2b8f651` | Enforce RBAC on academic services |
| `bbc6d6c` | Enforce RBAC on teacher services |
| `af0f311` | Enforce RBAC on class and subject services |
| `7ed4898` | Enforce RBAC on reports dashboard and fee plans |
| `36bbdac` | Enforce RBAC on library and transport services |
| `5ca40db` | Harden payroll contracts and shared student lookups |
| `85fdd9b` | Normalize RBAC permissions and add hardening migration |
| `2b4ef77` | Protect user administration service operations |
| `16112d8` | Add RBAC session isolation and acceptance tests |

## تحديث قاعدة البيانات على Windows وSQL Server

يجب أخذ نسخة احتياطية أولًا. بعد ذلك افتح SQL Server Management Studio واتصل بالخادم الصحيح، وافتح ملف `Databass/Migration_RBAC_Hardening.sql`، وتأكد من أن قاعدة البيانات المستهدفة هي `SchoolDB`، ثم نفّذ الملف. يمكن تشغيله أكثر من مرة؛ فهو لا ينشئ سجلات مستخدمين ولا يستبدل تخصيصات الأدوار غير الفارغة، لكنه يعيد مدير النظام إلى القائمة المركزية الكاملة عمدًا.

بعد التنفيذ شغّل الاستعلام الآتي للتحقق:

```sql
SELECT UserID, UserName, RoleName, Permissions, IsActive
FROM dbo.Users
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) = LOWER(N'مدير النظام');
```

يجب أن يظهر لكل مدير نظام الدور الموحد `مدير النظام`، وأن تحتوي `Permissions` على `Users.Manage` وجميع مفاتيح `PermissionKeys.All`.

## جلب وتشغيل النسخة على جهاز Windows

```powershell
git clone https://github.com/muhannad2080/SchoolSystem.git
cd SchoolSystem
git switch main
git pull --ff-only origin main
```

إذا كان المستودع موجودًا مسبقًا:

```powershell
cd D:\_Getintopc.com_VS2022_2_2\SchoolSystem
git switch main
git pull --ff-only origin main
```

بعد تحديث قاعدة البيانات، افتح ملف الحل في Visual Studio 2022، نفّذ Restore للـNuGet packages، ثم اختر `Release` و`Any CPU` وابنِ المشروع. يجب نسخ إعداد الاتصال الصحيح إلى `SchoolSystem.exe.config` وعدم استخدام إعداد بيئة التطوير على جهاز الإنتاج.

## نتيجة البناء

تم تنفيذ `git diff --check` ونجح بناء Debug وRelease عبر `xbuild` دون أخطاء تجميع. ظهرت تحذيرات بيئة Mono المتوقعة لأن المشروع يستهدف `.NET Framework 4.7.2` ولوجود مرجع `System.Resources.Extensions` غير محلول في بيئة Linux؛ يجب اعتماد بناء Visual Studio على Windows كاختبار الإصدار النهائي.

## الاختبار التشغيلي المطلوب

نفّذ الحالات الواردة بالتفصيل في `RBAC_ACCEPTANCE_TEST_AR.md`. أهم تسلسل قبول هو: الدخول كمدير، فتح العمليات المحمية، تسجيل الخروج، الدخول بمستخدم محدود، محاولة عملية إدارية، تسجيل الخروج، ثم إعادة التشغيل. يجب ألا تظهر أي صلاحية من جلسة المدير للمستخدم المحدود، ويجب أن تُرفض العملية المحمية من طبقة الخدمة حتى لو تم استدعاؤها خارج القائمة.

## ملاحظة تشغيلية

لا يمكن للبيئة الحالية تنفيذ اتصال SQL Server الفعلي أو اختبار واجهة WinForms تفاعليًا؛ لذلك يبقى تنفيذ migration والاختبار اليدوي على Windows وSQL Server خطوة اعتماد تشغيلية نهائية، وليس نقصًا في كود الإصلاح المرفوع.
