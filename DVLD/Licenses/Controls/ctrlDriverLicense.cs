using DVLD.Licenses.Local_Licenses;
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

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicense : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtLocalDrivingLicensesHistory;
        private DataTable _dtInternationalDrivingLicensesHistory;

        public ctrlDriverLicense()
        {
            InitializeComponent();
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.FindByDriverID(_DriverID);

            if (_Driver == null)
            {
                MessageBox.Show($"There is no driver with id [{_DriverID.ToString()}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }

        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if (_Driver == null)
            {
                MessageBox.Show($"There is no driver with id [{PersonID.ToString()}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_Driver != null)
            {
                _DriverID = _Driver.DriverID;
            }

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }

        private void _LoadLocalLicenseInfo()
        {
            _dtLocalDrivingLicensesHistory = clsDriver.GetLicenses(_DriverID);

            dgvLocalLicenses.DataSource = _dtLocalDrivingLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicenses.Rows.Count.ToString();

            if (dgvLocalLicenses.Rows.Count > 0)
            {
                dgvLocalLicenses.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicenses.Columns[0].Width = 110;

                dgvLocalLicenses.Columns[1].HeaderText = "App.ID";
                dgvLocalLicenses.Columns[1].Width = 110;

                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[2].Width = 270;

                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[3].Width = 170;

                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[4].Width = 170;

                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[5].Width = 110;
            }

        }


        private void _LoadInternationalLicenseInfo()
        {
            _dtInternationalDrivingLicensesHistory = clsInternationalLicense.GetDriverInternationalLicenses(_DriverID);

            dgvInternationalLicenses.DataSource = _dtInternationalDrivingLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicenses.Rows.Count.ToString();

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Lic.ID";
                dgvInternationalLicenses.Columns[0].Width = 110;

                dgvInternationalLicenses.Columns[1].HeaderText = "App.ID";
                dgvInternationalLicenses.Columns[1].Width = 110;

                dgvInternationalLicenses.Columns[2].HeaderText = "Class Name";
                dgvInternationalLicenses.Columns[2].Width = 270;

                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[3].Width = 170;

                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[4].Width = 170;

                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[5].Width = 110;
            }
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmShowDriverLicenseInfo frm = new FrmShowDriverLicenseInfo((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void InternationalLicenseHistorytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmShowDriverLicenseInfo frm = new FrmShowDriverLicenseInfo((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        public void Clear()
        {
            _dtLocalDrivingLicensesHistory.Clear();
            _dtInternationalDrivingLicensesHistory.Clear();

        }

    }
}   
