using DVLD.Global_Classes;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Test_Appointments
{
    public partial class FrmTakeTest : Form
    {
        private int _TestAppointmentID = -1;
        private clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;

        private int _TestID = -1;
        private clsTest _Test;

        public FrmTakeTest(int testAppointmentID, clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _TestAppointmentID = testAppointmentID;
            _TestType = TestType;
        }

        private void FrmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestType = _TestType;
            ctrlScheduledTest1.LoadInfo(_TestAppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            _TestID = ctrlScheduledTest1.TestID;
            if (_TestID != -1)
            {
                _Test = clsTest.Find(_TestID);
                if (_Test.TestResult)
                {
                    rbPass.Checked = true;
                }
                else
                {
                    rbFail.Checked = true;
                }

                txtNotes.Text = _Test.Notes;

                lblUserMessage.Visible = true;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                btnSave.Enabled = false;
                txtNotes.Enabled = false;
            }
            else
            {
                _Test = new clsTest();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want pass this test?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            clsTest Test = new clsTest();

            Test.TestAppointmentID = ctrlScheduledTest1.TestAppointmentID;
            Test.TestResult = rbPass.Checked;
            Test.Notes = txtNotes.Text.Trim();
            Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (Test.Save())
            {
                MessageBox.Show("Data saved successfully", "saved successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data was not saved successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
