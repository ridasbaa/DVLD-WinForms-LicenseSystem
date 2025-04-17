using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public static DataTable GetAllCountries()
        {
            DataTable dtCountries = new DataTable();    
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from countries order by CountryName";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    dtCountries.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return dtCountries;
        }

        public static bool GetCountryByID(int ID, ref string CountryName)
        {
            bool IsFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from countries where CountryID = @CountryID ";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    CountryName = (string)reader["CountryName"];
                }
                else
                {
                    IsFound = false;
                }
            }
            catch (Exception ex) { IsFound = false; }
            finally { conn.Close(); }

            return IsFound;
        }
        public static bool GetCountryByName(ref int ID, string CountryName)
        {
            bool IsFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from countries where CountryName = @CountryName";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    ID = (int)reader["CountryID"];
                }
                else
                {
                    IsFound = false;
                }
            }
            catch (Exception ex) { IsFound = false; }
            finally { conn.Close(); }

            return IsFound;
        }
    }
}
