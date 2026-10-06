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
                        CASE
                            WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                                (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                            THEN N'الرسوم الكلية'
                            ELSE LTRIM(RTRIM(ISNULL(fp.FeeType, N'')))
                        END AS FeeType,
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
                    SELECT
                        FeeTypeID,
                        LTRIM(RTRIM(FeeTypeName)) AS FeeType
                    FROM dbo.FeeTypes
                    WHERE ISNULL(IsActive, 1) = 1
                      AND NULLIF(LTRIM(RTRIM(FeeTypeName)), N'') IS NOT NULL
                    ORDER BY SortOrder, FeeTypeID;";

                using (SqlDataAdapter da =
                    new SqlDataAdapter(query, con))
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

                    IF NULLIF(LTRIM(RTRIM(@FeeType)), N'') IS NULL
                        THROW 51021, N'نوع الرسوم لا يمكن أن يكون فارغًا.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.FeeTypes WITH (UPDLOCK, HOLDLOCK)
                        WHERE LTRIM(RTRIM(FeeTypeName)) = LTRIM(RTRIM(@FeeType))
                    )
                    BEGIN
                        INSERT INTO dbo.FeeTypes
                        (
                            FeeTypeCode,
                            FeeTypeName,
                            IsTotal,
                            IsRequiredDefault,
                            DistributionWeight,
                            SortOrder,
                            IsActive,
                            Notes,
                            CreatedAt
                        )
                        VALUES
                        (
                            N'CUSTOM-' + RIGHT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 20),
                            LTRIM(RTRIM(@FeeType)),
                            0,
                            @IsRequired,
                            1,
                            1000,
                            1,
                            N'نوع رسوم أضيف من شاشة خطط الرسوم',
                            SYSUTCDATETIME()
                        );
                    END;

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
                                        Notes = @Notes,
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
                                    DECLARE @CreatedTotalPlanID INT;

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

                                    SET @CreatedTotalPlanID = CONVERT(INT, SCOPE_IDENTITY());

                                    INSERT INTO Fees
                                    (
                                        StudentID, FeePlanID, AcademicYear, FeeType,
                                        TotalAmount, DiscountAmount, NetAmount,
                                        PaidAmount, RemainingAmount, DueDate,
                                        PaymentDate, PaymentMethod, ReceiptNumber,
                                        Status, Notes
                                    )
                                    SELECT DISTINCT
                                        source.StudentID,
                                        @CreatedTotalPlanID,
                                        @AcademicYear,
                                        N'الرسوم الكلية',
                                        @TotalAmount,
                                        0,
                                        @TotalAmount,
                                        0,
                                        @TotalAmount,
                                        @DueDate,
                                        NULL,
                                        NULL,
                                        NULL,
                                        N'غير مسدد',
                                        @Notes
                                    FROM
                                    (
                                        SELECT sc.StudentID
                                        FROM StudentClasses sc
                                        INNER JOIN Enrollments e ON e.StudentID = sc.StudentID
                                           AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') =
                                               REPLACE(@AcademicYear, N'-', N'/')
                                           AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN
                                               (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                                        WHERE sc.ClassID = @ClassID
                                          AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') =
                                              REPLACE(@AcademicYear, N'-', N'/')
                                        UNION
                                        SELECT e.StudentID
                                        FROM Enrollments e
                                        WHERE e.ClassID = @ClassID
                                          AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') =
                                              REPLACE(@AcademicYear, N'-', N'/')
                                          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN
                                              (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                                    ) source;
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

                        if (childPlans.Count == 0)
                        {
                            childPlans =
                                CreateDefaultChildPlans(
                                    con,
                                    transaction,
                                    plan);
                        }

                        if (childPlans.Count == 0)
                            throw new InvalidOperationException(
                                "لا توجد أنواع رسوم فرعية قابلة للتوزيع للصف والعام المحددين.");

                        decimal oldChildTotal = 0m;

                        foreach (Tuple<int, string, decimal> child in childPlans)
                            oldChildTotal += Math.Max(0m, child.Item3);

                        decimal[] weights =
                            GetDefaultDistributionWeights(
                                con,
                                transaction,
                                childPlans);

                        decimal weightTotal = 0m;

                        foreach (decimal weight in weights)
                            weightTotal += weight;

                        if (weightTotal <= 0m)
                            throw new InvalidOperationException(
                                "تعذر تحديد أوزان توزيع الرسوم.");

                        // الحدود الدنيا هي أكبر مبلغ سبق تحصيله من أي طالب
                        // لكل نوع رسوم. نضمن عدم توزيع مبلغ أقل من المبلغ المحصّل.
                        decimal[] minimumPaidAmounts =
                            new decimal[childPlans.Count];

                        decimal minimumRequiredTotal = 0m;

                        for (int i = 0; i < childPlans.Count; i++)
                        {
                            minimumPaidAmounts[i] =
                                GetMaxPaidAmountForPlan(
                                    con,
                                    transaction,
                                    childPlans[i].Item1);

                            minimumRequiredTotal +=
                                minimumPaidAmounts[i];
                        }

                        if (plan.Amount + 0.01m <
                            minimumRequiredTotal)
                        {
                            throw new InvalidOperationException(
                                "الرسوم الكلية المطلوبة أقل من مجموع المبالغ المحصّلة سابقًا لبعض أنواع الرسوم. ارفع الرسوم الكلية أو استخدم الإجراءات المالية لتصحيح الحركة.");
                        }

                        decimal[] ratios =
                            new decimal[childPlans.Count];

                        for (int i = 0; i < childPlans.Count; i++)
                        {
                            ratios[i] =
                                oldChildTotal > 0m
                                    ? Math.Max(0m, childPlans[i].Item3) / oldChildTotal
                                    : weights[i] / weightTotal;
                        }

                        decimal distributableAmount =
                            plan.Amount -
                            minimumRequiredTotal;

                        decimal allocatedExtra = 0m;

                        for (int i = 0; i < childPlans.Count; i++)
                        {
                            Tuple<int, string, decimal> child =
                                childPlans[i];

                            decimal extra =
                                i == childPlans.Count - 1
                                    ? distributableAmount - allocatedExtra
                                    : Math.Round(
                                        distributableAmount * ratios[i],
                                        2,
                                        MidpointRounding.AwayFromZero);

                            decimal amount =
                                minimumPaidAmounts[i] +
                                Math.Max(0m, extra);

                            if (i != childPlans.Count - 1)
                                allocatedExtra += Math.Max(0m, extra);

                            UpdatePlanAmountAndUnpaidFees(
                                con,
                                transaction,
                                child.Item1,
                                amount,
                                plan.DueDate,
                                plan.Notes);

                            EnsureStudentFeesForPlan(
                                con,
                                transaction,
                                child.Item1,
                                plan.ClassID,
                                plan.AcademicYear,
                                child.Item2,
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

                        EnsureStudentFeesForPlan(
                            con,
                            transaction,
                            totalPlanId,
                            plan.ClassID,
                            plan.AcademicYear,
                            "الرسوم الكلية",
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

                        EnsureStudentFeesForPlan(
                            con,
                            transaction,
                            plan.FeePlanID,
                            plan.ClassID,
                            plan.AcademicYear,
                            plan.FeeType,
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

                        EnsureStudentFeesForPlan(
                            con,
                            transaction,
                            totalPlanId,
                            plan.ClassID,
                            plan.AcademicYear,
                            "الرسوم الكلية",
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

        private decimal GetMaxPaidAmountForPlan(
            SqlConnection con,
            SqlTransaction transaction,
            int feePlanId)
        {
            const string sql = @"
                SELECT ISNULL(MAX(PaidAmount), 0)
                FROM Fees WITH (UPDLOCK, HOLDLOCK)
                WHERE FeePlanID = @FeePlanID;";

            using (SqlCommand command =
                new SqlCommand(sql, con, transaction))
            {
                command.Parameters.AddWithValue(
                    "@FeePlanID",
                    feePlanId);

                object value = command.ExecuteScalar();

                return value == null || value == DBNull.Value
                    ? 0m
                    : Convert.ToDecimal(value);
            }
        }

        private void EnsurePlanAmountCoversPaid(
            SqlConnection con,
            SqlTransaction transaction,
            int feePlanId,
            decimal newAmount,
            string feeType)
        {
            decimal maxPaid =
                GetMaxPaidAmountForPlan(
                    con,
                    transaction,
                    feePlanId);

            if (newAmount + 0.01m < maxPaid)
            {
                throw new InvalidOperationException(
                    "لا يمكن تخفيض " +
                    (feeType ?? "الرسوم") +
                    " إلى " +
                    newAmount.ToString("N2") +
                    " ريال لأن هناك طالبًا سبق أن دفع " +
                    maxPaid.ToString("N2") +
                    " ريال لهذا البند.");
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
                    fp.FeePlanID,
                    LTRIM(RTRIM(fp.FeeType)) AS FeeType,
                    ISNULL(fp.Amount, 0) AS Amount
                FROM dbo.FeePlans fp WITH (UPDLOCK, HOLDLOCK)
                LEFT JOIN dbo.FeeTypes ft
                    ON LTRIM(RTRIM(ft.FeeTypeName)) = LTRIM(RTRIM(fp.FeeType))
                WHERE fp.ClassID = @ClassID
                  AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') =
                      REPLACE(@AcademicYear, N'-', N'/')
                  AND NULLIF(LTRIM(RTRIM(fp.FeeType)), N'') IS NOT NULL
                  AND LTRIM(RTRIM(fp.FeeType)) NOT IN
                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                ORDER BY
                    CASE
                        WHEN ft.SortOrder IS NULL THEN 100000
                        ELSE ft.SortOrder
                    END,
                    fp.FeePlanID;";

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
            FeePlan plan)
        {
            var definitions = new List<Tuple<string, decimal, bool>>();

            const string definitionSql = @"
                SELECT
                    LTRIM(RTRIM(FeeTypeName)) AS FeeTypeName,
                    ISNULL(DistributionWeight, 1) AS DistributionWeight,
                    ISNULL(IsRequiredDefault, 1) AS IsRequiredDefault
                FROM dbo.FeeTypes WITH (UPDLOCK, HOLDLOCK)
                WHERE ISNULL(IsActive, 1) = 1
                  AND ISNULL(IsTotal, 0) = 0
                  AND NULLIF(LTRIM(RTRIM(FeeTypeName)), N'') IS NOT NULL
                ORDER BY SortOrder, FeeTypeID;";

            using (SqlCommand definitionCommand =
                new SqlCommand(definitionSql, con, transaction))
            using (SqlDataReader reader = definitionCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    definitions.Add(
                        Tuple.Create(
                            Convert.ToString(reader["FeeTypeName"]),
                            reader["DistributionWeight"] == DBNull.Value
                                ? 1m
                                : Convert.ToDecimal(reader["DistributionWeight"]),
                            reader["IsRequiredDefault"] == DBNull.Value ||
                            Convert.ToBoolean(reader["IsRequiredDefault"])));
                }
            }

            foreach (Tuple<string, decimal, bool> definition in definitions)
            {
                const string insertSql = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE ClassID = @ClassID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                              REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) =
                              @FeeType
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
                            @IsRequired,
                            @Notes
                        );
                    END;";

                using (SqlCommand command =
                    new SqlCommand(insertSql, con, transaction))
                {
                    command.Parameters.AddWithValue(
                        "@AcademicYear",
                        (plan.AcademicYear ?? string.Empty).Trim());
                    command.Parameters.AddWithValue("@ClassID", plan.ClassID);
                    command.Parameters.AddWithValue("@FeeType", definition.Item1);
                    command.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                    command.Parameters.AddWithValue("@IsRequired", definition.Item3);
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

        private decimal[] GetDefaultDistributionWeights(
            SqlConnection con,
            SqlTransaction transaction,
            List<Tuple<int, string, decimal>> childPlans)
        {
            var weightByType =
                new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            const string sql = @"
                SELECT
                    LTRIM(RTRIM(FeeTypeName)) AS FeeTypeName,
                    ISNULL(DistributionWeight, 1) AS DistributionWeight
                FROM dbo.FeeTypes
                WHERE ISNULL(IsActive, 1) = 1;";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string feeType =
                        Convert.ToString(reader["FeeTypeName"]).Trim();

                    if (string.IsNullOrWhiteSpace(feeType))
                        continue;

                    decimal weight =
                        reader["DistributionWeight"] == DBNull.Value
                            ? 1m
                            : Convert.ToDecimal(reader["DistributionWeight"]);

                    weightByType[feeType] = Math.Max(0m, weight);
                }
            }

            decimal[] result = new decimal[childPlans.Count];

            for (int i = 0; i < childPlans.Count; i++)
            {
                string feeType =
                    (childPlans[i].Item2 ?? string.Empty).Trim();

                decimal weight;
                if (!weightByType.TryGetValue(feeType, out weight))
                    weight = 1m;

                result[i] = Math.Max(0m, weight);
            }

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
            EnsurePlanAmountCoversPaid(
                con,
                transaction,
                feePlanId,
                amount,
                null);

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
                    END";

                using (SqlCommand syncStudents =
                    new SqlCommand(
                        syncStudentsSql,
                        con,
                        transaction))
                {
                    syncStudents.Parameters.AddWithValue(
                        "@FeePlanID",
                        feePlanId);

                    syncStudents.ExecuteNonQuery();
                }
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
                        int classId = 0;
                        string academicYear = string.Empty;
                        string feeType = string.Empty;

                        const string readSql = @"
                            SELECT
                                ClassID,
                                AcademicYear,
                                LTRIM(RTRIM(ISNULL(FeeType, N''))) AS FeeType
                            FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                            WHERE FeePlanID = @FeePlanID;";

                        using (SqlCommand read =
                            new SqlCommand(
                                readSql,
                                con,
                                transaction))
                        {
                            read.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);

                            using (SqlDataReader reader = read.ExecuteReader())
                            {
                                if (!reader.Read())
                                    return false;

                                classId =
                                    Convert.ToInt32(reader["ClassID"]);

                                academicYear =
                                    Convert.ToString(reader["AcademicYear"])
                                        .Trim();

                                feeType =
                                    Convert.ToString(reader["FeeType"])
                                        .Trim();
                            }
                        }

                        bool isTotal =
                            IsTotalFeeType(feeType);

                        const string paidCheckSql = @"
                            SELECT COUNT(1)
                            FROM Fees WITH (UPDLOCK, HOLDLOCK)
                            WHERE FeePlanID = @FeePlanID
                              AND ISNULL(PaidAmount, 0) > 0;";

                        using (SqlCommand paidCheck =
                            new SqlCommand(
                                paidCheckSql,
                                con,
                                transaction))
                        {
                            paidCheck.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);

                            int paidRows =
                                Convert.ToInt32(
                                    paidCheck.ExecuteScalar());

                            if (paidRows > 0)
                            {
                                throw new InvalidOperationException(
                                    "لا يمكن حذف خطة رسوم لها دفعات محصلة. استخدم الإجراءات المالية/السند العكسي للحفاظ على السجل التاريخي.");
                            }
                        }

                        if (isTotal)
                        {
                            const string childExistsSql = @"
                                SELECT COUNT(1)
                                FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                                WHERE ClassID = @ClassID
                                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                      REPLACE(@AcademicYear, N'-', N'/')
                                  AND NULLIF(LTRIM(RTRIM(ISNULL(FeeType, N''))), N'') IS NOT NULL
                                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');";

                            using (SqlCommand childCheck =
                                new SqlCommand(
                                    childExistsSql,
                                    con,
                                    transaction))
                            {
                                childCheck.Parameters.AddWithValue("@ClassID", classId);
                                childCheck.Parameters.AddWithValue("@AcademicYear", academicYear);

                                int childCount =
                                    Convert.ToInt32(
                                        childCheck.ExecuteScalar());

                                if (childCount > 0)
                                {
                                    throw new InvalidOperationException(
                                        "لا يمكن حذف الرسوم الكلية بينما توجد أنواع رسوم فرعية للصف نفسه. عدّل الرسوم الكلية أو الأنواع الفرعية بدل حذف المرجع الرئيسي.");
                                }
                            }
                        }

                        const string deleteFeesSql = @"
                            DELETE FROM Fees
                            WHERE FeePlanID = @FeePlanID;";

                        using (SqlCommand deleteFees =
                            new SqlCommand(
                                deleteFeesSql,
                                con,
                                transaction))
                        {
                            deleteFees.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);
                            deleteFees.ExecuteNonQuery();
                        }

                        const string deletePlanSql = @"
                            DELETE FROM FeePlans
                            WHERE FeePlanID = @FeePlanID;";

                        using (SqlCommand deletePlan =
                            new SqlCommand(
                                deletePlanSql,
                                con,
                                transaction))
                        {
                            deletePlan.Parameters.AddWithValue(
                                "@FeePlanID",
                                feePlanId);

                            if (deletePlan.ExecuteNonQuery() == 0)
                                return false;
                        }

                        if (!isTotal)
                        {
                            // بعد حذف نوع فرعي، الكلي يصبح مجموع الأنواع
                            // المتبقية للصف والعام نفسه.
                            decimal totalAmount = 0m;

                            const string sumSql = @"
                                SELECT ISNULL(SUM(Amount), 0)
                                FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                                WHERE ClassID = @ClassID
                                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                                      REPLACE(@AcademicYear, N'-', N'/')
                                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');";

                            using (SqlCommand sum =
                                new SqlCommand(
                                    sumSql,
                                    con,
                                    transaction))
                            {
                                sum.Parameters.AddWithValue("@ClassID", classId);
                                sum.Parameters.AddWithValue("@AcademicYear", academicYear);

                                object value = sum.ExecuteScalar();

                                totalAmount =
                                    value == null || value == DBNull.Value
                                        ? 0m
                                        : Convert.ToDecimal(value);
                            }

                            FeePlan remainingTotal =
                                new FeePlan
                                {
                                    ClassID = classId,
                                    AcademicYear = academicYear,
                                    FeeType = "الرسوم الكلية",
                                    Amount = totalAmount,
                                    DueDate = DateTime.Today,
                                    IsRequired = true,
                                    Notes = string.Empty
                                };

                            int totalPlanId =
                                EnsureTotalPlan(
                                    con,
                                    transaction,
                                    remainingTotal);

                            UpdatePlanAmountAndUnpaidFees(
                                con,
                                transaction,
                                totalPlanId,
                                totalAmount,
                                DateTime.Today,
                                string.Empty);

                            EnsureStudentFeesForPlan(
                                con,
                                transaction,
                                totalPlanId,
                                classId,
                                academicYear,
                                "الرسوم الكلية",
                                totalAmount,
                                DateTime.Today,
                                string.Empty);
                        }

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

        private bool IsTotalFeeType(string feeType)
        {
            string value = (feeType ?? string.Empty).Trim();

            return value == "الرسوم الكلية" ||
                   value == "رسوم كلية" ||
                   value == "الرسوم الكليه" ||
                   value == "رسوم كليه" ||
                   value.Equals(
                       "Total Fee",
                       StringComparison.OrdinalIgnoreCase);
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
