# تدقيق أخطاء تحميل الشاشات

## النتيجة الأولية

أظهر تدقيق DataAccess أن المستودعات تستخدم الجداول التالية: `BookBorrowings`, `Books`, `Buses`, `BusRoutes`, `Classes`, `Enrollments`, `Expenses`, `FeePlans`, `Fees`, `Grades`, `Marks`, `Payroll`, `Receipts`, `Rooms`, `SchoolTimetable`, `StudentAttendance`, `StudentClasses`, `StudentFees`, `StudentGrades`, `Students`, `Subjects`, `TeacherAttendance`, `TeacherContracts`, `Teachers`, `Users`, و`Vouchers`.

ملف `SchoolDB.SQL` يعرّف صراحة جزءًا محدودًا فقط من الجداول الأساسية والتشغيلية، بينما `Migration_Step1.sql` يضيف أعمدة إلى الجداول وينشئ `Enrollments` فقط. لذلك فإن قاعدة موجودة قد تحتوي على بعض الجداول القديمة، لكنها قد تفتقد جداول مثل الرسوم، الرواتب، العقود، المكتبة، التقارير، الجدول، والمصروفات. عند فتح الشاشات تتحول أخطاء SQL مثل `Invalid object name` إلى رسالة UI العامة «تعذر تحميل الشاشة» عبر `UIHelper.ShowException`.

## الإجراء التالي

مطابقة استعلامات كل Repository مع Models وملفات Forms، ثم إنشاء migration شاملة idempotent للجداول المفقودة والأعمدة والفهارس الضرورية، وتحسين رسالة الخطأ لتسجيل وعرض السبب التشغيلي المفيد دون كشف stack trace.
