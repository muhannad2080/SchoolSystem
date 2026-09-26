# SchoolSystem

نظام إدارة مدارس مكتبي مبني باستخدام C# WinForms و.NET Framework 4.7.2 وKrypton Toolkit، مع واجهة عربية RTL وبنية Repository/Service.

## فتح المشروع في Visual Studio

يتطلب المشروع Visual Studio 2022 مع workload **.NET desktop development** وTargeting Pack الخاص بـ **.NET Framework 4.7.2**. يمكن فتح ملف `.vsconfig` من Visual Studio Installer لتثبيت المكونات المطلوبة تلقائيًا.

بعد استنساخ المستودع، افتح PowerShell داخل مجلد المشروع وشغّل:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Restore-Packages.ps1
```

بعد نجاح الاستعادة افتح:

```text
SchoolSystem.sln
```

إذا ظهر المشروع بحالة **unloaded**، أغلق Visual Studio، شغّل سكربت استعادة الحزم، ثم أعد فتح الحل. وتأكد أيضًا من تثبيت .NET Framework 4.7.2 Targeting Pack من Visual Studio Installer.

## البناء والتشغيل

من Visual Studio اختر `Build > Rebuild Solution`، ثم شغّل المشروع باستخدام `F5`. يجب إعداد اتصال SQL Server في `App.config` قبل التشغيل الفعلي.

## مزامنة التحديثات

```powershell
git switch main
git pull --ff-only origin main
```
