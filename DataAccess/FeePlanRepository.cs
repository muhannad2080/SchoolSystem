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

                        transaction.Commit();

                        return affected > 0;
                    }
                }
            }
        }

        // تعريف الدالة DistributeTotalFee لتنفيذ العملية المرتبطة بها.
        public int DistributeTotalFee(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string selectSql = @"
                    SELECT FeePlanID, Amount
                    FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                    WHERE ClassID = @ClassID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                    ORDER BY FeePlanID;";


                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    var plans = new List<Tuple<int, decimal>>();
                    using (SqlCommand select = new SqlCommand(selectSql, con, transaction))
                    {

                        select.Parameters.AddWithValue("@ClassID", plan.ClassID);

                        select.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? string.Empty);

                        using (SqlDataReader reader = select.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                decimal amount = reader["Amount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Amount"]);
                                if (amount > 0m)
                                    plans.Add(Tuple.Create(Convert.ToInt32(reader["FeePlanID"]), amount));
                            }
                        }
                    }

                    if (plans.Count == 0)
                    {
                        const string insertTotalPlanSql = @"
                            IF NOT EXISTS
                            (
                                SELECT 1 FROM FeePlans
                                WHERE ClassID = @ClassID
                                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                            )
                            BEGIN
                                INSERT INTO FeePlans (AcademicYear, ClassID, FeeType, Amount, DueDate, IsRequired, Notes)
                                VALUES (@AcademicYear, @ClassID, N'الرسوم الكلية', @Amount, @DueDate, 1, @Notes);
                            END";

                        using (SqlCommand insert = new SqlCommand(insertTotalPlanSql, con, transaction))
                        {
                            insert.Parameters.AddWithValue("@ClassID", plan.ClassID);
                            insert.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? string.Empty);
                            insert.Parameters.AddWithValue("@Amount", plan.Amount);
                            insert.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                            insert.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(plan.Notes) ? (object)DBNull.Value : plan.Notes);
                            insert.ExecuteNonQuery();
                        }

                        string[] childTypes = new[]
                        {
                            "رسوم دراسية", "رسوم كتب", "رسوم زي مدرسي",
                            "رسوم مواصلات", "رسوم امتحانات", "رسوم أنشطة", "رسوم تسجيل"
                        };

                        // أوزان منطقية تقريبية تعكس الأهمية النسبية لكل نوع.
                        decimal[] weights = new[]
                        {
                            40m, 12m, 8m, 8m, 12m, 5m, 15m
                        };

                        decimal weightSum = 0m;
                        for (int i = 0; i < weights.Length; i++)
                            weightSum += weights[i];

                        decimal childAllocated = 0m;
                        int created = 0;

                        for (int i = 0; i < childTypes.Length; i++)
                        {
                            string feeType = childTypes[i];
                            decimal amt = i == childTypes.Length - 1
                                ? plan.Amount - childAllocated
                                : Math.Round(plan.Amount * weights[i] / weightSum, 2, MidpointRounding.AwayFromZero);
                            if (amt < 0m) amt = 0m;
                            childAllocated += amt;

                            const string insertChildSql = @"
                                IF NOT EXISTS
                                (
                                    SELECT 1 FROM FeePlans
                                    WHERE ClassID = @ClassID
                                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                      AND FeeType = @FeeType
                                )
                                BEGIN
                                    INSERT INTO FeePlans (AcademicYear, ClassID, FeeType, Amount, DueDate, IsRequired, Notes)
                                    VALUES (@AcademicYear, @ClassID, @FeeType, @Amount, @DueDate, 1, @Notes);
                                    SELECT 1;
                                END
                                ELSE
                                    SELECT 0;";

                            using (SqlCommand insertChild = new SqlCommand(insertChildSql, con, transaction))
                            {
                                insertChild.Parameters.AddWithValue("@ClassID", plan.ClassID);
                                insertChild.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? string.Empty);
                                insertChild.Parameters.AddWithValue("@FeeType", feeType);
                                insertChild.Parameters.AddWithValue("@Amount", amt);
                                insertChild.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                                insertChild.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(plan.Notes) ? (object)DBNull.Value : plan.Notes);
                                object r = insertChild.ExecuteScalar();
                                if (r != null && r != DBNull.Value && Convert.ToInt32(r) == 1)
                                    created++;
                            }
                        }

                        transaction.Commit();
                        return created;
                    }

                    const string paidSql = @"
                        SELECT COUNT(1)
                        FROM Fees f
                        INNER JOIN FeePlans fp ON fp.FeePlanID = f.FeePlanID
                        WHERE fp.FeePlanID = @FeePlanID AND ISNULL(f.PaidAmount, 0) > 0;";

                    decimal baseTotal = 0m;

                    for (int i = 0; i < plans.Count; i++)
                        baseTotal += plans[i].Item2;


                    if (baseTotal <= 0m)

                        throw new InvalidOperationException("لا يمكن توزيع المبلغ لأن أنواع الرسوم الموجودة لا تحتوي على مبالغ.");

                    decimal allocated = 0m;

                    for (int i = 0; i < plans.Count; i++)
                    {
                        int feePlanId = plans[i].Item1;
                        using (SqlCommand paid = new SqlCommand(paidSql, con, transaction))
                        {

                            paid.Parameters.AddWithValue("@FeePlanID", feePlanId);

                            if (Convert.ToInt32(paid.ExecuteScalar()) > 0)

                                throw new InvalidOperationException("لا يمكن إعادة توزيع رسوم الصف لأن أحد أنواع الرسوم يحتوي على سداد سابق.");
                        }

                        decimal newAmount = i == plans.Count - 1
                            ? plan.Amount - allocated
                            : Math.Round(plan.Amount * plans[i].Item2 / baseTotal, 2, MidpointRounding.AwayFromZero);
                        if (newAmount < 0m)
                            newAmount = 0m;
                        allocated += newAmount;

                        const string updateSql = @"
                            UPDATE FeePlans
                            SET Amount = @Amount,
                                DueDate = @DueDate,
                                Notes = @Notes
                            WHERE FeePlanID = @FeePlanID;

                            UPDATE f SET
                                TotalAmount = @Amount,
                                NetAmount = CASE WHEN ISNULL(f.DiscountAmount, 0) > @Amount THEN 0 ELSE @Amount - ISNULL(f.DiscountAmount, 0) END,
                                RemainingAmount = CASE WHEN ISNULL(f.DiscountAmount, 0) > @Amount THEN 0 ELSE @Amount - ISNULL(f.DiscountAmount, 0) - ISNULL(f.PaidAmount, 0) END,
                                DueDate = @DueDate,
                                UpdatedAt = GETDATE()
                            FROM Fees f
                            WHERE f.FeePlanID = @FeePlanID AND ISNULL(f.PaidAmount, 0) = 0;";

                        using (SqlCommand update = new SqlCommand(updateSql, con, transaction))
                        {

                            update.Parameters.AddWithValue("@FeePlanID", feePlanId);

                            update.Parameters.AddWithValue("@Amount", newAmount);

                            update.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);

                            update.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(plan.Notes) ? (object)DBNull.Value : plan.Notes);

                            update.ExecuteNonQuery();
                        }
                    }

                    const string insertTotalSql = @"
                        IF NOT EXISTS
                        (
                            SELECT 1 FROM FeePlans
                            WHERE ClassID = @ClassID
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                              AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                        )
                        BEGIN
                            INSERT INTO FeePlans (AcademicYear, ClassID, FeeType, Amount, DueDate, IsRequired, Notes)
                            VALUES (@AcademicYear, @ClassID, N'الرسوم الكلية', @Amount, @DueDate, 1, @Notes);
                        END";

                    using (SqlCommand insertTotal = new SqlCommand(insertTotalSql, con, transaction))
                    {
                        insertTotal.Parameters.AddWithValue("@ClassID", plan.ClassID);
                        insertTotal.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? string.Empty);
                        insertTotal.Parameters.AddWithValue("@Amount", plan.Amount);
                        insertTotal.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
                        insertTotal.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(plan.Notes) ? (object)DBNull.Value : plan.Notes);
                        insertTotal.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    return plans.Count;
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

        // تعريف الدالة DeleteFeePlan لتنفيذ العملية المرتبطة بها.
        public bool DeleteFeePlan(int feePlanId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    DELETE FROM Fees
                    WHERE FeePlanID = @FeePlanID;

                    DELETE FROM FeePlans
                    WHERE FeePlanID = @FeePlanID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeePlanID", feePlanId);


                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
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
