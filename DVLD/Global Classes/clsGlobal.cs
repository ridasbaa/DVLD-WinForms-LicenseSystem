using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using Microsoft.Win32;
using System.Net.Http.Headers;

namespace DVLD.Global_Classes
{
    internal static  class clsGlobal
    {

        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPaasword(string Username, string Password)
        {

            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string UsernameKey = "Username";
            string PasswordKey= "Password";

            try
            {
                Registry.SetValue(KeyPath, UsernameKey, Username, RegistryValueKind.String);
                Registry.SetValue(KeyPath, PasswordKey, Password, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error Occured [{ex.Message}]");
                return false;
            }





            //try
            //{
            //    string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

            //    string FilePath = CurrentDirectory + "\\data.txt";

            //    if (Username == "" && File.Exists(FilePath))
            //    {
            //        File.Delete(FilePath);
            //        return true;
            //    }

            //    string datatosave = Username + "#//#" + Password;

            //    using (StreamWriter writer = new StreamWriter(FilePath))
            //    {
            //        writer.WriteLine(datatosave);
            //        return true;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error Occured [{ex.Message}]");
            //    return false;
            //}


        }

        public static bool GetStoredCredentials(ref string Username, ref string Password)
        {

            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string UsernameKey = "Username";
            string PasswordKey = "Password";

            try
            {
                string UsernameValue = Registry.GetValue(KeyPath, UsernameKey, null) as string;
                if (!string.IsNullOrEmpty(UsernameValue))
                {
                    Username = UsernameValue;
                }
                else
                {
                    return false;
                }

                string PasswordValue = Registry.GetValue(KeyPath, PasswordKey, null) as string;
                if (!string.IsNullOrEmpty(PasswordValue))
                {
                    Password = PasswordValue;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error Occured [{ex.Message}]");
                return false;
            }


            //try
            //{

            //    string FilePath = System.IO.Directory.GetCurrentDirectory() + "\\data.txt";

            //    if (File.Exists(FilePath))
            //    {
            //        using (StreamReader reader = new StreamReader(FilePath))
            //        {
            //            string Line;
            //            while ((Line = reader.ReadLine()) != null)
            //            {
            //                Console.WriteLine(Line);
            //                string[] result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);
            //                Username = result[0];
            //                Password = result[1];   
            //            }

            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error Occured [{ex.Message}]");
            //    return false;
            //}

        }





    }
}
