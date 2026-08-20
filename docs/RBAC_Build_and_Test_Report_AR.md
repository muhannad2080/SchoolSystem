# تقرير بناء واختبار نظام الصلاحيات RBAC

## تاريخ المراجعة

تمت مراجعة آخر نسخة من الفرع `main` بتاريخ 20 أغسطس 2026.

## النسخة التي تمت مراجعتها

```text
b7b898d Show modules with view permissions
```

حالة Git أثناء المراجعة:

```text
main...origin/main
```

## نطاق المراجعة

تمت مراجعة دورة الصلاحيات كاملة من قائمة الصلاحيات في `UsersForm`، مروراً بحفظ قيمة `Users.Permissions`، ثم تحميلها في `UserService` عند تسجيل الدخول، وتطبيعها في `CurrentUser`، وأخيراً تطبيقها على إظهار وفتح القوائم في `MainForm`.

أصبح ظهور الشاشة يعتمد على صلاحية `Module.View` أو أي صلاحية صحيحة تحت الوحدة نفسها، مع إبقاء مفاتيح `Module.Manage` القديمة للتوافق مع الحسابات السابقة.

## نتائج العقود والفحوص

| الفحص | النتيجة |
|---|---|
| Permission/RBAC coverage | PASS |
| Search contract | PASS |
| Search autocomplete contract | PASS — 17 واجهة |
| ComboBox safety contract | PASS — 22 واجهة |
| RTL contract | PASS — 26 شاشة |
| Validation contract | PASS — 23 واجهة و482 عنصر إدخال |
| UI save-handler validation | PASS |
| DataView search safety | PASS |
| Settings contract | PASS — 59 فحصاً |
| Operational readiness | PASS — 11 فحصاً |
| `git diff --check` | PASS |

## نتيجة البناء

تم البحث عن أدوات البناء التالية في بيئة Linux:

```text
dotnet
msbuild
xbuild
csc
```

هذه الأدوات غير متوفرة في البيئة الحالية، ولذلك لم يمكن تنفيذ بناء C#/.NET Framework 4.7.2 فعلياً من Linux. المشروع يعتمد على Windows Forms وKrypton Toolkit و.NET Framework، لذلك يجب تنفيذ `Clean` و`Rebuild` داخل Visual Studio على Windows.

## خطوات البناء على Windows

```powershell
git switch main
git pull --ff-only origin main
```

ثم افتح `SchoolSystem.sln` في Visual Studio ونفّذ:

```text
Build > Clean Solution
Build > Rebuild Solution
```

بعد نجاح البناء شغّل اختبار RBAC التالي:

1. أنشئ مستخدماً أو افتح مستخدماً موجوداً.
2. امنحه `Students.View` فقط وتأكد من ظهور شاشة الطلاب.
3. امنحه `Teachers.View` فقط وتأكد من ظهور شاشة المعلمين.
4. امنحه `Grades.View` فقط وتأكد من ظهور شاشة الدرجات.
5. اضغط منح كل الصلاحيات ثم احفظ.
6. أغلق الجلسة وسجّل الدخول مرة أخرى.
7. تأكد من بقاء الصلاحيات وعدم عودة الحساب إلى `Dashboard.View,Reports.View` فقط.
8. جرّب فتح كل شاشة ظاهرة فعلياً، وليس الاكتفاء بظهور القائمة.

## تشخيص قاعدة البيانات

عند استمرار ظهور القائمتين فقط، شغّل:

```text
Databass/Verify_User_RBAC_Permissions.sql
```

إذا كانت قيمة `Users.Permissions` تساوي فقط:

```text
Dashboard.View,Reports.View
```

فشغّل سكربت الإصلاح:

```text
Databass/Migration_RepairLegacyReportOnlyPermissions_Runtime.sql
```

ابدأ بوضع الفحص، ثم استخدم `@ApplyRepair = 1` بعد مراجعة النتائج وأخذ نسخة احتياطية من قاعدة البيانات.

## الخلاصة

الفحوص الثابتة وعقود التغطية ناجحة، وآخر نسخة في GitHub متزامنة مع الفرع المحلي. التحقق المتبقي الوحيد هو بناء وتشغيل التطبيق على Windows لأن مترجم .NET Framework وبيئة Visual Studio غير متوفرين في Linux.
