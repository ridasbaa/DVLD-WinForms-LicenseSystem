using DVLD.Licenses;
using DVLD.Licenses.Local_Licenses;
using DVLD.Tests;
using DVLD.Tests.Test_Types;
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

namespace DVLD.Applications.Local_Driving_License
{
    public partial class FrmListLocalDrivingLicenseApplication : Form
    {
        private DataTable _dtLocalDrivingLicenseApplications;

        public FrmListLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            Form frm = new FrmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            FrmListLocalDrivingLicenseApplication_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmListLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvListApplications.DataSource = _dtLocalDrivingLicenseApplications;
            dgvListApplications.AllowUserToAddRows = false;
            dgvListApplications.RowTemplate.Height = 27;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvListApplications.Rows.Count.ToString();

            if (dgvListApplications.Columns.Count > 0)
            {
                dgvListApplications.Columns[0].HeaderText = "L.D.L App ID";
                dgvListApplications.Columns[0].Width = 130;

                dgvListApplications.Columns[1].HeaderText = "Driving Class";
                dgvListApplications.Columns[1].Width = 300;

                dgvListApplications.Columns[2].HeaderText = "National No.";
                dgvListApplications.Columns[2].Width = 110;

                dgvListApplications.Columns[3].HeaderText = "Full Name";
                dgvListApplications.Columns[3].Width = 240;

                dgvListApplications.Columns[4].HeaderText = "Application Date";
                dgvListApplications.Columns[4].Width = 220;

                dgvListApplications.Columns[5].HeaderText = "Passed Tests";
                dgvListApplications.Columns[5].Width = 150;

                dgvListApplications.Columns[6].HeaderText = "Status";
                dgvListApplications.Columns[6].Width = 120;
            }


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvListApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            String FilterColumn = "";

            switch(cbFilterBy.Text)
            {
                case "L.D.L.App ID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvListApplications.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format($"[{FilterColumn}] = {txtFilterValue.Text.Trim()}");
            }
            else
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format($"[{FilterColumn}] LIKE '{txtFilterValue.Text.Trim()}%'");
            }

            lblRecordsCount.Text = dgvListApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.App ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddUpdateLocalDrivingLicenseApplication frm = new FrmAddUpdateLocalDrivingLicenseApplication((int)dgvListApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocalDrivingLicenseApplicationInfo frm = new FrmLocalDrivingLicenseApplicationInfo((int)dgvListApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this application", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvListApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(LocalDrivingLicenseApplicationID);

            if (localDrivingLicenseApplication != null)
            {
                if (localDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Has Been Cancelled", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Application Was not Cancelled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                    MessageBox.Show($"No Application Found with ID [{LocalDrivingLicenseApplicationID.ToString()}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            FrmListLocalDrivingLicenseApplication_Load(null, null);
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListTestAppointment frm = new FrmListTestAppointment((int)dgvListApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.VisionTest);
            frm.ShowDialog();
        }

        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListTestAppointment frm = new FrmListTestAppointment((int)dgvListApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.WrittenTest);
            frm.ShowDialog();
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListTestAppointment frm = new FrmListTestAppointment((int)dgvListApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.StreetTest);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this application", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvListApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmListLocalDrivingLicenseApplication_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Application Couldn't be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Error: No Application with ID [{LocalDrivingLicenseApplicationID.ToString()}] was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvListApplications.CurrentRow.Cells[5].Value;
            bool LicenseExist = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExist;

            showLicemseToolStripMenuItem.Enabled = LicenseExist;
            editApplicationToolStripMenuItem.Enabled = !LicenseExist && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            scheduleTestToolStripMenuItem.Enabled = !LicenseExist;

            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.StreetTest);

            scheduleTestToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            if (scheduleTestToolStripMenuItem.Enabled)
            {
                visionTestToolStripMenuItem.Enabled = !PassedVisionTest;
                writtenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;
                streetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }

        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIssueDrivingLicenseForTheFirstTime frm = new FrmIssueDrivingLicenseForTheFirstTime((int)dgvListApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListLocalDrivingLicenseApplication_Load(null, null);

        }

        private void showLicemseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID((int)dgvListApplications.CurrentRow.Cells[0].Value);
           
            int LicenseID = LocalDrivingLicenseApplication.GetActiveLicenseID();

            if (LicenseID != -1)
            {
                FrmShowDriverLicenseInfo frm = new FrmShowDriverLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLApplicationByID(LocalDrivingLicenseApplicationID);

            FrmShowPersonLicenseHistory frm = new FrmShowPersonLicenseHistory(localDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
