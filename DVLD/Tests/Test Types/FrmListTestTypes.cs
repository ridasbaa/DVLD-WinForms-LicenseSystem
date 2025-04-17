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
    public partial class FrmListTestTypes : Form
    {
        private DataTable _dtTestTypes;
        public FrmListTestTypes()
        {
            InitializeComponent();
        }

        private void FrmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtTestTypes = clsTestTypes.GetAllTestTypes();
            dgvTestTypes.DefaultCellStyle.Font = new Font("arial", 12);
            dgvTestTypes.RowTemplate.Height = 25;
            dgvTestTypes.AllowUserToAddRows = false;
            dgvTestTypes.DataSource = _dtTestTypes;
            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();

            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "TestTypeID";
                dgvTestTypes.Columns[0].Width = 120;

                dgvTestTypes.Columns[1].HeaderText = "TestTypeTitle";
                dgvTestTypes.Columns[1].Width = 170;

                dgvTestTypes.Columns[2].HeaderText = "TestTypeDescription";
                dgvTestTypes.Columns[2].Width = 320;

                dgvTestTypes.Columns[3].HeaderText = "TestTypeFees";
                dgvTestTypes.Columns[3].Width = 120;
            }



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmEditTestTypes((clsTestTypes.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListTestTypes_Load(null, null);
        }
    }
}
