using DVLD.Global_Classes;
using DVLD.People;
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

namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {


        private int _ApplicationID = -1;
        private clsApplication _Application;
        public int ApplicationID { get { return _ApplicationID; } }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.GetApplicationInfoByID(ApplicationID);

            if (_Application == null)
            {
                _ResetApplicationInfo();
                MessageBox.Show($"No Application with ID [{ApplicationID.ToString()}] was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo(); 
        }

        private void _ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[???]";
            lblApplicationStatus.Text = "[???]";
            lblApplicationFees.Text = "[$$$]";
            lblApplicationType.Text = "[???]";
            lblApplicant.Text = "[???]";

            lblDate.Text = "[??/??/????]";
            lblStatusDate.Text = "[??/??/????]";
            lblCreatedBy.Text = "[????]";
        }
        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblApplicationStatus.Text = _Application.StatusText;
            lblApplicationFees.Text = _Application.PaidFees.ToString();
            lblApplicationType.Text = _Application.ApplicationTypeInfo.ApplicationTypeTitle;
            lblApplicant.Text = _Application.ApplicantFullName;

            lblDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_Application.LastStatusDate);
            lblCreatedBy.Text = _Application.CreatedByUserInfo.Username;
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmShowPersonDetails frm = new FrmShowPersonDetails(_Application.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
