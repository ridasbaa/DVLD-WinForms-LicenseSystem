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

namespace DVLD.Users
{
    public partial class ctrlUserCard : UserControl
    {
        private static clsUser _User;
        private int _UserID = -1;

        public int UserID { get { return _UserID; } }
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void _LoadData()
        {
            lblUserID.Text = _User.UserID.ToString();
            lblUsername.Text = _User.Username;
            lblIsActive.Text = (_User.IsActive) ? "Yes" : "No";
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
        }

        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindUserByID(_UserID);

            if (_User == null)
            {
                MessageBox.Show($"No user with ID [{_UserID.ToString()}] was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadData();
        }

    }
}
