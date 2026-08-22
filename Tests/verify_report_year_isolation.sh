#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
REPORT="$ROOT/DataAccess/ReportRepository.cs"
DASHBOARD="$ROOT/Services/DashboardService.cs"

fail() { echo "FAIL: $1" >&2; exit 1; }
pass() { echo "PASS: $1"; }

[ -f "$REPORT" ] || fail "ReportRepository.cs غير موجود"
[ -f "$DASHBOARD" ] || fail "DashboardService.cs غير موجود"

define_method() {
  local method="$1"
  grep -q "private DataTable $method" "$REPORT" || fail "التقرير غير موجود: $method"
}

check_year_filter() {
  local method="$1"
  local marker="$2"
  local start end body
  start=$(grep -n "private DataTable $method" "$REPORT" | head -1 | cut -d: -f1)
  end=$(awk -v s="$start" 'NR>s && /private DataTable / { print NR; exit }' "$REPORT")
  end=${end:-99999}
  body=$(sed -n "${start},${end}p" "$REPORT")
  echo "$body" | grep -q "AcademicYear" || fail "$marker لا يحتوي على AcademicYear"
  echo "$body" | grep -q "@AcademicYear" || fail "$marker لا يمرر @AcademicYear"
}

define_method GetStudentsReport
check_year_filter GetStudentsReport "تقرير الطلاب"

define_method GetClassAssignmentReport
check_year_filter GetClassAssignmentReport "تقرير توزيع الطلاب"

define_method GetFeesReport
check_year_filter GetFeesReport "تقرير الرسوم"

define_method GetMarksReport
check_year_filter GetMarksReport "تقرير الدرجات"

grep -q "GetActiveAcademicYear\|GetOperationalStatus" "$DASHBOARD" || fail "لوحة التحكم لا تقرأ حالة العام النشط"
pass "تقارير الطلاب والتوزيع والرسوم والدرجات تحتوي على عزل العام"
pass "لوحة التحكم مرتبطة بالعام النشط"

# These reports are date-based by design because their source tables do not
# consistently expose AcademicYear. They must not be falsely labelled as
# year-isolated until the database schema gains an explicit year column.
for method in GetTeacherAttendanceReport GetPayrollReport GetFinancialMovementReport; do
  define_method "$method"
done
pass "التقارير التشغيلية الزمنية موجودة ومميزة عن التقارير الأكاديمية السنوية"
