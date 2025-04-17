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

namespace DVLD.Tests.Test_Types
{
    public partial class FrmEditTestTypes : Form
    {
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        private clsTestTypes _TestTypes;
        public FrmEditTestTypes(clsTestTypes.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }

        private void txtTestTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeTitle, "Title Cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtTestTypeTitle, null);
            }
        }

        private void txtTestTypeDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeDescription, "Description Cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtTestTypeDescription, null);
                
            }
        }

        private void txtTestTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeFees, "Fees Cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtTestTypeFees, null);

            }
        }

        private void txtTestTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);  
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse over the red icon to see the Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestTypes.TestTypeTitle = txtTestTypeTitle.Text;
            _TestTypes.TestTypeDescription = txtTestTypeDescription.Text;
            _TestTypes.TestTypeFees = Convert.ToSingle(txtTestTypeFees.Text);

            if (_TestTypes.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data was not Saved Successfully", "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmEditTestTypes_Load(object sender, EventArgs e)
        {
            _TestTypes = clsTestTypes.Find(_TestTypeID);

            if (_TestTypes == null)
            {
                MessageBox.Show("No TestType with ID [" + _TestTypeID.ToString() +  "] Was Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTestTypeID.Text = _TestTypes.ID.ToString();
            txtTestTypeTitle.Text = _TestTypes.TestTypeTitle;
            txtTestTypeDescription.Text = _TestTypes.TestTypeDescription;
            txtTestTypeFees.Text = _TestTypes.TestTypeFees.ToString();
        }
    }
}
