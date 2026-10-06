using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class FeePlanRepository
    {
        // تعريف الدالة GetAllFeePlans لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllFeePlans()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        fp.FeePlanID,
                        fp.AcademicYear,
                        fp.ClassID,
                        c.ClassName,
                        fp.FeeType,
                        fp.Amount,
                        fp.DueDate,
                        fp.IsRequired,
                        fp.Notes,
                        fp.CreatedAt
                    FROM FeePlans fp
                    INNER JOIN Classes c ON fp.ClassID = c.ClassID
                    ORDER BY fp.AcademicYear DESC, c.ClassName, fp.FeeType";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة GetClasses لتنفيذ العملية المرتبطة بها.
        public DataTable GetClasses()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT ClassID, ClassName
                    FROM Classes
                    ORDER BY ClassName";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة GetFeeTypes لتنفيذ العملية المرتبطة بها.
        public DataTable GetFeeTypes()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT FeeType
                    FROM
                    (
                        SELECT DISTINCT LTRIM(RTRIM(FeeType)) AS FeeType
                        FROM FeePlans
                        WHERE NULLIF(LTRIM(RTRIM(FeeType)), N'') IS NOT NULL

                        UNION ALL

                        SELECT N'الرسوم الكلية'
                        UNION ALL SELECT N'رسوم تسجيل'
                        UNION ALL SELECT N'رسوم دراسية'
                        UNION ALL SELECT N'رسوم كتب'
                        UNION ALL SELECT N'رسوم زي مدرسي'
                        UNION ALL SELECT N'رسوم مواصلات'
                        UNION ALL SELECT N'رسوم امتحانات'
                        UNION ALL SELECT N'رسوم أنشطة'
                        UNION ALL SELECT N'أخرى'
                    ) FeeTypes
                    WHERE NULLIF(LTRIM(RTRIM(FeeType)), N'') IS NOT NULL
                    GROUP BY FeeType
                    ORDER BY CASE FeeType
                        WHEN N'الرسوم الكلية' THEN 0
                        WHEN N'رسوم تسجيل' THEN 1
                        WHEN N'رسوم دراسية' THEN 2
                        WHEN N'رسوم كتب' THEN 3
                        WHEN N'رسوم زي مدرسي' THEN 4
                        WHEN N'رسوم مواصلات' THEN 5
                        WHEN N'رسوم امتحانات' THEN 6
                        WHEN N'رسوم أنشطة' THEN 7
                        WHEN N'أخرى' THEN 8
                        ELSE 99
                    END, FeeType;";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة GetRegistrationFeePlan لتنفيذ العملية المرتبطة بها.
        public FeePlan GetRegistrationFeePlan(int studentId, string academicYear, int classId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT TOP (1)
                        fp.FeePlanID, fp.AcademicYear, fp.ClassID, c.ClassName,
                        fp.FeeType, fp.Amount, fp.DueDate, fp.IsRequired, fp.Notes, fp.CreatedAt
                    FROM FeePlans fp
                    INNER JOIN Classes c ON c.ClassID = fp.ClassID
                    WHERE fp.ClassID = @ClassID
                      AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') =
                          REPLACE(@AcademicYear, N'-', N'/')
                      AND
                      (
                          LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                          (
                              N'رسوم تسجيل',
                              N'رسوم التسجيل',
                              N'Registration Fee'
                          )
                          OR LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) LIKE N'%تسجيل%'
                      )
                      AND EXISTS
                      (
                          SELECT 1
                          FROM Students s
                          WHERE s.StudentID = @StudentID
                      )
                    ORDER BY
                        CASE
                            WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = N'رسوم تسجيل' THEN 0
                            WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = N'رسوم التسجيل' THEN 1
                            WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = N'Registration Fee' THEN 2
                            ELSE 3
                        END,
                        fp.FeePlanID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;

                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;

                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim().Replace('-', '/');

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return MapFeePlan(reader);
                    }
                }
            }
        }

        // تعريف الدالة GetStudentFeePlan لتنفيذ العملية المرتبطة بها.
        public FeePlan GetStudentFeePlan(int studentId, string academicYear, string feeType)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT TOP (1)
                        fp.FeePlanID, fp.AcademicYear, fp.ClassID, c.ClassName,
                        fp.FeeType, fp.Amount, fp.DueDate, fp.IsRequired, fp.Notes, fp.CreatedAt
                    FROM FeePlans fp
                    INNER JOIN Classes c ON c.ClassID = fp.ClassID
                    WHERE REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = LTRIM(RTRIM(@FeeType))
                      AND
                      (
                          EXISTS
                          (
                              SELECT 1
                              FROM StudentClasses sc
                              WHERE sc.StudentID = @StudentID
                                AND sc.ClassID = fp.ClassID
                                AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          )
                          OR EXISTS
                          (
                              SELECT 1
                              FROM Enrollments e
                              WHERE e.StudentID = @StudentID
                                AND e.ClassID = fp.ClassID
                                AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                          )
                      )
                    ORDER BY fp.FeePlanID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;

                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim().Replace('-', '/');

                    cmd.Parameters.Add("@FeeType", SqlDbType.NVarChar, 100).Value = (feeType ?? string.Empty).Trim();

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;


                        return MapFeePlan(reader);
                    }
                }
            }
        }

        // تعريف الدالة MapFeePlan لتنفيذ العملية المرتبطة بها.
        private static FeePlan MapFeePlan(SqlDataReader reader)
        {

            return new FeePlan
            {
                FeePlanID = Convert.ToInt32(reader["FeePlanID"]),
                AcademicYear = reader["AcademicYear"].ToString(),
                ClassID = Convert.ToInt32(reader["ClassID"]),
                ClassName = reader["ClassName"].ToString(),
                FeeType = reader["FeeType"].ToString(),
                Amount = Convert.ToDecimal(reader["Amount"]),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                IsRequired = reader["IsRequired"] != DBNull.Value && Convert.ToBoolean(reader["IsRequired"]),
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : reader["Notes"].ToString(),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"])

            };
        }

        // تعريف الدالة AddFeePlan لتنفيذ العملية المرتبطة بها.
        public bool AddFeePlan(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    IF EXISTS
                    (
                        SELECT 1 FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND ClassID = @ClassID
                          AND FeeType = @FeeType
                    )
                        THROW 51020, N'توجد خطة رسوم مكررة لنفس العام والصف ونوع الرسوم.', 1;

                    INSERT INTO FeePlans
                    (
                        AcademicYear,
                        ClassID,
                        FeeType,
                        Amount,
                        DueDate,
                        IsRequired,
                        Notes
                    )
                    VALUES
                    (
                        @AcademicYear,
                        @ClassID,
                        @FeeType,
                        @Amount,
                        @DueDate,
                        @IsRequired,
                        @Notes
                    );

                    DECLARE @NewFeePlanID INT = CONVERT(INT, SCOPE_IDENTITY());

                    /* نشر الخطة تلقائياً للطلاب المقبولين والموزعين في الصف نفسه. */
                    INSERT INTO Fees
                    (
                        StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount,
                        DiscountAmount, NetAmount, PaidAmount, RemainingAmount,
                        DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes
                    )
                    SELECT DISTINCT
                        sc.StudentID,
                        @NewFeePlanID,
                        @AcademicYear,
                        @FeeType,
                        @Amount,
                        0,
                        @Amount,
                        0,
                        @Amount,
                        @DueDate,
                        NULL,
                        NULL,
                        NULL,
                        N'غير مسدد',
                        @Notes
                    FROM StudentClasses sc
                    INNER JOIN Enrollments e ON e.StudentID = sc.StudentID
                        AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                        AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                    WHERE sc.ClassID = @ClassID
                      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND NOT EXISTS
                      (
                          SELECT 1 FROM Fees f
                          WHERE f.StudentID = sc.StudentID
                            AND f.FeePlanID = @NewFeePlanID
                      )

                    UNION

                    SELECT DISTINCT
                        e.StudentID,
                        @NewFeePlanID,
                        @AcademicYear,
                        @FeeType,
                        @Amount,
                        0,
                        @Amount,
                        0,
                        @Amount,
                        @DueDate,
                        NULL,
                        NULL,
                        NULL,
                        N'غير مسدد',
                        @Notes
                    FROM Enrollments e
                    WHERE e.ClassID = @ClassID
                      AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                      AND NOT EXISTS
                      (
                          SELECT 1 FROM Fees f
                          WHERE f.StudentID = e.StudentID
                            AND f.FeePlanID = @NewFeePlanID
                      )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, plan);


                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        cmd.Transaction = transaction;

                        int affected = cmd.ExecuteNonQuery();

                        /* أي بند فرعي جديد يرفع/يخفض إجمالي رسوم الصف والعام نفسه
                           ليظل مجموع البنود مساويًا للكلي. */
                        if (affected > 0 &&
                            !IsTotalFeeType(plan.FeeType))
                        {
                            const string syncTotalSql = @"
                                DECLARE @TotalAmount DECIMAL(18,2);

                                SELECT @TotalAmount =
                                    ISNULL(SUM(Amount), 0)
                                FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                                WHERE ClassID = @ClassID
                                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                      REPLACE(@AcademicYear, N'-', N'/')
                                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');

                                IF EXISTS
                                (
                                    SELECT 1
                                    FROM FeePlans
                                    WHERE ClassID = @ClassID
                                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                          REPLACE(@AcademicYear, N'-', N'/')
                                      AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                                          (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                )
                                BEGIN
                                    UPDATE FeePlans
                                    SET Amount = @TotalAmount,
                                        DueDate = @DueDate,
                                        UpdatedAt = GETDATE()
                                    WHERE ClassID = @ClassID
                                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                          REPLACE(@AcademicYear, N'-', N'/')
                                      AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                                          (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');

                                    UPDATE f
                                    SET TotalAmount = @TotalAmount,
                                        NetAmount =
                                            CASE
                                                WHEN ISNULL(f.DiscountAmount, 0) > @TotalAmount
                                                    THEN 0
                                                ELSE @TotalAmount - ISNULL(f.DiscountAmount, 0)
                                            END,
                                        RemainingAmount =
                                            CASE
                                                WHEN ISNULL(f.DiscountAmount, 0) > @TotalAmount
                                                    THEN 0
                                                ELSE @TotalAmount - ISNULL(f.DiscountAmount, 0) - ISNULL(f.PaidAmount, 0)
                                            END,
                                        UpdatedAt = GETDATE()
                                    FROM Fees f
                                    INNER JOIN FeePlans fp ON fp.FeePlanID = f.FeePlanID
                                    WHERE fp.ClassID = @ClassID
                                      AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') =
                                          REPLACE(@AcademicYear, N'-', N'/')
                                      AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                                          (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                      AND ISNULL(f.PaidAmount, 0) = 0;
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO FeePlans
                                    (
                                        AcademicYear,
                                        ClassID,
                                        FeeType,
                                        Amount,
                                        DueDate,
                                        IsRequired,
                                        Notes
                                    )
                                    VALUES
                                    (
                                        @AcademicYear,
                                        @ClassID,
                                        N'الرسوم الكلية',
                                        @TotalAmount,
                                        @DueDate,
                                        1,
                                        @Notes
                                    );
                                END";

                            using (SqlCommand syncTotal =
                                new SqlCommand(
                                    syncTotalSql,
                                    con,
                                    transaction))
                            {
                                syncTotal.Parameters.AddWithValue(
                                    "@ClassID",
                                    plan.ClassID);
                                syncTotal.Parameters.AddWithValue(
                                    "@AcademicYear",
                                    plan.AcademicYear ?? string.Empty);
                                syncTotal.Parameters.AddWithValue(
                                    "@DueDate",
                                    plan.DueDate.Date);
                                syncTotal.Parameters.AddWithValue(
                                    "@Notes",
                                    string.IsNullOrWhiteSpace(plan.Notes)
                                        ? (object)DBNull.Value
                                        : plan.Notes);

                                syncTotal.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        return affected > 0;
                    }
                }
            }
        }

        // تعريف الدالة DistributeTotalFee لتنفيذ العملية المرتبطة بها.
        private int EnsureStudentFeesForPlan(
            SqlConnection con,
            SqlTransaction transaction,
            int feePlanId,
            int classId,
            string academicYear,
            string feeType,
            decimal amount,
            DateTime dueDate,
            string notes)
        {
            const string sql = @"
                INSERT INTO Fees
                (
                    StudentID, FeePlanID, AcademicYear, FeeType,
                    TotalAmount, DiscountAmount, NetAmount, PaidAmount,
                    RemainingAmount, DueDate, PaymentDate, PaymentMethod,
                    ReceiptNumber, Status, Notes
                )
                SELECT DISTINCT
                    source.StudentID,
                    @FeePlanID, @AcademicYear, @FeeType,
                    @Amount, 0, @Amount, 0, @Amount, @DueDate,
                    NULL, NULL, NULL, N'غير مسدد', @Notes
                FROM
                (
                    SELECT sc.StudentID
                    FROM StudentClasses sc
                    INNER JOIN Enrollments e ON e.StudentID = sc.StudentID
                       AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                       AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                    WHERE sc.ClassID = @ClassID
                      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                    UNION
                    SELECT e.StudentID
                    FROM Enrollments e
                    WHERE e.ClassID = @ClassID
                      AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                ) source
                WHERE NOT EXISTS
                (
                    SELECT 1 FROM Fees f
                    WHERE f.StudentID = source.StudentID
                      AND f.FeePlanID = @FeePlanID
                );";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                command.Parameters.AddWithValue("@FeePlanID", feePlanId);
                command.Parameters.AddWithValue("@ClassID", classId);
                command.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace("-", "/"));
                command.Parameters.AddWithValue("@FeeType", (feeType ?? string.Empty).Trim());
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@DueDate", dueDate.Date);
                command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);
                return command.ExecuteNonQuery();
            }
        }
        public int DistributeTotalFee(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                using (SqlTransaction transaction =
                    con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int totalPlanId = EnsureTotalPlan(
                            con,
                            transaction,
                            plan);

                        List<Tuple<int, string, decimal>> childPlans =
                            LoadChildPlans(
                                con,
                                transaction,
                                plan.ClassID,
                                plan.AcademicYear);

                        string[] defaultTypes =
                        {
                            "رسوم تسجيل",
                            "رسوم دراسية",
                            "رسوم كتب",
                            "رسوم زي مدرسي",
                            "رسوم مواصلات",
                            "رسوم امتحانات",
                            "رسوم أنشطة",
                            "أخرى"
                        };

                        if (childPlans.Count == 0)
                        {
                            childPlans =
                                CreateDefaultChildPlans(
                                    con,
                                    transaction,
                                    plan,
                                    defaultTypes);
                        }

                        if (childPlans.Count == 0)
                            throw new InvalidOperationException(
                                "لا توجد أنواع رسوم فرعية قابلة للتوزيع للصف والعام المحددين.");

                        decimal oldChildTotal = 0m;

                        foreach (Tuple<int, string, decimal> child in childPlans)
                            oldChildTotal += Math.Max(0m, child.Item3);

                        decimal[] weights =
                            GetDefaultDistributionWeights(
                                childPlans.Count);

                        decimal weightTotal = 0m;

                        foreach (decimal weight in weights)
                            weightTotal += weight;

                        if (weightTotal <= 0m)
                            throw new InvalidOperationException(
                                "تعذر تحديد أوزان توزيع الرسوم.");

                        decimal allocated = 0m;

                        for (int i = 0; i < childPlans.Count; i++)
                        {
                            Tuple<int, string, decimal> child =
                                childPlans[i];

                            decimal ratio =
                                oldChildTotal > 0m
                                    ? Math.Max(0m, child.Item3) / oldChildTotal
                                    : weights[i] / weightTotal;

                            decimal amount =
                                i == childPlans.Count - 1
                                    ? plan.Amount - allocated
                                    : Math.Round(
                                        plan.Amount * ratio,
                                        2,
                                        MidpointRounding.AwayFromZero);

                            if (amount < 0m)
                                amount = 0m;

                            allocated += amount;

                            UpdatePlanAmountAndUnpaidFees(
                                con,
                                transaction,
                                child.Item1,
                                amount,
                                plan.DueDate,
                                plan.Notes);
                        }

                        UpdatePlanAmountAndUnpaidFees(
                            con,
                            transaction,
                            totalPlanId,
                            plan.Amount,
                            plan.DueDate,
                            plan.Notes);

                        transaction.Commit();
                        return childPlans.Count;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // تحديث الرسوم الكلية ثم توزيعها على بنود الصف نفسه فقط.
        public int UpdateTotalFeePlanAndDistribute(FeePlan plan)
        {
            return DistributeTotalFee(plan);
        }

        // تعديل بند فرعي ثم إعادة احتساب الرسوم الكلية من مجموع البنود.
        public bool UpdateChildFeePlanAndRecalculateTotal(FeePlan plan)
        {
            if (plan == null || plan.FeePlanID <= 0)
                throw new ArgumentException("خطة الرسوم غير صالحة.", "plan");

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                using (SqlTransaction transaction =
                    con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        const string updateChildSql = @"
                            UPDATE FeePlans
                            SET Amount = @Amount,
                                DueDate = @DueDate,
                                IsRequired = @IsRequired,
                                Notes = @Notes
                            WHERE FeePlanID = @FeePlanID
                              AND ClassID = @ClassID
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND LTRIM(RTRIM(ISNULL(FeeType, N''))) =
                                  LTRIM(RTRIM(@FeeType));";

                        using (SqlCommand updateChild =
                            new SqlCommand(
                                updateChildSql,
                                con,
                                transaction))
                        {
                            AddParameters(updateChild, plan);
                            updateChild.Parameters.AddWithValue(
                                "@FeePlanID",
                                plan.FeePlanID);

                            if (updateChild.ExecuteNonQuery() == 0)
                                return false;
                        }

                        UpdatePlanAmountAndUnpaidFees(
                            con,
                            transaction,
                            plan.FeePlanID,
                            plan.Amount,
                            plan.DueDate,
                            plan.Notes);

                        List<Tuple<int, string, decimal>> childPlans =
                            LoadChildPlans(
                                con,
                                transaction,
                                plan.ClassID,
                                plan.AcademicYear);

                        decimal totalAmount = 0m;

                        foreach (Tuple<int, string, decimal> child in childPlans)
                            totalAmount += Math.Max(0m, child.Item3);

                        int totalPlanId =
                            EnsureTotalPlan(
                                con,
                                transaction,
                                new FeePlan
                                {
                                    AcademicYear = plan.AcademicYear,
                                    ClassID = plan.ClassID,
                                    FeeType = "الرسوم الكلية",
                                    Amount = totalAmount,
                                    DueDate = plan.DueDate,
                                    IsRequired = true,
                                    Notes = plan.Notes
                                });

                        UpdatePlanAmountAndUnpaidFees(
                            con,
                            transaction,
                            totalPlanId,
                            totalAmount,
                            plan.DueDate,
                            plan.Notes);

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private int EnsureTotalPlan(
            SqlConnection con,
            SqlTransaction transaction,
            FeePlan plan)
        {
            const string findSql = @"
                SELECT TOP (1) FeePlanID
                FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                WHERE ClassID = @ClassID
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                      REPLACE(@AcademicYear, N'-', N'/')
                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                ORDER BY CASE
                    WHEN LTRIM(RTRIM(ISNULL(FeeType, N''))) = N'الرسوم الكلية' THEN 0
                    WHEN LTRIM(RTRIM(ISNULL(FeeType, N''))) = N'رسوم كلية' THEN 1
                    ELSE 2
                END, FeePlanID;";

            int existingId = 0;

            using (SqlCommand find =
                new SqlCommand(
                    findSql,
                    con,
                    transaction))
            {
                find.Parameters.AddWithValue("@ClassID", plan.ClassID);
                find.Parameters.AddWithValue(
                    "@AcademicYear",
                    (plan.AcademicYear ?? string.Empty).Trim());

                object value = find.ExecuteScalar();

                if (value != null && value != DBNull.Value)
                    existingId = Convert.ToInt32(value);
            }

            if (existingId > 0)
            {
                const string updateSql = @"
                    UPDATE FeePlans
                    SET FeeType = N'الرسوم الكلية',
                        Amount = @Amount,
                        DueDate = @DueDate,
                        IsRequired = @IsRequired,
                        Notes = @Notes
                    WHERE FeePlanID = @FeePlanID;";

                using (SqlCommand update =
                    new SqlCommand(
                        updateSql,
                        con,
                        transaction))
                {
                    update.Parameters.AddWithValue("@FeePlanID", existingId);
                    update.Parameters.AddWithValue("@Amount", plan.Amount);
                    update.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                    update.Parameters.AddWithValue("@IsRequired", plan.IsRequired);
                    update.Parameters.AddWithValue(
                        "@Notes",
                        string.IsNullOrWhiteSpace(plan.Notes)
                            ? (object)DBNull.Value
                            : plan.Notes);

                    update.ExecuteNonQuery();
                }

                return existingId;
            }

            const string insertSql = @"
                INSERT INTO FeePlans
                (
                    AcademicYear,
                    ClassID,
                    FeeType,
                    Amount,
                    DueDate,
                    IsRequired,
                    Notes
                )
                VALUES
                (
                    @AcademicYear,
                    @ClassID,
                    N'الرسوم الكلية',
                    @Amount,
                    @DueDate,
                    @IsRequired,
                    @Notes
                );

                SELECT CONVERT(INT, SCOPE_IDENTITY());";

            using (SqlCommand insert =
                new SqlCommand(
                    insertSql,
                    con,
                    transaction))
            {
                insert.Parameters.AddWithValue(
                    "@AcademicYear",
                    (plan.AcademicYear ?? string.Empty).Trim());
                insert.Parameters.AddWithValue("@ClassID", plan.ClassID);
                insert.Parameters.AddWithValue("@Amount", plan.Amount);
                insert.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                insert.Parameters.AddWithValue("@IsRequired", plan.IsRequired);
                insert.Parameters.AddWithValue(
                    "@Notes",
                    string.IsNullOrWhiteSpace(plan.Notes)
                        ? (object)DBNull.Value
                        : plan.Notes);

                return Convert.ToInt32(insert.ExecuteScalar());
            }
        }

        private List<Tuple<int, string, decimal>> LoadChildPlans(
            SqlConnection con,
            SqlTransaction transaction,
            int classId,
            string academicYear)
        {
            const string sql = @"
                SELECT
                    FeePlanID,
                    LTRIM(RTRIM(FeeType)) AS FeeType,
                    ISNULL(Amount, 0) AS Amount
                FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                WHERE ClassID = @ClassID
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                      REPLACE(@AcademicYear, N'-', N'/')
                  AND NULLIF(LTRIM(RTRIM(FeeType)), N'') IS NOT NULL
                  AND LTRIM(RTRIM(FeeType)) NOT IN
                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                ORDER BY
                    CASE LTRIM(RTRIM(FeeType))
                        WHEN N'رسوم تسجيل' THEN 1
                        WHEN N'رسوم دراسية' THEN 2
                        WHEN N'رسوم كتب' THEN 3
                        WHEN N'رسوم زي مدرسي' THEN 4
                        WHEN N'رسوم مواصلات' THEN 5
                        WHEN N'رسوم امتحانات' THEN 6
                        WHEN N'رسوم أنشطة' THEN 7
                        WHEN N'أخرى' THEN 8
                        ELSE 99
                    END,
                    FeePlanID;";

            var result = new List<Tuple<int, string, decimal>>();

            using (SqlCommand command =
                new SqlCommand(
                    sql,
                    con,
                    transaction))
            {
                command.Parameters.AddWithValue("@ClassID", classId);
                command.Parameters.AddWithValue(
                    "@AcademicYear",
                    (academicYear ?? string.Empty).Trim());

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                            Tuple.Create(
                                Convert.ToInt32(reader["FeePlanID"]),
                                Convert.ToString(reader["FeeType"]),
                                reader["Amount"] == DBNull.Value
                                    ? 0m
                                    : Convert.ToDecimal(reader["Amount"])));
                    }
                }
            }

            return result;
        }

        private List<Tuple<int, string, decimal>> CreateDefaultChildPlans(
            SqlConnection con,
            SqlTransaction transaction,
            FeePlan plan,
            string[] types)
        {
            decimal[] weights =
            {
                15m, 40m, 10m, 8m, 8m, 10m, 4m, 5m
            };

            var created = new List<Tuple<int, string, decimal>>();

            for (int i = 0; i < types.Length; i++)
            {
                const string sql = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE ClassID = @ClassID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                              REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) = @FeeType
                    )
                    BEGIN
                        INSERT INTO FeePlans
                        (
                            AcademicYear,
                            ClassID,
                            FeeType,
                            Amount,
                            DueDate,
                            IsRequired,
                            Notes
                        )
                        VALUES
                        (
                            @AcademicYear,
                            @ClassID,
                            @FeeType,
                            0,
                            @DueDate,
                            1,
                            @Notes
                        );
                    END";

                using (SqlCommand command =
                    new SqlCommand(
                        sql,
                        con,
                        transaction))
                {
                    command.Parameters.AddWithValue(
                        "@AcademicYear",
                        (plan.AcademicYear ?? string.Empty).Trim());
                    command.Parameters.AddWithValue("@ClassID", plan.ClassID);
                    command.Parameters.AddWithValue("@FeeType", types[i]);
                    command.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                    command.Parameters.AddWithValue(
                        "@Notes",
                        string.IsNullOrWhiteSpace(plan.Notes)
                            ? (object)DBNull.Value
                            : plan.Notes);
                    command.ExecuteNonQuery();
                }
            }

            return LoadChildPlans(
                con,
                transaction,
                plan.ClassID,
                plan.AcademicYear);
        }

        private decimal[] GetDefaultDistributionWeights(int count)
        {
            decimal[] defaults =
            {
                15m, 40m, 10m, 8m, 8m, 10m, 4m, 5m
            };

            if (count == defaults.Length)
                return defaults;

            decimal[] result = new decimal[count];

            for (int i = 0; i < count; i++)
                result[i] = 1m;

            return result;
        }

        private void UpdatePlanAmountAndUnpaidFees(
            SqlConnection con,
            SqlTransaction transaction,
            int feePlanId,
            decimal amount,
            DateTime dueDate,
            string notes)
        {
            const string sql = @"
                UPDATE FeePlans
                SET Amount = @Amount,
                    DueDate = @DueDate,
                    Notes = @Notes
                WHERE FeePlanID = @FeePlanID;

                UPDATE f
                SET TotalAmount = @Amount,
                    NetAmount =
                        CASE
                            WHEN ISNULL(f.DiscountAmount, 0) > @Amount THEN 0
                            ELSE @Amount - ISNULL(f.DiscountAmount, 0)
                        END,
                    RemainingAmount =
                        CASE
                            WHEN ISNULL(f.DiscountAmount, 0) > @Amount THEN 0
                            ELSE @Amount - ISNULL(f.DiscountAmount, 0) - ISNULL(f.PaidAmount, 0)
                        END,
                    DueDate = @DueDate,
                    UpdatedAt = GETDATE()
                FROM Fees f
                WHERE f.FeePlanID = @FeePlanID
                  AND ISNULL(f.PaidAmount, 0) = 0;";

            using (SqlCommand command =
                new SqlCommand(
                    sql,
                    con,
                    transaction))
            {
                command.Parameters.AddWithValue("@FeePlanID", feePlanId);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@DueDate", dueDate.Date);
                command.Parameters.AddWithValue(
                    "@Notes",
                    string.IsNullOrWhiteSpace(notes)
                        ? (object)DBNull.Value
                        : notes);

                command.ExecuteNonQuery();

                /* مزامنة الطلاب الحاليين في الصف: إذا أُنشئت الخطة قبل قبول/توزيع الطالب،
                   ننشئ له بند الرسوم غير المسدد تلقائيًا. الرسوم الكلية تبقى خطة مرجعية. */
                const string syncStudentsSql = @"
                    DECLARE @ClassID INT;
                    DECLARE @AcademicYear NVARCHAR(100);
                    DECLARE @FeeType NVARCHAR(100);

                    SELECT
                        @ClassID = ClassID,
                        @AcademicYear = AcademicYear,
                        @FeeType = LTRIM(RTRIM(FeeType))
                    FROM FeePlans
                    WHERE FeePlanID = @FeePlanID;

                    IF LTRIM(RTRIM(ISNULL(@FeeType, N''))) NOT IN
                    (
                        N'الرسوم الكلية',
                        N'رسوم كلية',
                        N'الرسوم الكليه',
                        N'رسوم كليه',
                        N'Total Fee'
                    )
                    BEGIN
                        ;WITH CurrentStudents AS
                        (
                            SELECT DISTINCT e.StudentID
                            FROM Enrollments e
                            WHERE e.ClassID = @ClassID
                              AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN
                                  (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')

                            UNION

                            SELECT DISTINCT sc.StudentID
                            FROM StudentClasses sc
                            WHERE sc.ClassID = @ClassID
                              AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                        )
                        INSERT INTO Fees
                        (
                            StudentID,
                            FeePlanID,
                            AcademicYear,
                            FeeType,
                            TotalAmount,
                            DiscountAmount,
                            NetAmount,
                            PaidAmount,
                            RemainingAmount,
                            DueDate,
                            PaymentDate,
                            PaymentMethod,
                            ReceiptNumber,
                            Status,
                            Notes
                        )
                        SELECT
                            cs.StudentID,
                            fp.FeePlanID,
                            fp.AcademicYear,
                            fp.FeeType,
                            fp.Amount,
                            0,
                            fp.Amount,
                            0,
                            fp.Amount,
                            fp.DueDate,
                            NULL,
                            NULL,
                            NULL,
                            N'غير مسدد',
                            fp.Notes
                        FROM CurrentStudents cs
                        INNER JOIN FeePlans fp ON fp.FeePlanID = @FeePlanID
                        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM Fees existing
                            WHERE existing.StudentID = cs.StudentID
                              AND existing.FeePlanID = @FeePlanID
                        );
                    END
            }
        }

        // تعريف الدالة UpdateFeePlan لتنفيذ العملية المرتبطة بها.
        public bool UpdateFeePlan(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

                    IF EXISTS
                    (
                        SELECT 1 FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND ClassID = @ClassID
                          AND FeeType = @FeeType
                          AND FeePlanID <> @FeePlanID
                    )
                        THROW 51021, N'توجد خطة رسوم مكررة لنفس العام والصف ونوع الرسوم.', 1;

                    IF EXISTS
                    (
                        SELECT 1 FROM Fees f
                        INNER JOIN FeePlans oldPlan ON oldPlan.FeePlanID = f.FeePlanID
                        WHERE f.FeePlanID = @FeePlanID
                          AND (
                              REPLACE(ISNULL(oldPlan.AcademicYear, N''), N'-', N'/') <> REPLACE(@AcademicYear, N'-', N'/')
                              OR oldPlan.ClassID <> @ClassID
                              OR oldPlan.FeeType <> @FeeType
                          )
                    )
                        THROW 51022, N'لا يمكن تغيير صف أو عام أو نوع خطة مستخدمة في رسوم موجودة. أنشئ خطة جديدة بدلاً من تغيير هوية الخطة.', 1;

                    UPDATE FeePlans SET
                        AcademicYear = @AcademicYear,
                        ClassID = @ClassID,
                        FeeType = @FeeType,
                        Amount = @Amount,
                        DueDate = @DueDate,
                        IsRequired = @IsRequired,
                        Notes = @Notes
                    WHERE FeePlanID = @FeePlanID;

                    /* تحديث البنود غير المسددة فقط؛ السجلات المسددة تبقى تاريخية. */
                    UPDATE f SET
                        TotalAmount = @Amount,
                        NetAmount = CASE WHEN f.DiscountAmount > @Amount THEN 0 ELSE @Amount - f.DiscountAmount END,
                        RemainingAmount = CASE WHEN f.DiscountAmount > @Amount THEN 0 ELSE @Amount - f.DiscountAmount - f.PaidAmount END,
                        DueDate = @DueDate,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    FROM Fees f
                    WHERE f.FeePlanID = @FeePlanID
                      AND f.PaidAmount = 0;

                    COMMIT TRANSACTION";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeePlanID", plan.FeePlanID);

                    AddParameters(cmd, plan);


                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // حذف خطة رسوم مع حماية السجلات المدفوعة وإبقاء الإجمالي متزامنًا.
        public bool DeleteFeePlan(int feePlanId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                using (SqlTransaction transaction =
                    con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        const string findSql = @"
                            SELECT
                                FeePlanID,
                                ClassID,
                                AcademicYear,
                                FeeType
                            FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                            WHERE FeePlanID = @FeePlanID;";

                        int classId;
                        string academicYear;
                        string feeType;

                        using (SqlCommand find =
                            new SqlCommand(
                                findSql,
                                con,
                                transaction))
                        {
                            find.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);

                            using (SqlDataReader reader =
                                find.ExecuteReader())
                            {
                                if (!reader.Read())
                                    return false;

                                classId =
                                    Convert.ToInt32(reader["ClassID"]);
                                academicYear =
                                    Convert.ToString(
                                        reader["AcademicYear"]);
                                feeType =
                                    Convert.ToString(
                                        reader["FeeType"]);
                            }
                        }

                        bool isTotal =
                            IsTotalFeeType(feeType);

                        const string paidSql = @"
                            SELECT COUNT(1)
                            FROM Fees f
                            INNER JOIN FeePlans fp
                                ON fp.FeePlanID = f.FeePlanID
                            WHERE fp.ClassID = @ClassID
                              AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND ISNULL(f.PaidAmount, 0) > 0
                              AND
                              (
                                  @IsTotal = 1
                                  OR fp.FeePlanID = @FeePlanID
                              );";

                        using (SqlCommand paid =
                            new SqlCommand(
                                paidSql,
                                con,
                                transaction))
                        {
                            paid.Parameters.AddWithValue("@ClassID", classId);
                            paid.Parameters.AddWithValue(
                                "@AcademicYear",
                                academicYear ?? string.Empty);
                            paid.Parameters.AddWithValue(
                                "@IsTotal",
                                isTotal);
                            paid.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);

                            if (Convert.ToInt32(
                                    paid.ExecuteScalar()) > 0)
                            {
                                throw new InvalidOperationException(
                                    "لا يمكن حذف خطة رسوم تحتوي على سداد سابق. استخدم الاسترجاع أو الإلغاء المالي للحفاظ على السجل التاريخي.");
                            }
                        }

                        if (isTotal)
                        {
                            const string deleteFamilySql = @"
                                DELETE FROM Fees
                                WHERE FeePlanID IN
                                (
                                    SELECT FeePlanID
                                    FROM FeePlans
                                    WHERE ClassID = @ClassID
                                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                          REPLACE(@AcademicYear, N'-', N'/')
                                );

                                DELETE FROM FeePlans
                                WHERE ClassID = @ClassID
                                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                      REPLACE(@AcademicYear, N'-', N'/');";

                            using (SqlCommand deleteFamily =
                                new SqlCommand(
                                    deleteFamilySql,
                                    con,
                                    transaction))
                            {
                                deleteFamily.Parameters.AddWithValue("@ClassID", classId);
                                deleteFamily.Parameters.AddWithValue(
                                    "@AcademicYear",
                                    academicYear ?? string.Empty);

                                int affected =
                                    deleteFamily.ExecuteNonQuery();

                                transaction.Commit();
                                return affected > 0;
                            }
                        }

                        const string deleteChildSql = @"
                            DELETE FROM Fees
                            WHERE FeePlanID = @FeePlanID;

                            DELETE FROM FeePlans
                            WHERE FeePlanID = @FeePlanID;

                            DECLARE @RemainingTotal DECIMAL(18,2);

                            SELECT @RemainingTotal =
                                ISNULL(SUM(Amount), 0)
                            FROM FeePlans
                            WHERE ClassID = @ClassID
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                                  (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');

                            UPDATE FeePlans
                            SET Amount = @RemainingTotal,
                                UpdatedAt = GETDATE()
                            WHERE ClassID = @ClassID
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                                  (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');";

                        using (SqlCommand deleteChild =
                            new SqlCommand(
                                deleteChildSql,
                                con,
                                transaction))
                        {
                            deleteChild.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);
                            deleteChild.Parameters.AddWithValue(
                                "@ClassID",
                                classId);
                            deleteChild.Parameters.AddWithValue(
                                "@AcademicYear",
                                academicYear ?? string.Empty);

                            int affected =
                                deleteChild.ExecuteNonQuery();

                            transaction.Commit();
                            return affected > 0;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, FeePlan plan)
        {

            cmd.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? "");

            cmd.Parameters.AddWithValue("@ClassID", plan.ClassID);

            cmd.Parameters.AddWithValue("@FeeType", plan.FeeType ?? "");

            cmd.Parameters.AddWithValue("@Amount", plan.Amount);

            cmd.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);

            cmd.Parameters.AddWithValue("@IsRequired", plan.IsRequired);


            cmd.Parameters.AddWithValue(

                "@Notes",
                string.IsNullOrWhiteSpace(plan.Notes)
                    ? (object)DBNull.Value
                    : plan.Notes
            );
        }
    }
}
