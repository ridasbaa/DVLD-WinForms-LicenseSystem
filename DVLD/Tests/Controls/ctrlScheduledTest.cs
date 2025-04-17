using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Test_Appointments
{
    public partial class ctrlScheduledTest : UserControl
    {
        private int _TestID;
        public int TestID
        {
            get { return _TestID; }
        }

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;
        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
        }

        private clsTestTypes.enTestType _TestType;
        public clsTestTypes.enTestType TestType
        {
            get { return _TestType; }
            set
            {
                _TestType = value;

                switch (_TestType)
                {
                    case clsTestTypes.enTestType.VisionTest:
                        pbTestTypeImage.Image = Resources.eye;
                        lblTitle.Text = "Vision Test";
                        gbTestType.Text = "Vision Test";
                        break;
                    case clsTestTypes.enTestType.WrittenTest:
                        pbTestTypeImage.Image = Resources.Written_test;
                        lblTitle.Text = "Written Test";
                        gbTestType.Text = "Written Test";
                        break;
                    case clsTestTypes.enTestType.StreetTest:
                        pbTestTypeImage.Image = Resources.Street_Test;
                        lblTitle.Text = "Street Test";
                        gbTestType.Text = "Street Test";
                        break;
                }
            }
        }

        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int testAppointmentID)
        {
            _TestAppointmentID = testAppointmentID;
            _TestAppointment = clsTestAppointment.Find(testAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show($"No Test Appointment with ID [{testAppointmentID.ToString()}] was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestID = _TestAppointment.TestID; 

            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(_TestAppointment.LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"No Local Driving License Application with ID [{_TestAppointment.LocalDrivingLicenseApplicationID.ToString()}] was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseAppID.Text = _TestAppointment.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.LicenseClassName;
            lblName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotatTrialsperTestType(_TestType).ToString();
            lblTestID.Text = (_TestAppointment.TestID == -1) ? "Not taken yet" : _TestAppointment.TestID.ToString();
        }



    }
}
