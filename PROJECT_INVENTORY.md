# جرد آلي للمشروع

## Forms و UserControls
./MainForm.Designer.cs:3:    partial class MainForm
./MainForm.cs:11:    public partial class MainForm : Form
./UI/ClassAssignmentForm.Designer.cs:7:    partial class ClassAssignmentForm
./UI/ClassAssignmentForm.cs:11:    public partial class ClassAssignmentForm : UserControl
./UI/ClassesForm.Designer.cs:3:    partial class ClassesForm
./UI/ClassesForm.cs:11:    public partial class ClassesForm : UserControl
./UI/DailyAttendanceForm.Designer.cs:3:    partial class DailyAttendanceForm
./UI/DailyAttendanceForm.cs:11:    public partial class DailyAttendanceForm : UserControl
./UI/DashboardHome.cs:11:    public partial class DashboardHome : UserControl
./UI/EnrollmentForm.Designer.cs:2:    partial class EnrollmentForm {
./UI/EnrollmentForm.cs:10:    public partial class EnrollmentForm : Form
./UI/ExpensesForm.Designer.cs:3:    partial class ExpensesForm
./UI/ExpensesForm.cs:12:    public partial class ExpensesForm : UserControl
./UI/FeePlansForm.Designer.cs:3:    partial class FeePlansForm
./UI/FeePlansForm.cs:12:    public partial class FeePlansForm : UserControl
./UI/FeesForm.Designer.cs:3:    partial class FeesForm
./UI/FeesForm.cs:12:    public partial class FeesForm : UserControl
./UI/GradeEntryForm.Designer.cs:3:    partial class GradeEntryForm
./UI/GradeEntryForm.cs:11:    public partial class GradeEntryForm : UserControl
./UI/LibraryForm.Designer.cs:3:    partial class LibraryForm
./UI/LibraryForm.cs:12:    public partial class LibraryForm : UserControl
./UI/LoginForm.Designer.cs:3:    partial class LoginForm
./UI/LoginForm.cs:11:    public partial class LoginForm : Form
./UI/PayrollForm.Designer.cs:3:    partial class PayrollForm
./UI/PayrollForm.cs:13:    public partial class PayrollForm : UserControl
./UI/ReportCenterForm.Designer.cs:3:    partial class ReportCenterForm
./UI/ReportCenterForm.cs:17:    public partial class ReportCenterForm : UserControl
./UI/StaffAttendanceForm.Designer.cs:3:    partial class StaffAttendanceForm
./UI/StaffAttendanceForm.cs:12:    public partial class StaffAttendanceForm : UserControl
./UI/StudentsForm.Designer.cs:2:    partial class StudentsForm {
./UI/StudentsForm.cs:14:    public partial class StudentsForm : Form
./UI/SubjectsForm.Designer.cs:3:    partial class SubjectsForm
./UI/SubjectsForm.cs:11:    public partial class SubjectsForm : UserControl
./UI/TeachersForm.Designer.cs:3:    partial class TeachersForm
./UI/TeachersForm.cs:13:    public partial class TeachersForm : UserControl
./UI/TimetableForm.Designer.cs:3:    partial class TimetableForm
./UI/TimetableForm.cs:11:    public partial class TimetableForm : UserControl
./UI/TransportForm.Designer.cs:3:    partial class TransportForm
./UI/TransportForm.cs:12:    public partial class TransportForm : UserControl
./UI/UsersForm.Designer.cs:3:    partial class UsersForm
./UI/UsersForm.cs:13:    public partial class UsersForm : UserControl
./UI/VouchersForm.Designer.cs:3:    partial class VouchersForm
./UI/VouchersForm.cs:12:    public partial class VouchersForm : UserControl
./UI/WelcomeScreen.cs:7:    public partial class WelcomeScreen : UserControl

## Services
./Services/BookService.cs
./Services/BorrowingService.cs
./Services/BusRouteService.cs
./Services/BusService.cs
./Services/ClassService.cs
./Services/ContractService.cs
./Services/DashboardService.cs
./Services/EnrollmentService.cs
./Services/ExpenseService.cs
./Services/FeePlanService.cs
./Services/FeeService.cs
./Services/GradeService.cs
./Services/MarkService.cs
./Services/PayrollService.cs
./Services/ReportService.cs
./Services/RoomService.cs
./Services/StudentAttendanceService.cs
./Services/StudentClassService.cs
./Services/StudentService.cs
./Services/SubjectService.cs
./Services/TeacherAttendanceService.cs
./Services/TeacherContractService.cs
./Services/TeacherService.cs
./Services/TimetableService.cs
./Services/UserService.cs
./Services/VoucherService.cs

## Repositories و DataAccess
./DataAccess/BookRepository.cs
./DataAccess/BorrowingRepository.cs
./DataAccess/BusRepository.cs
./DataAccess/BusRouteRepository.cs
./DataAccess/ClassRepository.cs
./DataAccess/ContractRepository.cs
./DataAccess/DbConnection.cs
./DataAccess/EnrollmentRepository.cs
./DataAccess/ExpenseRepository.cs
./DataAccess/FeePlanRepository.cs
./DataAccess/FeeRepository.cs
./DataAccess/GradeRepository.cs
./DataAccess/MarkRepository.cs
./DataAccess/PayrollRepository.cs
./DataAccess/ReportRepository.cs
./DataAccess/RoomRepository.cs
./DataAccess/StudentAttendanceRepository.cs
./DataAccess/StudentClassRepository.cs
./DataAccess/StudentRepository.cs
./DataAccess/SubjectRepository.cs
./DataAccess/TeacherAttendanceRepository.cs
./DataAccess/TeacherContractRepository.cs
./DataAccess/TeacherRepository.cs
./DataAccess/TimetableRepository.cs
./DataAccess/UserRepository.cs
./DataAccess/VoucherRepository.cs

## Event handlers
MainForm.cs:109:        private void timerClock_Tick(object sender, EventArgs e)
MainForm.cs:278:        private void tsmiDashboard_Click(object sender, EventArgs e)
MainForm.cs:304:        private void tsmiStudentsManage_Click(object sender, EventArgs e)
MainForm.cs:312:        private void tsmiStudentsEnroll_Click(object sender, EventArgs e)
MainForm.cs:320:        private void tsmiStudentsClasses_Click(object sender, EventArgs e)
MainForm.cs:328:        private void tsmiTeachersManage_Click(object sender, EventArgs e)
MainForm.cs:336:        private void tsmiTeachersAttendance_Click(object sender, EventArgs e)
MainForm.cs:344:        private void tsmiTeachersPayroll_Click(object sender, EventArgs e)
MainForm.cs:352:        private void tsmiSubjects_Click(object sender, EventArgs e)
MainForm.cs:360:        private void tsmiClasses_Click(object sender, EventArgs e)
MainForm.cs:368:        private void tsmiTimetable_Click(object sender, EventArgs e)
MainForm.cs:376:        private void tsmiGrades_Click(object sender, EventArgs e)
MainForm.cs:384:        private void tsmiAttendance_Click(object sender, EventArgs e)
MainForm.cs:392:        private void tsmiFees_Click(object sender, EventArgs e)
MainForm.cs:400:        private void tsmiVouchers_Click(object sender, EventArgs e)
MainForm.cs:408:        private void tsmiExpenses_Click(object sender, EventArgs e)
MainForm.cs:416:        private void tsmiTransport_Click(object sender, EventArgs e)
MainForm.cs:424:        private void tsmiLibrary_Click(object sender, EventArgs e)
MainForm.cs:432:        private void tsmiUsers_Click(object sender, EventArgs e)
MainForm.cs:440:        private void tsmiReports_Click(object sender, EventArgs e)
MainForm.cs:456:        private void tsmiLogout_Click(object sender, EventArgs e)
UI/ClassAssignmentForm.cs:288:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/ClassAssignmentForm.cs:328:        private void btnSelectAll_Click(object sender, EventArgs e)
UI/ClassAssignmentForm.cs:422:        private void dataGridViewAssigned_CellClick(object sender, DataGridViewCellEventArgs e)
UI/ClassesForm.cs:182:        private void btnClassClear_Click(object sender, EventArgs e)
UI/ClassesForm.cs:193:        private void dataGridViewClasses_CellClick(object sender, DataGridViewCellEventArgs e)
UI/ClassesForm.cs:258:        private void txtClassSearch_TextChanged(object sender, EventArgs e)
UI/ClassesForm.cs:449:        private void btnRoomClear_Click(object sender, EventArgs e)
UI/ClassesForm.cs:460:        private void dataGridViewRooms_CellClick(object sender, DataGridViewCellEventArgs e)
UI/ClassesForm.cs:539:        private void txtRoomSearch_TextChanged(object sender, EventArgs e)
UI/DailyAttendanceForm.cs:287:        private void btnMarkAllPresent_Click(object sender, EventArgs e)
UI/DailyAttendanceForm.cs:307:        private void btnClear_Click(object sender, EventArgs e)
UI/DailyAttendanceForm.cs:315:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/DailyAttendanceForm.cs:335:        private void dataGridViewAttendance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
UI/DailyAttendanceForm.cs:341:        private void dataGridViewAttendance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
UI/EnrollmentForm.cs:124:        private void cmbStudentID_SelectedIndexChanged(object sender, EventArgs e)
UI/EnrollmentForm.cs:135:        private void txtFees_TextChanged(object sender, EventArgs e)
UI/EnrollmentForm.cs:151:        private void btnSearch_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:170:        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
UI/EnrollmentForm.cs:178:        private void btnRefresh_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:185:        private void btnAdd_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:195:        private void btnSave_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:255:        private void btnUpdate_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:266:        private void btnDelete_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:28:        private void EnrollmentForm_Load(object sender, EventArgs e)
UI/EnrollmentForm.cs:294:        private void btnCancel_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:301:        private void btnClose_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:306:        private void btnPrintForm_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:316:        private void btnPrintReceipt_Click(object sender, EventArgs e)
UI/EnrollmentForm.cs:326:        private void dgvEnrollments_CellClick(object sender, DataGridViewCellEventArgs e)
UI/EnrollmentForm.cs:334:        private void dgvEnrollments_SelectionChanged(object sender, EventArgs e)
UI/ExpensesForm.cs:210:        private void FilterControls_Changed(object sender, EventArgs e)
UI/ExpensesForm.cs:291:        private void dataGridViewExpenses_CellClick(object sender, DataGridViewCellEventArgs e)
UI/ExpensesForm.cs:401:        private void btnClear_Click(object sender, EventArgs e)
UI/FeePlansForm.cs:169:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/FeePlansForm.cs:250:        private void dataGridViewFeePlans_CellClick(object sender, DataGridViewCellEventArgs e)
UI/FeePlansForm.cs:359:        private void btnClear_Click(object sender, EventArgs e)
UI/FeesForm.cs:256:        private void FilterControls_Changed(object sender, EventArgs e)
UI/FeesForm.cs:262:        private void AmountFields_TextChanged(object sender, EventArgs e)
UI/FeesForm.cs:328:        private void dataGridViewFees_CellClick(object sender, DataGridViewCellEventArgs e)
UI/FeesForm.cs:673:        private void btnClear_Click(object sender, EventArgs e)
UI/GradeEntryForm.cs:281:        private void dataGridViewGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
UI/GradeEntryForm.cs:422:        private void dataGridViewGrades_CellClick(object sender, DataGridViewCellEventArgs e)
UI/GradeEntryForm.cs:478:        private void btnClear_Click(object sender, EventArgs e)
UI/GradeEntryForm.cs:486:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/LibraryForm.cs:236:        private void dataGridViewBooks_CellClick(object sender, DataGridViewCellEventArgs e)
UI/LibraryForm.cs:346:        private void btnClearBook_Click(object sender, EventArgs e)
UI/LibraryForm.cs:562:        private void dataGridViewBorrowings_CellClick(object sender, DataGridViewCellEventArgs e)
UI/PayrollForm.cs:240:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/PayrollForm.cs:245:        private void SalaryField_TextChanged(object sender, EventArgs e)
UI/PayrollForm.cs:349:        private void dataGridViewContracts_CellClick(object sender, DataGridViewCellEventArgs e)
UI/PayrollForm.cs:524:        private void btnClear_Click(object sender, EventArgs e)
UI/PayrollForm.cs:529:        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
UI/PayrollForm.cs:545:        private void ContractNumber_KeyPress(object sender, KeyPressEventArgs e)
UI/PayrollForm.cs:559:        private void Notes_KeyPress(object sender, KeyPressEventArgs e)
UI/ReportCenterForm.cs:338:        private void btnExportExcel_Click(object sender, EventArgs e)
UI/ReportCenterForm.cs:440:        private void btnExportCsv_Click(object sender, EventArgs e)
UI/ReportCenterForm.cs:505:        private void btnExportPDF_Click(object sender, EventArgs e)
UI/ReportCenterForm.cs:636:        private void btnPrint_Click(object sender, EventArgs e)
UI/ReportCenterForm.cs:647:        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
UI/StaffAttendanceForm.cs:169:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/StaffAttendanceForm.cs:174:        private void dataGridViewAttendance_CellClick(object sender, DataGridViewCellEventArgs e)
UI/StaffAttendanceForm.cs:431:        private void btnClear_Click(object sender, EventArgs e)
UI/StaffAttendanceForm.cs:442:        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
UI/StaffAttendanceForm.cs:448:        private void dtpCheckIn_ValueChanged(object sender, EventArgs e)
UI/StaffAttendanceForm.cs:453:        private void dtpCheckOut_ValueChanged(object sender, EventArgs e)
UI/StaffAttendanceForm.cs:523:        private void txtAbsenceReason_KeyPress(object sender, KeyPressEventArgs e)
UI/StaffAttendanceForm.cs:528:        private void txtNotes_KeyPress(object sender, KeyPressEventArgs e)
UI/StudentsForm.cs:219:        private void btnAdd_Click(object sender, EventArgs e)
UI/StudentsForm.cs:224:        private void btnSave_Click(object sender, EventArgs e)
UI/StudentsForm.cs:229:        private void btnUpdate_Click(object sender, EventArgs e)
UI/StudentsForm.cs:33:        private void StudentsForm_Load(object sender, EventArgs e)
UI/StudentsForm.cs:378:        private void btnDelete_Click(object sender, EventArgs e)
UI/StudentsForm.cs:410:        private void btnCancel_Click(object sender, EventArgs e)
UI/StudentsForm.cs:415:        private void btnRefresh_Click(object sender, EventArgs e)
UI/StudentsForm.cs:420:        private void btnSearch_Click(object sender, EventArgs e)
UI/StudentsForm.cs:425:        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
UI/StudentsForm.cs:434:        private void btnChooseImage_Click(object sender, EventArgs e)
UI/StudentsForm.cs:472:        private void btnRemoveImage_Click(object sender, EventArgs e)
UI/StudentsForm.cs:488:        private void btnExportExcel_Click(object sender, EventArgs e)
UI/StudentsForm.cs:548:        private void btnPrint_Click(object sender, EventArgs e)
UI/StudentsForm.cs:559:        private void btnClose_Click(object sender, EventArgs e)
UI/StudentsForm.cs:572:        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
UI/StudentsForm.cs:580:        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
UI/SubjectsForm.cs:232:        private void btnDelete_Click(object sender, EventArgs e)
UI/SubjectsForm.cs:237:        private void btnClear_Click(object sender, EventArgs e)
UI/SubjectsForm.cs:248:        private void dataGridViewSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
UI/SubjectsForm.cs:333:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/TeachersForm.cs:346:        private void btnClear_Click(object sender, EventArgs e)
UI/TeachersForm.cs:356:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/TeachersForm.cs:361:        private void dataGridViewTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
UI/TimetableForm.cs:368:        private void btnClear_Click(object sender, EventArgs e)
UI/TimetableForm.cs:373:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/TimetableForm.cs:383:        private void dataGridViewTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
UI/TransportForm.cs:202:        private void dataGridViewBuses_CellClick(object sender, DataGridViewCellEventArgs e)
UI/TransportForm.cs:308:        private void btnClearBus_Click(object sender, EventArgs e)
UI/TransportForm.cs:457:        private void dataGridViewRoutes_CellClick(object sender, DataGridViewCellEventArgs e)
UI/TransportForm.cs:578:        private void btnClearRoute_Click(object sender, EventArgs e)
UI/UsersForm.cs:201:        private void txtSearch_TextChanged(object sender, EventArgs e)
UI/UsersForm.cs:206:        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
UI/UsersForm.cs:521:        private void btnClear_Click(object sender, EventArgs e)
UI/UsersForm.cs:532:        private void dataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
UI/UsersForm.cs:570:        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
UI/VouchersForm.cs:192:        private void FilterControls_Changed(object sender, EventArgs e)
UI/VouchersForm.cs:321:        private void dataGridViewVouchers_CellClick(object sender, DataGridViewCellEventArgs e)
UI/VouchersForm.cs:438:        private void btnClear_Click(object sender, EventArgs e)
UI/WelcomeScreen.cs:21:        private void btnStudents_Click(object sender, EventArgs e)
UI/WelcomeScreen.cs:27:        private void btnTeachers_Click(object sender, EventArgs e)
UI/WelcomeScreen.cs:33:        private void btnFinance_Click(object sender, EventArgs e)
UI/WelcomeScreen.cs:38:        private void btnAttendance_Click(object sender, EventArgs e)

## UIHelper usage
./Helpers/UIHelper.cs
./MainForm.cs
./UI/ClassAssignmentForm.cs
./UI/ClassesForm.cs
./UI/DailyAttendanceForm.cs
./UI/DashboardHome.cs
./UI/ExpensesForm.cs
./UI/FeePlansForm.cs
./UI/FeesForm.cs
./UI/GradeEntryForm.cs
./UI/LibraryForm.cs
./UI/LoginForm.cs
./UI/PayrollForm.cs
./UI/StaffAttendanceForm.cs
./UI/StudentsForm.cs
./UI/SubjectsForm.cs
./UI/TeachersForm.cs
./UI/TimetableForm.cs
./UI/TransportForm.cs
./UI/UsersForm.cs
./UI/VouchersForm.cs

## TODO/Under Development

## Project packages and references
﻿<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="BouncyCastle.Cryptography" version="2.6.2" targetFramework="net472" />
  <package id="ClosedXML" version="0.105.0" targetFramework="net472" />
  <package id="ClosedXML.Parser" version="2.0.0" targetFramework="net472" />
  <package id="ClosedXML.Report" version="0.2.12" targetFramework="net472" />
  <package id="DocumentFormat.OpenXml" version="3.1.1" targetFramework="net472" />
  <package id="DocumentFormat.OpenXml.Framework" version="3.1.1" targetFramework="net472" />
  <package id="ExcelNumberFormat" version="1.1.0" targetFramework="net472" />
  <package id="iTextSharp.LGPLv2.Core" version="3.8.1" targetFramework="net472" />
  <package id="Microsoft.Bcl.HashCode" version="1.1.1" targetFramework="net472" />
  <package id="Microsoft.CSharp" version="4.7.0" targetFramework="net472" />
  <package id="morelinq" version="4.4.0" targetFramework="net472" />
  <package id="RBush.Signed" version="4.0.0" targetFramework="net472" />
  <package id="SixLabors.Fonts" version="1.0.0" targetFramework="net472" />
  <package id="SkiaSharp" version="4.148.0" targetFramework="net472" />
  <package id="SkiaSharp.NativeAssets.macOS" version="4.148.0" targetFramework="net472" />
  <package id="SkiaSharp.NativeAssets.Win32" version="4.148.0" targetFramework="net472" />
  <package id="System.Buffers" version="4.6.1" targetFramework="net472" />
  <package id="System.Linq.Dynamic.Core" version="1.6.0.2" targetFramework="net472" />
  <package id="System.Memory" version="4.6.3" targetFramework="net472" />
  <package id="System.Numerics.Vectors" version="4.6.1" targetFramework="net472" />
  <package id="System.Runtime.CompilerServices.Unsafe" version="6.1.2" targetFramework="net472" />
</packages>11:    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
38:    <Reference Include="BouncyCastle.Cryptography, Version=2.0.0.0, Culture=neutral, PublicKeyToken=072edcf4a5328938, processorArchitecture=MSIL">
39:      <HintPath>packages\BouncyCastle.Cryptography.2.6.2\lib\net461\BouncyCastle.Cryptography.dll</HintPath>
42:    <Reference Include="ClosedXML, Version=0.105.0.0, Culture=neutral, PublicKeyToken=fd1eb21b62ae805b, processorArchitecture=MSIL">
43:      <HintPath>packages\ClosedXML.0.105.0\lib\netstandard2.0\ClosedXML.dll</HintPath>
45:    <Reference Include="ClosedXML.Parser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1d5f7376574c51ec, processorArchitecture=MSIL">
46:      <HintPath>packages\ClosedXML.Parser.2.0.0\lib\netstandard2.0\ClosedXML.Parser.dll</HintPath>
48:    <Reference Include="ClosedXML.Report, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b5435ca1fa2ab9d6, processorArchitecture=MSIL">
49:      <HintPath>packages\ClosedXML.Report.0.2.12\lib\netstandard2.0\ClosedXML.Report.dll</HintPath>
51:    <Reference Include="DocumentFormat.OpenXml, Version=3.1.1.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17, processorArchitecture=MSIL">
52:      <HintPath>packages\DocumentFormat.OpenXml.3.1.1\lib\net46\DocumentFormat.OpenXml.dll</HintPath>
54:    <Reference Include="DocumentFormat.OpenXml.Framework, Version=3.1.1.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17, processorArchitecture=MSIL">
55:      <HintPath>packages\DocumentFormat.OpenXml.Framework.3.1.1\lib\net46\DocumentFormat.OpenXml.Framework.dll</HintPath>
57:    <Reference Include="ExcelNumberFormat, Version=1.1.0.0, Culture=neutral, PublicKeyToken=23c6f5d73be07eca, processorArchitecture=MSIL">
58:      <HintPath>packages\ExcelNumberFormat.1.1.0\lib\net20\ExcelNumberFormat.dll</HintPath>
60:    <Reference Include="iTextSharp.LGPLv2.Core, Version=3.8.1.0, Culture=neutral, PublicKeyToken=51d712e21b66ad36, processorArchitecture=MSIL">
61:      <HintPath>packages\iTextSharp.LGPLv2.Core.3.8.1\lib\net462\iTextSharp.LGPLv2.Core.dll</HintPath>
63:    <Reference Include="Microsoft.Bcl.HashCode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
64:      <HintPath>packages\Microsoft.Bcl.HashCode.1.1.1\lib\net461\Microsoft.Bcl.HashCode.dll</HintPath>
66:    <Reference Include="MoreLinq, Version=4.4.0.0, Culture=neutral, PublicKeyToken=384d532d7e88985d, processorArchitecture=MSIL">
67:      <HintPath>packages\morelinq.4.4.0\lib\netstandard2.0\MoreLinq.dll</HintPath>
69:    <Reference Include="RBush, Version=4.0.0.0, Culture=neutral, PublicKeyToken=c77e27b81f4d0187, processorArchitecture=MSIL">
70:      <HintPath>packages\RBush.Signed.4.0.0\lib\net47\RBush.dll</HintPath>
72:    <Reference Include="SixLabors.Fonts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d998eea7b14cab13, processorArchitecture=MSIL">
73:      <HintPath>packages\SixLabors.Fonts.1.0.0\lib\netstandard2.0\SixLabors.Fonts.dll</HintPath>
75:    <Reference Include="SkiaSharp, Version=4.148.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756, processorArchitecture=MSIL">
76:      <HintPath>packages\SkiaSharp.4.148.0\lib\net462\SkiaSharp.dll</HintPath>
78:    <Reference Include="System" />
79:    <Reference Include="netstandard" />
80:    <Reference Include="System.Buffers, Version=4.0.5.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
81:      <HintPath>packages\System.Buffers.4.6.1\lib\net462\System.Buffers.dll</HintPath>
83:    <Reference Include="System.Collections.Immutable, Version=9.0.0.10, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
84:      <HintPath>packages\System.Collections.Immutable.9.0.10\lib\net462\System.Collections.Immutable.dll</HintPath>
86:    <Reference Include="System.Core" />
87:    <Reference Include="System.Configuration" />
88:    <Reference Include="System.Formats.Nrbf, Version=9.0.0.10, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
89:      <HintPath>packages\System.Formats.Nrbf.9.0.10\lib\net462\System.Formats.Nrbf.dll</HintPath>
91:    <Reference Include="System.Linq.Dynamic.Core, Version=1.6.0.2, Culture=neutral, PublicKeyToken=0f07ec44de6ac832, processorArchitecture=MSIL">
92:      <HintPath>packages\System.Linq.Dynamic.Core.1.6.0.2\lib\net46\System.Linq.Dynamic.Core.dll</HintPath>
94:    <Reference Include="System.Memory, Version=4.0.5.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
95:      <HintPath>packages\System.Memory.4.6.3\lib\net462\System.Memory.dll</HintPath>
97:    <Reference Include="System.Numerics" />
98:    <Reference Include="System.Numerics.Vectors, Version=4.1.6.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
99:      <HintPath>packages\System.Numerics.Vectors.4.6.1\lib\net462\System.Numerics.Vectors.dll</HintPath>
101:    <Reference Include="System.Reflection.Metadata, Version=9.0.0.10, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
102:      <HintPath>packages\System.Reflection.Metadata.9.0.10\lib\net462\System.Reflection.Metadata.dll</HintPath>
104:    <Reference Include="System.Resources.Extensions, Version=9.0.0.10, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
105:      <HintPath>packages\System.Resources.Extensions.9.0.10\lib\net462\System.Resources.Extensions.dll</HintPath>
107:    <Reference Include="System.Runtime.CompilerServices.Unsafe, Version=6.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
108:      <HintPath>packages\System.Runtime.CompilerServices.Unsafe.6.1.2\lib\net462\System.Runtime.CompilerServices.Unsafe.dll</HintPath>
110:    <Reference Include="System.ValueTuple, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL">
111:      <HintPath>packages\System.ValueTuple.4.5.0\lib\net47\System.ValueTuple.dll</HintPath>
113:    <Reference Include="System.Xml.Linq" />
114:    <Reference Include="System.Data.DataSetExtensions" />
115:    <Reference Include="Microsoft.CSharp" />
116:    <Reference Include="System.Data" />
117:    <Reference Include="System.Deployment" />
118:    <Reference Include="System.Drawing" />
119:    <Reference Include="System.Net.Http" />
120:    <Reference Include="System.Windows.Forms" />
121:    <Reference Include="System.Xml" />
122:    <Reference Include="WindowsBase" />
125:    <Compile Include="DataAccess\BookRepository.cs" />
126:    <Compile Include="DataAccess\BorrowingRepository.cs" />
127:    <Compile Include="DataAccess\BusRepository.cs" />
128:    <Compile Include="DataAccess\BusRouteRepository.cs" />
129:    <Compile Include="DataAccess\ClassRepository.cs" />
130:    <Compile Include="DataAccess\ContractRepository.cs" />
131:    <Compile Include="DataAccess\DbConnection.cs" />
132:    <Compile Include="DataAccess\EnrollmentRepository.cs" />
133:    <Compile Include="DataAccess\ExpenseRepository.cs" />
134:    <Compile Include="DataAccess\FeePlanRepository.cs" />
135:    <Compile Include="DataAccess\FeeRepository.cs" />
136:    <Compile Include="DataAccess\GradeRepository.cs" />
137:    <Compile Include="DataAccess\MarkRepository.cs" />
138:    <Compile Include="DataAccess\PayrollRepository.cs" />
139:    <Compile Include="DataAccess\ReportRepository.cs" />
140:    <Compile Include="DataAccess\RoomRepository.cs" />
141:    <Compile Include="DataAccess\StudentAttendanceRepository.cs" />
142:    <Compile Include="DataAccess\StudentClassRepository.cs" />
143:    <Compile Include="DataAccess\StudentRepository.cs" />
144:    <Compile Include="DataAccess\SubjectRepository.cs" />
145:    <Compile Include="DataAccess\TeacherAttendanceRepository.cs" />
146:    <Compile Include="DataAccess\TeacherContractRepository.cs" />
147:    <Compile Include="DataAccess\TeacherRepository.cs" />
148:    <Compile Include="DataAccess\TimetableRepository.cs" />
149:    <Compile Include="DataAccess\UserRepository.cs" />
150:    <Compile Include="DataAccess\VoucherRepository.cs" />
151:        <Compile Include="Helpers\ApplicationLogger.cs" />
152:    <Compile Include="Helpers\MenuRenderer.cs" />
153:    <Compile Include="Helpers\UIHelper.cs" />
155:    <Compile Include="MainForm.cs">
158:    <Compile Include="MainForm.Designer.cs">
161:    <Compile Include="Models\Book.cs" />
162:    <Compile Include="Models\Borrowing.cs" />
163:    <Compile Include="Models\Bus.cs" />
164:    <Compile Include="Models\BusRoute.cs" />
165:    <Compile Include="Models\FeePlan.cs" />
166:    <Compile Include="Models\Room.cs" />
167:    <Compile Include="Models\SchoolClass.cs" />
168:    <Compile Include="Models\Enrollment.cs" />
169:    <Compile Include="Models\Expense.cs" />
170:    <Compile Include="Models\Fee.cs" />
171:    <Compile Include="Models\Marks.cs" />
172:    <Compile Include="Models\Payroll.cs" />
173:    <Compile Include="Models\ReportRequest.cs" />
174:    <Compile Include="Models\StudentAttendance.cs" />
175:    <Compile Include="Models\StudentClass.cs" />
176:    <Compile Include="Models\StudentGrade.cs" />
177:    <Compile Include="Models\Students.cs" />
178:    <Compile Include="Models\Subjects.cs" />
179:    <Compile Include="Models\TeacherAttendance.cs" />
180:    <Compile Include="Models\TeacherContract.cs" />
181:    <Compile Include="Models\Teachers.cs" />
182:    <Compile Include="Models\TimetableEntry.cs" />
183:    <Compile Include="Models\User.cs" />
184:    <Compile Include="Models\Voucher.cs" />
185:    <Compile Include="Program.cs" />
186:    <Compile Include="Properties\AssemblyInfo.cs" />
187:    <Compile Include="Security\CurrentUser.cs" />
188:    <Compile Include="Security\PasswordHasher.cs" />
189:    <Compile Include="Security\PermissionKeys.cs" />
190:    <Compile Include="Services\BookService.cs" />
191:    <Compile Include="Services\BorrowingService.cs" />
192:    <Compile Include="Services\BusRouteService.cs" />
193:    <Compile Include="Services\BusService.cs" />
194:    <Compile Include="Services\ClassService.cs" />
195:    <Compile Include="Services\ContractService.cs" />
196:    <Compile Include="Services\DashboardService.cs" />
197:    <Compile Include="Services\EnrollmentService.cs" />
198:    <Compile Include="Services\ExpenseService.cs" />
199:    <Compile Include="Services\FeePlanService.cs" />
200:    <Compile Include="Services\FeeService.cs" />
201:    <Compile Include="Services\GradeService.cs" />
202:    <Compile Include="Services\MarkService.cs" />
203:    <Compile Include="Services\PayrollService.cs" />
204:    <Compile Include="Services\ReportService.cs" />
205:    <Compile Include="Services\RoomService.cs" />
206:    <Compile Include="Services\StudentAttendanceService.cs" />
207:    <Compile Include="Services\StudentClassService.cs" />
208:    <Compile Include="Services\StudentService.cs" />
209:    <Compile Include="Services\SubjectService.cs" />
210:    <Compile Include="Services\TeacherAttendanceService.cs" />
211:    <Compile Include="Services\TeacherContractService.cs" />
212:    <Compile Include="Services\TeacherService.cs" />
213:    <Compile Include="Services\TimetableService.cs" />
214:    <Compile Include="Services\UserService.cs" />
215:    <Compile Include="Services\VoucherService.cs" />
216:    <Compile Include="UI\ClassAssignmentForm.cs">
219:    <Compile Include="UI\ClassAssignmentForm.Designer.cs">
222:    <Compile Include="UI\ClassesForm.cs">
225:    <Compile Include="UI\ClassesForm.Designer.cs">
228:    <Compile Include="UI\DailyAttendanceForm.cs">
231:    <Compile Include="UI\DailyAttendanceForm.Designer.cs">
234:    <Compile Include="UI\DashboardHome.cs">
237:    <Compile Include="UI\DashboardHome.Designer.cs">
240:    <Compile Include="UI\EnrollmentForm.cs">
243:    <Compile Include="UI\EnrollmentForm.Designer.cs">
246:    <Compile Include="UI\ExpensesForm.cs">
249:    <Compile Include="UI\ExpensesForm.Designer.cs">
252:    <Compile Include="UI\FeePlansForm.cs">
255:    <Compile Include="UI\FeePlansForm.Designer.cs">
258:    <Compile Include="UI\FeesForm.cs">
261:    <Compile Include="UI\FeesForm.Designer.cs">
264:    <Compile Include="UI\GradeEntryForm.cs">
267:    <Compile Include="UI\GradeEntryForm.Designer.cs">
270:    <Compile Include="UI\LibraryForm.cs">
273:    <Compile Include="UI\LibraryForm.Designer.cs">
276:    <Compile Include="UI\LoginForm.cs">
279:    <Compile Include="UI\LoginForm.Designer.cs">
282:    <Compile Include="UI\PayrollForm.cs">
285:    <Compile Include="UI\PayrollForm.Designer.cs">
288:    <Compile Include="UI\ReportCenterForm.cs">
291:    <Compile Include="UI\ReportCenterForm.Designer.cs">
294:    <Compile Include="UI\StaffAttendanceForm.cs">
297:    <Compile Include="UI\StaffAttendanceForm.Designer.cs">
300:    <Compile Include="UI\StudentsForm.cs">
303:    <Compile Include="UI\StudentsForm.Designer.cs">
306:    <Compile Include="UI\SubjectsForm.cs">
309:    <Compile Include="UI\SubjectsForm.Designer.cs">
312:    <Compile Include="UI\TeachersForm.cs">
315:    <Compile Include="UI\TeachersForm.Designer.cs">
318:    <Compile Include="UI\TimetableForm.cs">
321:    <Compile Include="UI\TimetableForm.Designer.cs">
324:    <Compile Include="UI\TransportForm.cs">
327:    <Compile Include="UI\TransportForm.Designer.cs">
330:    <Compile Include="UI\UsersForm.cs">
333:    <Compile Include="UI\UsersForm.Designer.cs">
336:    <Compile Include="UI\VouchersForm.cs">
339:    <Compile Include="UI\VouchersForm.Designer.cs">
342:    <Compile Include="UI\WelcomeScreen.cs">
345:    <Compile Include="UI\WelcomeScreen.Designer.cs">
356:    <Compile Include="Properties\Resources.Designer.cs">
419:    <Compile Include="Properties\Settings.Designer.cs">
