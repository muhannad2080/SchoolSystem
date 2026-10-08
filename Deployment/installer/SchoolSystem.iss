; SchoolSystem.iss  -  ملف إعداد Inno Setup لنظام إدارة المدارس
; يستدعيه Deployment\Build-Installers.ps1 لبناء SchoolSystem-Setup-<version>.exe
; يشترط: Inno Setup 6 مثبّت (winget install JRSoftware.InnoSetup)

#define MyAppName "نظام إدارة المدارس - SchoolSystem"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "SchoolSystem"
#define MyAppExeName "SchoolSystem.exe"

[Setup]
AppId={{9F5C7E62-1A2B-4C3D-8E4F-5A6B7C8D9E0F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\SchoolSystem
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\..\dist
OutputBaseFilename=SchoolSystem-Setup-{#MyAppVersion}
SetupIconFile=..\..\Assets\Brand\SchoolSystem.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/ultra
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
ArchitecturesAllowed=x64compatible

[Languages]
Name: "arabic"; MessagesFile: "compiler:Languages\Arabic.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "إنشاء اختصار على سطح المكتب"; GroupDescription: "اختصارات إضافية:"

[Files]
Source: "..\..\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "تشغيل النظام بعد التثبيت"; Flags: nowait postinstall skipifsilent unchecked