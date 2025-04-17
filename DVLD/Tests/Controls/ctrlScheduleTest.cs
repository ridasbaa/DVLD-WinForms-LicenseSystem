using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;
        private clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;
        public clsTestTypes.enTestType TestTypeID
        {
            get { return _TestType; }
            set
            {
                _TestType = value;

                switch (_TestType)
                {
                    case clsTestTypes.enTestType.VisionTest:
                        pbTestTypeImage.Image = Resources.eye;
                        lblTitle.Text = "Schedule Vision Test";
                        gbTestType.Text = "Vision Test";
                        break;
                    case clsTestTypes.enTestType.WrittenTest:
                        pbTestTypeImage.Image = Resources.Written_test;
                        lblTitle.Text = "Schedule Written Test";
                        gbTestType.Text = "Written Test";
                        break;
                    case clsTestTypes.enTestType.StreetTest:
                        pbTestTypeImage.Image = Resources.Street_Test;
                        lblTitle.Text = "Schedule Street Test";
                        gbTestType.Text = "Street Test";
                        break;
                }
            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }


        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show($"No Test Appointment With ID [{_TestAppointmentID.ToString()}] Was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) > 0)
                dtpTestDate.Value = DateTime.Now;
            else
                dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";  
            }
            else
            {
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTest.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
            }

            return true;
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            if (AppointmentID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"No Local Driving License Application With ID [{_LocalDrivingLicenseApplicationID.ToString()}] was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.LicenseClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestType))
            {
                _CreationMode = enCreationMode.RetakeTestSchedule;
            }
            else
            {
                _CreationMode = enCreationMode.FirstTimeSchedule;
            }

            switch(_CreationMode)
            {
                case enCreationMode.FirstTimeSchedule:
                    gbRetakeTest.Enabled = false;
                    lblTitle.Text = "Schedule Test";
                    lblRetakeTestAppID.Text = "N/A";
                    lblRetakeAppFees.Text = "0";
                    break;
                case enCreationMode.RetakeTestSchedule:
                    gbRetakeTest.Enabled = true;
                    lblTitle.Text = "Schedule Retake Test";
                    lblRetakeTestAppID.Text = "N/A";
                    lblRetakeAppFees.Text = clsApplicationType.GetApplicationTypeByID((int)clsApplication.enApplicationType.RetakeTest).ApplicationTypeFees.ToString();
                    break;
            }

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.LicenseClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;

            lblTrial.Text = _LocalDrivingLicenseApplication.TotatTrialsperTestType(_TestType).ToString();

            if (_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestTypes.Find(_TestType).TestTypeFees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                {
                    return;
                }

            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();
            
            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePreviousTestConstraint())
                return;




        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduleTest(_LocalDrivingLicenseApplicationID, _TestType))
            {
                lblUserMessage.Text = "Person Already have an active appointment at this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Peson already sat for the test, appointment locked";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
            {
                lblUserMessage.Visible = false;
            }
            return true;
        }
        private bool _HandlePreviousTestConstraint()
        {
            switch (_TestType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;
                case clsTestTypes.enTestType.WrittenTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot schedule, Vision Test Should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled=false;
                        dtpTestDate.Enabled=false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                        return true;
                    }
                case clsTestTypes.enTestType.StreetTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot schedule, Written Test Should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                        return true;
                    }
            }

            return true;
        }

        private bool HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.GetApplicationTypeByID((int)clsApplication.enApplicationType.RetakeTest).ApplicationTypeFees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Failed To create application", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!HandleRetakeApplication())
                return;

            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.TestTypeID = TestTypeID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data was not saved successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
       
    }
}
