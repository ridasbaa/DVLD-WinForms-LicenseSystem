using DVLD.Global_Classes;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Local_Driving_License
{
    public partial class FrmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.AddNew;

        private int _LDLAppLicationID = -1;
        private int _SelectedPersonID = -1;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseAppLication;

        public FrmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public FrmAddUpdateLocalDrivingLicenseApplication(int lDLAppLicationsID)
        {
            InitializeComponent();
            _LDLAppLicationID = lDLAppLicationsID; 
            _Mode = enMode.Update;
        }

        private void _FillClassesInComboBox()
        {
            DataTable dtClasses = clsLicenseClass.GetAllClasses();

            foreach (DataRow Row in dtClasses.Rows)
            {
                cbLicenseClass.Items.Add(Row["ClassName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            _FillClassesInComboBox();
            if (_Mode == enMode.AddNew)
            {
                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;
                _LocalDrivingLicenseAppLication = new clsLocalDrivingLicenseApplication();
                this.Text = "Add New Local Driving License Application";
                lblTitle.Text = "Add New Local Driving License Application";
                ctrlPersonCardWithFilter1.FilterFocus();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeByID((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationTypeFees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.Username;
            }
            else
            {
                btnNext.Enabled = true;
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";
            }
        }

        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseAppLication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(_LDLAppLicationID);

            if (_LocalDrivingLicenseAppLication == null)
            {
                MessageBox.Show($"No Local Driving License Application With [{_LDLAppLicationID}] Was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseAppLication.ApplicantPersonID);
            lblApplicationID.Text = _LocalDrivingLicenseAppLication.ApplicationID.ToString();    
            lblApplicationDate.Text = _LocalDrivingLicenseAppLication.ApplicationDate.ToString();
            lblApplicationFees.Text = _LocalDrivingLicenseAppLication.PaidFees.ToString();
            cbLicenseClass.SelectedIndex = _LocalDrivingLicenseAppLication.LicenseClassID - 1;
            lblCreatedBy.Text = clsUser.FindUserByID(_LocalDrivingLicenseAppLication.CreatedByUserID).Username;


        }

        private void FrmAddUpdateLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonId != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }

            else
            {
                MessageBox.Show("Please Select a person", "Select a person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }


        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid, put the mouse over the red icons to see the error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            _LocalDrivingLicenseAppLication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonId
                ;
            _LocalDrivingLicenseAppLication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseAppLication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseAppLication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseAppLication.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            _LocalDrivingLicenseAppLication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicenseAppLication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseAppLication.LicenseClassID = LicenseClassID;

            if (_LocalDrivingLicenseAppLication.Save())
            {
                lblApplicationID.Text = _LocalDrivingLicenseAppLication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving Licence Application";
                MessageBox.Show("Data saved Successfully", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error : Data was not saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
    }
}
