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

namespace DVLD.People
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int personId)
        {
            Action<int>Handler = OnPersonSelected;
            if (Handler != null)
            {
                Handler(personId);
            }
        }

        private bool _ShowAddPerson = true;

        public bool ShowAddPerson
        {
            set { _ShowAddPerson = value; btnAddNewPerson.Visible = _ShowAddPerson; }
            get { return _ShowAddPerson; }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            set { _FilterEnabled = value; gbFilter.Enabled = _FilterEnabled; }
            get { return _FilterEnabled; }
        }
        
        private int _PersonId = -1;

        public int PersonId
        {
            get { return ctrlPersonCard2.PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return (ctrlPersonCard2.SelectedPersonInfo); }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private void FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard2.LoadPersonInfo(int.Parse(txtFilterValue.Text.Trim()));
                    break;

                case "National No.":
                    ctrlPersonCard2.LoadPersonInfo(txtFilterValue.Text);
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
            {
                OnPersonSelected(PersonId); 
            }

        }

        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            FindNow();
        }

        private void btnSearchForPerson_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FindNow();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSearchForPerson.PerformClick();
            }

            if (cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
                errorProvider1.SetError(txtFilterValue, "This field is required");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            FrmAddUpdatePerson frm = new FrmAddUpdatePerson();
            frm.DataBack += DataBackEvent;    
            frm.ShowDialog();
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard2.LoadPersonInfo(PersonID);
        }

        private void gbFilter_Enter(object sender, EventArgs e)
        {

        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

    }
}
