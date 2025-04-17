using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypesData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dtTestTypes = new DataTable();
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from testTypes";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtTestTypes.Load(reader);
                }
                reader.Close();
            }
            catch(Exception ex) { }
            finally { conn.Close(); }
            return dtTestTypes;
        }

        public static bool UpdateTestType(int testTypeID, string testTypeTitle, string testTypeDescription, float testTypeFees)
        {
            int rowsAffected = 0;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  TestTypes  
                            set TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription=@TestTypeDescription,
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@TestTypeTitle", testTypeTitle);
            cmd.Parameters.AddWithValue("@TestTypeDescription", testTypeDescription);
            cmd.Parameters.AddWithValue("@TestTypeFees", testTypeFees);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { return false; }    
            finally { conn.Close(); }
            return (rowsAffected > 0);

        }

        public static int AddNewTestType(string testTypeTitle, string testTypeDescription, float testTypeFees)
        {
            int TestTypeID = -1;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert Into TestTypes (testTypeTitle, testTypeDescription, testTypeFees) Values (@testTypeTitle, @testTypeDescription, @testTypeFees); select scope_Identity();";

            SqlCommand cmd = new SqlCommand(@query, conn);

            cmd.Parameters.AddWithValue("@testTypeTitle", testTypeTitle);
            cmd.Parameters.AddWithValue("@testTypeDescription", testTypeDescription);
            cmd.Parameters.AddWithValue("@testTypeFees", testTypeFees);

            try
            {
                conn.Open();
                object Result = cmd.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    TestTypeID = InsertedID;
                }
            }
            catch (Exception ex) { }
            finally { conn.Close(); }
            return TestTypeID;
        }

        public static bool GetTestTypeByID(int testTypeID, ref string testTypeTitle, ref string testTypeDescription, ref float testTypeFees)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from TestTypes where testTypeID = @testTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@testTypeID", testTypeID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    testTypeTitle = (string)reader["testTypeTitle"];
                    testTypeDescription = (string)reader["testTypeDescription"];
                    testTypeFees = Convert.ToSingle(reader["testTypeFees"]);
                }
                else
                {
                    IsFound = false;
                }
            }
            catch(Exception ex) { IsFound = false; }
            finally { conn.Close(); }
            return IsFound;
        }


    }
}
