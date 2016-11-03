using System;
using System.Data;
using System.Data.SqlClient;

namespace PTS.DataAccess
{
    internal class Payment
    {
        public int StudId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ValidMonth { get; set; }
        public decimal ValidMonthAmount { get; set; }
        public int ExamFee { get; set; }
        public int TestFee { get; set; }
        public int MockFee { get; set; }

        // Additional Parameter for Receipt
        public string AdmissionFee { get; set; }
        public string ExamFee2 { get; set; }
        public string TestFee2 { get; set; }
        public string MockFee2 { get; set; }
        public string Payable { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string StudName { get; set; }
        public string GroupName { get; set; }
        public string Contact { get; set; }
        public string MonthOf { get; set; }
        public string ReceivedBy { get; set; }

        public bool Save(bool isAdmissionPaid, bool isExamFeePaid, bool isTestFeePaid, bool isMockFeePaid)
        {
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText =
                                "INSERT INTO Payment(StudId, PaymentDate, ValidMonth, ValidMonthAmount, UserId)VALUES (@StudId, @PaymentDate, @ValidMonth,@ValidMonthAmount,@UserId)";
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                            cmd.Parameters.AddWithValue("@ValidMonth", ValidMonth);
                            cmd.Parameters.AddWithValue("@ValidMonthAmount", ValidMonthAmount);
                            cmd.Parameters.AddWithValue("@UserId", Sniper.UserId);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE LastValidMonth SET ValidMonth=? WHERE StudId=?";
                            cmd.Parameters.AddWithValue("@ValidMonth", ValidMonth);
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.ExecuteNonQuery();
                        }

                        if (isAdmissionPaid)
                        {
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                var query = string.Format("UPDATE Admission SET IsPaid=1 WHERE StudId={0}", StudId);
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Exam Fee
                        if (isExamFeePaid)
                        {
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText =
                                    "INSERT INTO ExamFee (PaymentDate, StudId, ExamFee) VALUES (@PaymentDate, @StudId, @ExamFee)";
                                cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Today.Date);
                                cmd.Parameters.AddWithValue("@StudId", StudId);
                                cmd.Parameters.AddWithValue("@ExamFee", ExamFee);
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                var query = string.Format("UPDATE ExamFeeList SET IsPaid=1 WHERE StudId={0}", StudId);
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Test Fee
                        if (isTestFeePaid)
                        {
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText =
                                    "INSERT INTO TestFee (PaymentDate, StudId, TestFee) VALUES (@PaymentDate, @StudId, @TestFee)";
                                cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Today.Date);
                                cmd.Parameters.AddWithValue("@StudId", StudId);
                                cmd.Parameters.AddWithValue("@TestFee", TestFee);
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                var query = string.Format("UPDATE TestFeeList SET IsPaid=1 WHERE StudId={0}", StudId);
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Mock Fee
                        if (isMockFeePaid)
                        {
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText =
                                    "INSERT INTO MockFee (PaymentDate, StudId, MockFee) VALUES (@PaymentDate, @StudId, @MockFee)";
                                cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Today.Date);
                                cmd.Parameters.AddWithValue("@StudId", StudId);
                                cmd.Parameters.AddWithValue("@MockFee", MockFee);
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.Text;
                                var query = string.Format("UPDATE MockFeeList SET IsPaid=1 WHERE StudId={0}", StudId);
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                        }
 
                        tran.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public static DataTable GetLastValidMonth(string studId)
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("qry_Student_SearchForPayment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudId", studId);
                    dt.Load(cmd.ExecuteReader());
                }
                if (dt.Rows.Count == 0)
                {
                    using (var cmd = new SqlCommand("qry_Student_SearchForPayment_Empty", conn))
                    {
                        dt.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StudId", studId);
                        dt.Load(cmd.ExecuteReader());
                    }
                }
            }
            return dt;
        }

        public static DataTable GetPaymentHistory(string studId)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(string.Format("SELECT * FROM Payment WHERE StudId={0}", studId), conn)
                    )
                {
                    dt.Load(cmd.ExecuteReader());
                }
            }
            return dt;
        }

        public static bool Delete(object id, string studId)
        {
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var query = string.Format("DELETE FROM Payment WHERE Id={0}", id);
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();

                            query = string.Format("SELECT MAX(ValidMonth) AS ValidMonth FROM Payment WHERE StudId={0}",
                                studId);
                            cmd.CommandText = query;
                            var dr = cmd.ExecuteReader();
                            var validM = new DateTime();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    validM = Convert.ToDateTime(dr["ValidMonth"]);
                                    break;
                                }
                            }
                            dr.Close();
                            query = string.Format("UPDATE LastValidMonth SET ValidMonth = ? WHERE StudId={0}", studId);
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("?", validM.Date);
                            cmd.ExecuteNonQuery();
                            tran.Commit();
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }
 
    }
}
