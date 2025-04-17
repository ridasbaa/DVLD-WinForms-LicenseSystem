using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public  class clsUserData
    {

        public static DataTable GetAllUsers()
       {
            DataTable dtUsers = new DataTable();    
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT  Users.UserID, Users.PersonID,
                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People ON Users.PersonID = People.PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    dtUsers.Load(reader);
                }
                reader.Close(); 
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return dtUsers;
        }

        public static int AddNewUser(int PersonID, string Username, string Password, bool IsActive)
        {
            int UserID = -1;
            SqlConnection conn = new SqlConnection (clsDataAccessSettings.ConnectionString);
            string query = @"Insert Into users (PersonID, Username, Password, IsActive) Values (@PersonID, @Username, @Password, @IsActive); 
                             select SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand (query, conn);
            cmd.Parameters.AddWithValue ("@PersonID", PersonID);
            cmd.Parameters.AddWithValue ("@Username", Username);
            cmd.Parameters.AddWithValue ("@Password", Password);
            cmd.Parameters.AddWithValue ("@IsActive", IsActive);

            try
            {
                conn.Open();

                object Result = cmd.ExecuteScalar();

                if(Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    UserID = InsertedID;
                }

            }
            catch (Exception ex) { } 
            finally { conn.Close(); }

            return UserID;

        }
        public static bool UpdateUser(int UserID, int PersonID, string Username, string Password, bool IsActive)
        {
            int rowsAffected = 0;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update Users 
                             SET PersonID = @PersonID,
                                 Username = @Username,
                                 Password = @Password,
                                 IsActive = @IsActive
                           WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, conn);   

            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {}
            finally { conn.Close(); }

            return (rowsAffected > 0);
        }
        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Delete * from Users where UserID = @UserID ";

            SqlCommand cmd=  new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch(Exception ex) { }
            finally { conn.Close(); }

            return (rowsAffected > 0);
        }

        public static bool GetUserInfoByUsernameAndPassword(ref int PersonID, ref int UserID, string Username, string Password, ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection (clsDataAccessSettings.ConnectionString);

            string query = @"Select * from Users where Username = @Username and Password = @Password";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    IsActive = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
                }

            }
            catch(Exception ex) { } 
            finally { conn.Close(); }
            return IsFound;
        }
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from Users where UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception e) { IsFound = false; }
            finally { conn.Close(); }
            return IsFound;

        }
        public static bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from Users where PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    UserID = (int)reader["UserID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception e) { IsFound = false; }
            finally { conn.Close(); }
            return IsFound;

        }

        public static bool IsUserExist(int UserID)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select found = 1 from Users where UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    IsFound = true;
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex) { IsFound = false; }
            finally { conn.Close(); }

            return IsFound;
        }
        public static bool IsUserExist(string Username)
        {
            bool IsFound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select found = 1 from Users where Username = @Username";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", Username);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    IsFound = true;
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex) { IsFound = false; }
            finally { conn.Close(); }

            return IsFound;
        }
        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select found = 1 from users where PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                IsFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception  ex) { IsFound = false; }  
            finally { conn.Close(); }
            return IsFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Users Set Password = @NewPassword where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@NewPassword", NewPassword);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return (rowsAffected > 0);
        }

    }
}
