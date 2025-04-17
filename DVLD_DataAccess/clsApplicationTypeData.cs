using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dtTypes = new DataTable();
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Select * from applicationTypes";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTypes.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return dtTypes;
        }

        public static bool GetApplicationTypeByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationTypeFees)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand cmd = new SqlCommand(@query, conn);

            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationTypeFees = Convert.ToSingle(reader["ApplicationFees"]);
                }
                else
                {
                    IsFound = false;
                }
            }
            catch (System.Exception ex) { }
            finally { conn.Close(); }

            return IsFound;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            int rowsAffected = 0;
            SqlConnection conn  = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update ApplicationTypes SET ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationTypeFees
                             where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            cmd.Parameters.AddWithValue("@ApplicationTypeFees", ApplicationTypeFees);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();   
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return (rowsAffected > 0);
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            int ApplicationTypeID = -1;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Insert into ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees) Values (@ApplicationTypeTitle, @ApplicationTypeFees);
                             select Scope_identity();";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            cmd.Parameters.AddWithValue("@ApplicationTypeFees", ApplicationTypeFees);

            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    ApplicationTypeID = InsertedID;
                }

            }
            catch (Exception ex) { }
            finally { conn.Close(); }
            return ApplicationTypeID;
        }



    }
}
