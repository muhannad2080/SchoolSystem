# SchoolSystem

نظام إدارة مدارس مكتبي مبني باستخدام C# WinForms و.NET Framework 4.7.2 وKrypton Toolkit، مع واجهة عربية RTL وبنية Repository/Service.

## فتح المشروع في Visual Studio

يتطلب المشروع Visual Studio 2022 مع workload **.NET desktop development** وTargeting Pack الخاص بـ **.NET Framework 4.7.2**. يمكن فتح ملف `.vsconfig` من Visual Studio Installer لتثبيت المكونات المطلوبة تلقائيًا.

بعد استنساخ المستودع، استخدم فرع التطوير الحالي ثم افتح PowerShell داخل مجلد المشروع وشغّل:

```powershell
git switch feature/new-development
git pull --ff-only origin feature/new-development
powershell -ExecutionPolicy Bypass -File .\scripts\Restore-Packages.ps1
```

بعد نجاح الاستعادة افتح:

```text
SchoolSystem.sln
```

إذا ظهر المشروع بحالة **unloaded**، أغلق Visual Studio، شغّل سكربت استعادة الحزم، ثم أعد فتح الحل. وتأكد أيضًا من تثبيت .NET Framework 4.7.2 Targeting Pack من Visual Studio Installer.

## البناء والتشغيل

من Visual Studio اختر `Build > Rebuild Solution`، ثم شغّل المشروع باستخدام `F5`.

لإنشاء ملف التثبيت النهائي:

```powershell
powershell -ExecutionPolicy Bypass -File .\Deployment\Build-Installers.ps1 -Configuration Release -Version 1.0.0
```

ينتج المثبت الأساسي:

`dist\SchoolSystem-Setup-1.0.0.exe`

### ماذا يفعل المثبت

بعد تثبيت `Setup.exe` وتشغيل النظام لأول مرة:

1. يكتشف مثيل SQL Server المحلي الشائع تلقائيًا عند استخدام Windows Authentication.
2. إذا كانت `SchoolDB` موجودة، يستخدمها ويطبق فقط الـmigrations المعلقة دون حذف بياناتها.
3. إذا لم تكن موجودة، ينشئها تلقائيًا من `Database\databaseschool.sql` ثم يطبق الـmigrations.
4. يجهز بيانات العام الأكاديمي `2026/2027` للشحنة الجديدة فقط.
5. يجهز حساب `admin` بكلمة مرور مؤقتة عشوائية ويطلب تغييرها عند أول دخول.
6. يثبت المكتبات الأصلية المطلوبة للتقارير والتصدير.

المتطلبات: Windows، و`.NET Framework 4.7.2` أو أحدث، وSQL Server/SQL Express/LocalDB متاح للمستخدم. إذا لم يوجد SQL Server محلي أو لم يكن الاتصال قابلاً للاستخدام، تظهر شاشة إعداد الاتصال داخل البرنامج.

## مزامنة التحديثات للنسخة الحالية

```powershell
git switch feature/new-development
git pull --ff-only origin feature/new-development
```

لبناء نسخة العرض النظيفة:

```powershell
nuget restore SchoolSystem.sln
msbuild SchoolSystem.sln /m /p:Configuration=Release /p:Platform="Any CPU"
```