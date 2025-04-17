using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDrivingLicenseInfo : UserControl
    {
        private int _LicenseID = -1;
        clsLicense _License;

        public int LicenseID { get { return _LicenseID; } }
        public clsLicense SelectedLicenseInfo { get { return _License; } }

        public ctrlDrivingLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LicenseID)
        {
            _License = clsLicense.Find(LicenseID);

            if (_License == null)
            {
                MessageBox.Show($"No License with ID [{LicenseID.ToString()}] was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            _LicenseID = _License.LicenseID;
            lblClass.Text = _License.LicenseClassIfo.LicenseClassName;
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);
            lblDriveriD.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblGender.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblName.Text = _License.DriverInfo.PersonInfo.FirstName;
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblNotes.Text = _License.Notes == string.Empty ? "No Notes" : _License.Notes;
            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor == 0)
            {
                pbLicensePicture.Image = Resources.Male;
            }
            else
            {
                pbLicensePicture.Image = Resources.Female;
            }

            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                    pbLicensePicture.Load(ImagePath);
            }
            else
            {
                MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
