# جرد الواجهات والحقول


## MainForm.cs
    public partial class MainForm : Form
Services/Repositories: 
Controls declared: System System.Windows.Forms.Label System.Windows.Forms.MenuStrip System.Windows.Forms.Panel System.Windows.Forms.StatusStrip System.Windows.Forms.Timer System.Windows.Forms.ToolStripMenuItem System.Windows.Forms.ToolStripStatusLabel void 
Validation hooks: UIHelper.LogException UIHelper.ShowException 
Buttons/events: ToolStripMenuItem_Click tsmiAttendance_Click tsmiClasses_Click tsmiDashboard_Click tsmiExpenses_Click tsmiFees_Click tsmiGrades_Click tsmiLibrary_Click tsmiLogout_Click tsmiReports_Click tsmiStudentsClasses_Click tsmiStudentsEnroll_Click tsmiStudentsManage_Click tsmiSubjects_Click tsmiTeachersAttendance_Click tsmiTeachersManage_Click tsmiTeachersPayroll_Click tsmiTimetable_Click tsmiTransport_Click tsmiUsers_Click tsmiVouchers_Click 

## UI/ClassAssignmentForm.cs
    public partial class ClassAssignmentForm : UserControl
Services/Repositories: ClassService StudentClassService classService new ClassService new StudentClassService studentClassService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckedListBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAssign_Click btnLoad_Click btnRemove_Click btnSelectAll_Click 

## UI/ClassesForm.cs
    public partial class ClassesForm : UserControl
Services/Repositories: ClassService RoomService classService new ClassService new RoomService roomService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.NumericUpDown System.Windows.Forms.Panel System.Windows.Forms.TabControl System.Windows.Forms.TabPage System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnClassClear_Click btnClassRefresh_Click btnClassUpdate_Click btnRoomAdd_Click btnRoomClear_Click btnRoomDelete_Click btnRoomRefresh_Click btnRoomUpdate_Click 

## UI/DailyAttendanceForm.cs
    public partial class DailyAttendanceForm : UserControl
Services/Repositories: ClassService StudentAttendanceService attendanceService classService new ClassService new StudentAttendanceService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnClear_Click btnLoad_Click btnMarkAllPresent_Click btnRefresh_Click btnSave_Click 

## UI/DashboardHome.cs
    public partial class DashboardHome : UserControl
Services/Repositories: DashboardService dashboardService new DashboardService 
Controls declared: System System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel void 
Validation hooks: UIHelper.ApplyTheme 
Buttons/events: 

## UI/EnrollmentForm.cs
    public partial class EnrollmentForm : Form
Services/Repositories: ClassService EnrollmentService StudentService classService enrollmentService new ClassService new EnrollmentService new StudentService studentService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.ErrorProvider System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.RichTextBox System.Windows.Forms.SplitContainer System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: 
Buttons/events: btnAdd btnAdd_Click btnCancel_Click btnClose_Click btnDelete btnDelete_Click btnPrintForm_Click btnPrintReceipt_Click btnRefresh_Click btnSave btnSave_Click btnSearch_Click btnUpdate btnUpdate_Click 

## UI/ExpensesForm.cs
    public partial class ExpensesForm : UserControl
Services/Repositories: ExpenseService expenseService new ExpenseService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnUpdate_Click 

## UI/FeePlansForm.cs
    public partial class FeePlansForm : UserControl
Services/Repositories: FeePlanService feePlanService new FeePlanService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnUpdate_Click 

## UI/FeesForm.cs
    public partial class FeesForm : UserControl
Services/Repositories: FeeService StudentService VoucherService feeService new FeeService new StudentService new VoucherService studentService voucherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnGenerateFees_Click btnUpdate_Click 

## UI/GradeEntryForm.cs
    public partial class GradeEntryForm : UserControl
Services/Repositories: ClassService GradeService classService gradeService new ClassService new GradeService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue UIHelper.ShowException 
Buttons/events: btnClear_Click btnDeleteGrade_Click btnLoad_Click btnRefresh_Click btnSaveAll_Click 

## UI/LibraryForm.cs
    public partial class LibraryForm : UserControl
Services/Repositories: BookService BorrowingService StudentService TeacherService bookService borrowingService new BookService new BorrowingService new StudentService new TeacherService studentService teacherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TabControl System.Windows.Forms.TabPage System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.ShowException 
Buttons/events: btnAddBook_Click btnBorrow_Click btnClearBook_Click btnDeleteBook_Click btnReturn_Click btnUpdateBook_Click 

## UI/LoginForm.cs
    public partial class LoginForm : Form
Services/Repositories: UserService new UserService userService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.AccentColor UIHelper.ApplyTheme UIHelper.CardColor UIHelper.MutedTextColor UIHelper.NeutralColor UIHelper.PrimaryColor UIHelper.ShowError UIHelper.ShowWarning UIHelper.StyleButton UIHelper.StyleTextBox UIHelper.SuccessColor UIHelper.TextColor 
Buttons/events: btnExit btnExit_Click btnLogin btnLogin_Click 

## UI/PayrollForm.cs
    public partial class PayrollForm : UserControl
Services/Repositories: ContractService TeacherService contractService new ContractService new TeacherService teacherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnUpdate_Click 

## UI/ReportCenterForm.cs
    public partial class ReportCenterForm : UserControl
Services/Repositories: ClassService ReportService classService new ClassService new ReportService reportService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.PrintPreviewDialog System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: 
Buttons/events: btnExportCsv btnExportCsv_Click btnExportExcel btnExportExcel_Click btnExportPDF btnExportPDF_Click btnLoad btnLoad_Click btnPrint btnPrint_Click btnRefresh btnRefresh_Click 

## UI/StaffAttendanceForm.cs
    public partial class StaffAttendanceForm : UserControl
Services/Repositories: TeacherAttendanceService TeacherService attendanceService new TeacherAttendanceService new TeacherService teacherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnRefresh_Click btnUpdate_Click 

## UI/StudentsForm.cs
    public partial class StudentsForm : Form
Services/Repositories: StudentService new StudentService studentService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.ErrorProvider System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.PictureBox System.Windows.Forms.SplitContainer System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.AccentColor UIHelper.AllowOnlyNumbers UIHelper.ApplyStyle UIHelper.BackgroundColor UIHelper.DangerColor UIHelper.ExportColor UIHelper.NeutralColor UIHelper.PreventNumbers UIHelper.PrimaryColor UIHelper.SearchColor UIHelper.ShowError UIHelper.ShowInfo UIHelper.ShowSuccess UIHelper.ShowWarning UIHelper.StyleButton UIHelper.StyleComboBox UIHelper.StyleDataGridView UIHelper.StyleTextBox UIHelper.SuccessColor 
Buttons/events: btnAdd btnAdd_Click btnCancel btnCancel_Click btnChooseImage_Click btnClose btnClose_Click btnDelete btnDelete_Click btnExportExcel btnExportExcel_Click btnPrint btnPrint_Click btnRefresh btnRefresh_Click btnReload btnRemoveImage_Click btnSave btnSave_Click btnSearch btnSearch_Click btnUpdate btnUpdate_Click 

## UI/SubjectsForm.cs
    public partial class SubjectsForm : UserControl
Services/Repositories: ClassService SubjectService classService new ClassService new SubjectService subjectService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.NumericUpDown System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue UIHelper.ShowException 
Buttons/events: btnAdd btnAdd_Click btnClear_Click btnDelete btnDelete_Click btnRefresh_Click btnUpdate_Click 

## UI/TeachersForm.cs
    public partial class TeachersForm : UserControl
Services/Repositories: TeacherService new TeacherService teacherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.NumericUpDown System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd btnAdd_Click btnClear btnClear_Click btnDelete btnDelete_Click btnRefresh btnRefresh_Click btnUpdate btnUpdate_Click 

## UI/TimetableForm.cs
    public partial class TimetableForm : UserControl
Services/Repositories: ClassService TimetableService classService new ClassService new TimetableService timetableService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.NumericUpDown System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue UIHelper.ShowException 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnRefresh_Click btnUpdate_Click 

## UI/TransportForm.cs
    public partial class TransportForm : UserControl
Services/Repositories: BusRouteService BusService busService new BusRouteService new BusService routeService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TabControl System.Windows.Forms.TabPage System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.ShowException 
Buttons/events: btnAddBus_Click btnAddRoute_Click btnClearBus_Click btnClearRoute_Click btnDeleteBus_Click btnDeleteRoute_Click btnUpdateBus_Click btnUpdateRoute_Click 

## UI/UsersForm.cs
    public partial class UsersForm : UserControl
Services/Repositories: UserService new UserService userService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.CheckedListBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.GroupBox System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnRefresh_Click btnUpdate_Click 

## UI/VouchersForm.cs
    public partial class VouchersForm : UserControl
Services/Repositories: VoucherService new VoucherService voucherService 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.CheckBox System.Windows.Forms.ComboBox System.Windows.Forms.DataGridView System.Windows.Forms.DateTimePicker System.Windows.Forms.Label System.Windows.Forms.Panel System.Windows.Forms.TableLayoutPanel System.Windows.Forms.TextBox void 
Validation hooks: UIHelper.EscapeDataViewFilterValue UIHelper.ShowException 
Buttons/events: btnAdd_Click btnClear_Click btnDelete_Click btnUpdate_Click 

## UI/WelcomeScreen.cs
    public partial class WelcomeScreen : UserControl
Services/Repositories: 
Controls declared: System System.Windows.Forms.Button System.Windows.Forms.FlowLayoutPanel System.Windows.Forms.Label System.Windows.Forms.Panel void 
Validation hooks: 
Buttons/events: btnAttendance_Click btnFinance_Click btnStudents_Click btnTeachers_Click 
