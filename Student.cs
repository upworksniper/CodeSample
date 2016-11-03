using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PTS.DataAccess
{
    internal class Student
    {
        public int StudId { get; set; }
        public string StudName { get; set; }
        public int Group { get; set; }
        public string FatherName { get; set; }
        public string FContactNo { get; set; }
        public string MotherName { get; set; }
        public string MContactNo { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime AppliedDate { get; set; }
        public int AdmissionFee { get; set; }
        public int MonthlyTuitionFee { get; set; }
        public int AdmissionPaidAmount { get; set; }
        public string ProximityId { get; set; }
        public int IsDueNotifier { get; set; }
        public int StudStatus { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public byte[] Photo { get; set; }

        public bool Save()
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
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "qry_Student_Insert";
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.Parameters.AddWithValue("@StudName", StudName);
                            cmd.Parameters.AddWithValue("@Gender", Gender);
                            cmd.Parameters.AddWithValue("@DOB", DOB);
                            cmd.Parameters.AddWithValue("@ContactNo", Contact);
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Group", Group);
                            cmd.Parameters.AddWithValue("@FatherName", FatherName);
                            cmd.Parameters.AddWithValue("@FContactNo", FContactNo);
                            cmd.Parameters.AddWithValue("@MotherName", MotherName);
                            cmd.Parameters.AddWithValue("@MContactNo", MContactNo);
                            cmd.Parameters.AddWithValue("@Address", Address);
                            cmd.Parameters.Add("@Photo", SqlType.Binary).Value = Photo;
                            cmd.Parameters.AddWithValue("@ProximityId", ProximityId);
                            cmd.Parameters.AddWithValue("@IsDueNotifier", IsDueNotifier);
                            cmd.Parameters.AddWithValue("@StudStatus", StudStatus);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText =
                                "INSERT INTO Admission(StudId, AdmissionDate, AppliedDate, AdmissionFee, MonthlyTuitionFee, IsPaid)	VALUES (@StudId, @AdmissionDate, @AppliedDate,@AdmissionFee,@MonthlyTuitionFee,@IsPaid)";
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.Parameters.AddWithValue("@AdmissionDate", AdmissionDate);
                            cmd.Parameters.AddWithValue("@AppliedDate", AppliedDate);
                            cmd.Parameters.AddWithValue("@AdmissionFee", AdmissionFee);
                            cmd.Parameters.AddWithValue("@MonthlyTuitionFee", MonthlyTuitionFee);
                            var isPaid = AdmissionFee.ToInt() == 0 ? 1 : 0;
                            cmd.Parameters.AddWithValue("@IsPaid", isPaid);
                            cmd.ExecuteNonQuery();
                        }
                       
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText =
                                "INSERT INTO LastValidMonth (StudId, ValidMonth) VALUES (@StudId, @ValidMonth)";
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.Parameters.AddWithValue("@ValidMonth", AppliedDate.AddMonths(-1));
                            cmd.ExecuteNonQuery();
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

        public bool Update()
        {
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        string query;
                        using (var cmd = new SqlCommand("qry_Student_Update", conn))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StudName", StudName);
                            cmd.Parameters.AddWithValue("@Gender", Gender);
                            cmd.Parameters.AddWithValue("@DOB", DOB);
                            cmd.Parameters.AddWithValue("@ContactNo", Contact);
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Group", Group);
                            cmd.Parameters.AddWithValue("@FatherName", FatherName);
                            cmd.Parameters.AddWithValue("@FContactNo", FContactNo);
                            cmd.Parameters.AddWithValue("@MotherName", MotherName);
                            cmd.Parameters.AddWithValue("@MContactNo", MContactNo);
                            cmd.Parameters.AddWithValue("@Address", Address);
                            cmd.Parameters.Add("@Photo", SqlType.Binary).Value = Photo;
                            cmd.Parameters.AddWithValue("@ProximityId", ProximityId);
                            cmd.Parameters.AddWithValue("@IsDueNotifier", IsDueNotifier);
                            cmd.Parameters.AddWithValue("@StudStatus", StudStatus);
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            query =
                                "UPDATE Admission SET AdmissionDate = @AdmissionDate, AppliedDate=@AppliedDate, AdmissionFee =@AdmissionFee, MonthlyTuitionFee=@MonthlyTuitionFee,IsPaid=@IsPaid  WHERE StudId = @StudId";
                            cmd.CommandText = query;

                            cmd.Parameters.AddWithValue("@AdmissionDate", AdmissionDate);
                            cmd.Parameters.AddWithValue("@AppliedDate", AppliedDate);
                            cmd.Parameters.AddWithValue("@AdmissionFee", AdmissionFee);
                            cmd.Parameters.AddWithValue("@MonthlyTuitionFee", MonthlyTuitionFee);
                            var isPaid = AdmissionFee.ToInt() == 0 ? 1 : 0;
                            cmd.Parameters.AddWithValue("@IsPaid", isPaid);
                            cmd.Parameters.AddWithValue("@StudId", StudId);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.Text;
                            query = string.Format("UPDATE LastValidMonth SET ValidMonth = #{0}# WHERE StudId = {1}",
                                AppliedDate.AddMonths(-1).ToShortDateString(), StudId);
                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();
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

        public static string GetActive()
        {
            var total = "0";
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                const string query = "SELECT COUNT(StudId) AS Total FROM Student WHERE StudStatus =1";
                using (var cmd = new SqlCommand(query, conn))
                {
                    var dr = cmd.ExecuteReader();
                    while (dr != null && dr.Read())
                    {
                        total = dr["Total"].ToString();
                        break;
                    }
                }
            }
            return total;
        }

        public static string GetInactive()
        {
            var total = "0";
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = "SELECT COUNT(StudId) AS Total FROM Student WHERE StudStatus =0";
                using (var cmd = new SqlCommand(query, conn))
                {
                    var dr = cmd.ExecuteReader();
                    while (dr != null && dr.Read())
                    {
                        total = dr["Total"].ToString();
                        break;
                    }
                }
            }
            return total;
        }

        public static DataTable GetStudentByGroup(string group)
        {
            var dtStudent = new DataTable();
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT * FROM qry_Student_View WHERE GroupName='{0}'", group);
                using (var cmd = new SqlCommand(query, conn))
                {
                    dtStudent.Load(cmd.ExecuteReader());
                }
            }
            return dtStudent;
        }

        public static string GetStudentNameById(string studId)
        {
            var studName = @"";
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                const string query = "SELECT StudName FROM Student WHERE StudId=?";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", studId);
                    var dr = cmd.ExecuteReader();
                    if (dr != null && dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            studName = dr["StudName"].ToString();
                            break;
                        }
                    }
                }
            }
            return studName;
        }

        public static List<string> Get3Contact(string studId)
        {
            var contact = new List<string>();
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT ContactNo, FContactNo, MContactNo FROM Student WHERE StudId={0}",
                    studId);
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    var dr = cmd.ExecuteReader();
                    if (dr != null && dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr["ContactNo"] != null)
                            {
                                contact.Add(dr["ContactNo"].ToString());
                            }
                            if (dr["FContactNo"] != null)
                            {
                                contact.Add(dr["FContactNo"].ToString());
                            }
                            if (dr["MContactNo"] != null)
                            {
                                contact.Add(dr["MContactNo"].ToString());
                            }
                            break;
                        }
                    }
                }
            }
            return contact;
        }

        public static List<string> GetParentsContact(string studId)
        {
            var contact = new List<string>();
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT FContactNo, MContactNo FROM Student WHERE StudId={0}", studId);
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    var dr = cmd.ExecuteReader();
                    if (dr != null && dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr["FContactNo"] != null)
                            {
                                contact.Add(dr["FContactNo"].ToString());
                            }
                            if (dr["MContactNo"] != null)
                            {
                                contact.Add(dr["MContactNo"].ToString());
                            }
                            break;
                        }
                    }
                }
            }
            return contact;
        }

        public static bool IsAvaliable(string studId)
        {
            var isFound = false;
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT StudId FROM Student WHERE StudId={0} AND StudStatus=1", studId);
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr != null && dr.HasRows)
                        {
                            isFound = true;
                        }
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetStudent(string studId)
        {
            var dtStudent = new DataTable();
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT * FROM qry_Student_All_View WHERE StudId={0}", studId);
                using (var cmd = new SqlCommand(query, conn))
                {
                    dtStudent.Load(cmd.ExecuteReader());
                }
            }
            return dtStudent;
        }

        public static DataTable SearchStudent(string groupName)
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                if (groupName == "All")
                {
                    using (var cmd = new SqlCommand("SELECT * FROM qry_Student_View ORDER BY StudId ASC", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        dtResult.Load(cmd.ExecuteReader());
                    }
                }
                else if (groupName == "INACTIVE")
                {
                    using (
                        var cmd = new SqlCommand(
                            "SELECT * FROM qry_Report_Inactive_Student_List ORDER BY StudId ASC", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        dtResult.Load(cmd.ExecuteReader());
                    }
                }
                else
                {
                    using (
                        var cmd =
                            new SqlCommand("SELECT * FROM qry_Student_View WHERE GroupName=? ORDER BY StudId ASC",
                                conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("?", groupName);
                        dtResult.Load(cmd.ExecuteReader());
                    }
                }
            }
            return dtResult;
        }

        public static DataTable SearchStudentSingle(string groupName, string studId)
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                if (groupName == "All")
                {
                    using (
                        var cmd =
                            new SqlCommand(
                                "SELECT * FROM qry_Student_View WHERE StudId= @studId ORDER BY StudId ASC", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@studId", studId);
                        dtResult.Load(cmd.ExecuteReader());
                    }
                }
                else
                {
                    using (
                        var cmd =
                            new SqlCommand(
                                "SELECT * FROM qry_Student_View WHERE GroupName=@groupName AND StudId=@studId ORDER BY StudId ASC",
                                conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@groupName", groupName);
                        cmd.Parameters.AddWithValue("@studId", studId);
                        dtResult.Load(cmd.ExecuteReader());
                    }
                }
            }
            return dtResult;
        }

        public static DataTable GetContacts(int studId)
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                using (
                    var cmd =
                        new SqlCommand(
                            string.Format("SELECT ContactNo, FContactNo, MContactNo FROM Student WHERE StudId={0}",
                                studId), conn))
                {
                    dtResult.Load(cmd.ExecuteReader());
                }
            }
            return dtResult;
        }

        public static DataTable Search(string srcInput)
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT * FROM qry_Student_View WHERE StudName LIKE '%{0}%' OR StudId LIKE '%{0}%' OR MContactNo LIKE '%{0}%' OR FContactNo LIKE '%{0}%' OR ContactNo LIKE '%{0}%'", srcInput);
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    dtResult.Load(cmd.ExecuteReader());
                }
            }
            return dtResult;
        }

        public static DataTable Search(string groupName, string studName)
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query =
                    string.Format("SELECT * FROM qry_Student_View WHERE GroupName='{0}' AND StudName LIKE '%{1}%'",
                        groupName, studName);
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    dtResult.Load(cmd.ExecuteReader());
                }
            }
            return dtResult;
        }

        internal static DateTime GetLastValidMonth(object studId)
        {
            var date = new DateTime(2016,2,14);
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                var query = string.Format("SELECT ValidMonth FROM LastValidMonth WHERE StudId={0}", studId);
                using (var cmd = new SqlCommand(query, conn))
                {
                    var dr = cmd.ExecuteReader();
                    if (dr != null && !dr.HasRows) return date;
                    while (dr.Read())
                    {
                        date = Convert.ToDateTime(dr["ValidMonth"]);
                    }
                }
            }
            return date;
        }


        public static string AutoId()
        {
            var autoId = @"1";
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                const string query = "SELECT MAX(StudId) AS NextId FROM Student";
                using (var cmd = new SqlCommand(query, conn))
                {
                    var dr = cmd.ExecuteReader();
                    if (dr != null && dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var nextId = dr[0].ToInt() + 1;
                            autoId = nextId.ToString();
                        }
                    }
                }
            }
            return autoId;
        }


        internal static DataTable GetAll()
        {
            var dtResult = new DataTable("Student");
            using (var conn = new SqlConnection(Sniper.ConnString))
            {
                conn.Open();
                const string query = "SELECT StudId, StudName, ContactNo, Group FROM Student WHERE StudStatus =1";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    dtResult.Load(cmd.ExecuteReader());
                }
            }
            return dtResult;
        }
    }
}
