using DVLD.Applications;
using DVLD.Applications.International_License;
using DVLD.Applications.Local_Driving_License;
using DVLD.Applications.Release_Detained_License;
using DVLD.Applications.Renew_Local_Driving_Licence;
using DVLD.Applications.ReplaceLostOrDamagedLicense;
using DVLD.Drivers;
using DVLD.Global_Classes;
using DVLD.Licenses.Detain_Licenses;
using DVLD.Login;
using DVLD.People;
using DVLD.Tests.Test_Types;
using DVLD.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class FrmMain : Form
    {
        FrmLogin _FrmLogin;

        public FrmMain(FrmLogin frmLogin)
        {
            InitializeComponent();
            _FrmLogin = frmLogin;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmListPeople();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm =  new FrmListUsers();
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _FrmLogin.Show();
            this.Close();
        }

        private void applicationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangePassword frm = new FrmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void currentUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmShowUserDetails(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void manageApplicayionTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmListApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmListTestTypes();
            frm.ShowDialog();   
        }

        private void localDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddUpdateLocalDrivingLicenseApplication frm = new FrmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void localLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListLocalDrivingLicenseApplication frm = new FrmListLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRenewLocalDrivingLicense frm = new FrmRenewLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void replacementnForLostOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReplaceLostOrDamagedLicenseApplication frm = new FrmReplaceLostOrDamagedLicenseApplication();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListDrivers frm = new FrmListDrivers();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDetainLicenseApplication frm = new FrmDetainLicenseApplication();
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReleaseDetainedLicense frm = new FrmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListDetainedLicenses frm = new FrmListDetainedLicenses();
            frm.ShowDialog();
        }

        private void internationalDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewInternationalLicenseApplication frm = new FrmNewInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListInternationalLicenseApplications frm = new FrmListInternationalLicenseApplications();
            frm.ShowDialog();
        }
    }
}
