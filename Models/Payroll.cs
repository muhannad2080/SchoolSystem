using System;


namespace SchoolSystem.Models
{

    /// <summary>سجل راتب معلم — مطابق مباشرة لجدول dbo.Payroll في قاعدة البيانات.</summary>
    public class Payroll
    {

        public int PayrollID { get; set; }

        public int TeacherID { get; set; }

        /// <summary>اسم المعلم من جدول Teachers (للعرض فقط).</summary>
        public string TeacherName { get; set; }

        /// <summary>الشهر (1-12) — عمود SalaryMonth.</summary>
        public int SalaryMonth { get; set; }

        /// <summary>السنة الميلادية — عمود SalaryYear.</summary>
        public int SalaryYear { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal Allowances { get; set; }

        public decimal Deductions { get; set; }

        /// <summary>عمود محسوب في قاعدة البيانات (BasicSalary + Allowances - Deductions) — للقراءة فقط.</summary>
        public decimal NetSalary { get; set; }

        /// <summary>تاريخ الصرف الفعلي؛ قيمة null تعني أن الراتب لم يُصرف بعد.</summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>طريقة الصرف المرتبطة بسند الصرف التلقائي؛ لا تُخزّن في جدول Payroll نفسه.</summary>
        public string PaymentMethod { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
