using DVLD.Properties;
using DVLD.Tests.Test_Appointments;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class FrmListTestAppointment : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private DataTable _dtTestAppointments;
        private clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;


        public FrmListTestAppointment(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType testType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = testType;
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            
            
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(_LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication.IsThereAnActiveScheduleTest(_TestType))
            {
                MessageBox.Show("Person already have an Appointment for this test, you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FrmScheduleTest frm = new FrmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
            frm.ShowDialog();
            FrmListTestAppointment_Load(null, null);
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    pbTestType.Image = Resources.eye;
                    lblTitle.Text = "Vision Test Appointment";
                    this.Text = lblTitle.Text;
                    break;
                case clsTestTypes.enTestType.WrittenTest:
                    pbTestType.Image = Resources.Written_test;
                    lblTitle.Text = "Written Test Appointment";
                    this.Text = lblTitle.Text;
                    break;
                case clsTestTypes.enTestType.StreetTest:
                    pbTestType.Image = Resources.Street_Test;
                    lblTitle.Text = "Street Test Appointment";
                    this.Text = lblTitle.Text;
                    break;
            }
        }

        private void FrmListTestAppointment_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlDrivingLicenseApplicationInfo2.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);
            _dtTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestType);

            dgvListAppointments.DataSource = _dtTestAppointments;
            dgvListAppointments.AllowUserToAddRows = false;
            lblRecordsCount.Text = dgvListAppointments.Rows.Count.ToString();

            if (dgvListAppointments.Rows.Count > 0)
            {
                dgvListAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvListAppointments.Columns[0].Width = 200;

                dgvListAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvListAppointments.Columns[1].Width = 265;

                dgvListAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvListAppointments.Columns[2].Width = 150;

                dgvListAppointments.Columns[3].HeaderText = "Is Locked";
                dgvListAppointments.Columns[3].Width = 150;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmScheduleTest frm = new FrmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, (int)dgvListAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListTestAppointment_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTakeTest frm = new FrmTakeTest((int)dgvListAppointments.CurrentRow.Cells[0].Value, _TestType);
            frm.ShowDialog();
            FrmListTestAppointment_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
