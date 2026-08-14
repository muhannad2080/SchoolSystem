# جرد واجهات SchoolSystem

تم جرد ملفات الواجهات الفعلية تحت `UI` وملفات `MainForm`. المشروع يحتوي على Forms وUserControls متعددة، ومعظم الشاشات تستخدم Designer-generated absolute `Location` و`Size` مع بعض `Dock`, `Anchor`, `TableLayoutPanel`, و`FlowLayoutPanel`.

## ملاحظات أولية

- شاشة `MainForm` تطبق تنسيقًا عامًا، لكن الشاشات الداخلية تستخدم مستويات متفاوتة من التخصيص.
- `ClassesForm` مثال على شاشة تستخدم `TableLayoutPanel` و`FlowLayoutPanel` و`DataGridView` بشكل أفضل من الشاشات القديمة، لكنها ما زالت تعتمد على أحجام ثابتة واسعة.
- يلزم توحيد `RightToLeft`, `RightToLeftLayout`, `AutoScaleMode`, `MinimumSize`, و`Dock/Anchor` على مستوى النوافذ والحاويات.
- يلزم توحيد ارتفاع الأزرار والحقول، وتنسيق الجداول، وهوامش الأقسام، ورسائل الحالة.
- يجب تعديل ملفات code-behind وhelpers قدر الإمكان، مع تجنب العبث اليدوي الواسع بملفات Designer إلا عند الحاجة لتصحيح التداخل.

## نطاق التطبيق

سيتم تطوير `UIHelper` كنظام تصميم مركزي، ثم تطبيقه تدريجيًا على جميع Forms/UserControls الموجودة فعليًا، مع الحفاظ على Business Logic وRepositories وServices وDatabase وPermissions.
