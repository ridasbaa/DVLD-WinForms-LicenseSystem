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
    public partial class FrmAddUpdateUser : Form
    {

        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;

        private int _UserID = -1;
        private clsUser _User;

        public FrmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public FrmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode=enMode.Update;
            _UserID = UserID;   
        }

        private void _LoadData()
        {
            _User = clsUser.FindUserByID(_UserID);

            ctrlPersonCardWithFilter2.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUsername.Text = _User.Username;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter2.LoadPersonInfo(_User.PersonID);
        }

        private void _ResetDefaultValues()
        {
             if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = lblTitle.Text;
                _User = new clsUser();
                tpLoginInfo.Enabled = false;
                btnSave.Enabled = false;
            }

            if (_Mode == enMode.Update)
            {
                lblTitle.Text = "Update User";
                this.Text = lblTitle.Text;
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }


            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = false;

        }

        private void FrmAddNewUser_Load(object sender, EventArgs e)
        {

            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tpLoginInfo"];
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            if (ctrlPersonCardWithFilter2.PersonId != -1)
            {
                if (clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter2.PersonId))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter2.FilterFocus();
                }
                else
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tpLoginInfo"];
                    tpLoginInfo.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter2.FilterFocus();
            }



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.Username = txtUsername.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;
            _User.PersonID = ctrlPersonCardWithFilter2.PersonId;

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = lblTitle.Text;

                MessageBox.Show("Data saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);   
            }
            else
            {
                MessageBox.Show("Data was not saved successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "Password can't be blank");
            }
            else
            {
                errorProvider1.SetError(txtUsername, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = true;    
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password can't be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void FrmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter2.FilterFocus();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;

            try
            {
                if (chkShowPassword.Checked)
                {
                    txtPassword.PasswordChar = '\0';
                    txtConfirmPassword.PasswordChar = '\0';
                }
                else
                {
                    txtPassword.PasswordChar = '*';
                    txtConfirmPassword.PasswordChar = '*';
                }
            }
            finally
            {
                this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            }

        }
    }
}
