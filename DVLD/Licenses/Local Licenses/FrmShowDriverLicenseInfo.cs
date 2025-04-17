using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses
{
    public partial class FrmShowDriverLicenseInfo : Form
    {
        private int _LicensesID;
        public FrmShowDriverLicenseInfo(int licensesID)
        {
            InitializeComponent();
            _LicensesID = licensesID;
        }

        private void FrmShowDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfo1.LoadInfo(_LicensesID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
