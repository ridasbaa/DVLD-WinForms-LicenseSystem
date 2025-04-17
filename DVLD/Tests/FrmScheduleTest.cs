using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class FrmScheduleTest : Form
    {

        int _LocalDrivingLicenseApplicationID = -1;
        clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;
        private int _AppointmentID  = -1; 
        public FrmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestType, int AppointmentID = -1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
            _AppointmentID = AppointmentID;
        } 

        private void FrmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestType;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
