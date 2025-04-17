using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class FrmShowUserDetails : Form
    {
        private int _UserID;

        public FrmShowUserDetails(int userID)
        {
            InitializeComponent();
            _UserID = userID;
        }

        private void FrmShowUserDetails_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
    }
}
