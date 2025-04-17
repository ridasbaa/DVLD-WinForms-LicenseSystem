using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_Business
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }   
        public string Password { get; set; }
        public bool IsActive { get; set; }

        clsPerson PersonInfo;

        public clsUser() 
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.IsActive = false;
            _Mode = enMode.AddNew;
        }

        public clsUser(int UserID, int PersonID, string Username, string Password, bool IsActive)
        {
            this.UserID=UserID;
            this.PersonID=PersonID;
            this.Username=Username;
            this.Password=Password;
            this.IsActive =IsActive;
            this.PersonInfo = clsPerson.Find(PersonID);
            _Mode = enMode.Update;
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string Username)
        {
            return clsUserData.IsUserExist(Username);
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistByPersonID(PersonID);
        }

        private bool _AddNewUser()
        {
            this.Password = clsDataHelper.ComputeHash(this.Password);
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.Username, this.Password, this.IsActive);
            return (this.UserID != -1);
        }
        private bool _UpdateUser()
        {
            this.Password = clsDataHelper.ComputeHash(this.Password);
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.Username, this.Password, this.IsActive);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }

            return false;

        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public static clsUser FindUserByID(int UserID)
        {
            int PersonID = -1;
            string Username = "", Password = "";
            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindUserByPersonID(int PersonID)
        {
            int UserID = -1;
            string Username = "", Password = "";
            bool IsActive = false;

            if (clsUserData.GetUserInfoByPersonID(ref UserID, PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindUserByUsernameAndPassword(string Username, string Password)
        {
            int UserID = -1, PersonID = -1;
            bool IsActive = false;
            if (clsUserData.GetUserInfoByUsernameAndPassword(ref PersonID, ref UserID, Username, Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

    }
}
