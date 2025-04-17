using DVLD.Users;
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

namespace DVLD
{
    public partial class FrmListUsers : Form
    {
        private static DataTable _dtUsers;
        public FrmListUsers()
        {
            InitializeComponent();
        }


        private void FrmListUsers_Load(object sender, EventArgs e)
        {

            _dtUsers = clsUser.GetAllUsers();
            dgvUsersList.DefaultCellStyle.Font = new Font("arial", 12);
            dgvUsersList.AllowUserToAddRows = false;
            dgvUsersList.DataSource = _dtUsers;
            lblRecordsCount.Text = dgvUsersList.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;

            if (dgvUsersList.RowCount > 0)
            {
                dgvUsersList.Columns[0].HeaderText = "UserID";
                dgvUsersList.Columns[0].Width = 150;

                dgvUsersList.Columns[1].HeaderText = "PersonID";
                dgvUsersList.Columns[1].Width = 150;

                dgvUsersList.Columns[2].HeaderText = "FullName";
                dgvUsersList.Columns[2].Width = 280;

                dgvUsersList.Columns[3].HeaderText = "Username";
                dgvUsersList.Columns[3].Width = 280;

                dgvUsersList.Columns[4].HeaderText = "IsActive";
                dgvUsersList.Columns[4].Width = 140;

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilterBy.Text == "Is Active")
            {
                cbIsActive.Visible = true;
                txtFilterValue.Visible = false;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;
                _dtUsers.DefaultView.RowFilter = "";
            }


            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new FrmAddUpdateUser();
            frm.ShowDialog();
            FrmListUsers_Load(null, null);
        }

        private void ShowDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmShowUserDetails((int)dgvUsersList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListUsers_Load(null, null);
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FrmAddUpdateUser();
            form.ShowDialog();
            FrmListUsers_Load(null, null);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implented yet", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implented yet", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmAddUpdateUser((int)dgvUsersList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListUsers_Load(null, null);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = string.Empty;
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Full Name":
                    FilterColumn = "Fullname";
                    break;
                case "Username":
                    FilterColumn = "Username";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsersList.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "Fullname" && FilterColumn != "Username")
            {
                _dtUsers.DefaultView.RowFilter = string.Format($"[{FilterColumn}] = {txtFilterValue.Text.Trim()}");
            }
            else
            {
                _dtUsers.DefaultView.RowFilter = string.Format($"[{FilterColumn}] LIKE '{txtFilterValue.Text.Trim()}%'");
            }

            lblRecordsCount.Text = dgvUsersList.Rows.Count.ToString();  

        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0"; 
                    break;
            }

            if (FilterValue == "All")
            {
                _dtUsers.DefaultView.RowFilter = "";
            }
            else
            {
                _dtUsers.DefaultView.RowFilter = string.Format($"[{FilterColumn}] = {FilterValue}");
            }


        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmChangePassword((int)dgvUsersList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure uou want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsUser.DeleteUser((int)dgvUsersList.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User deleted successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmListUsers_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Error: User was not deleted successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
