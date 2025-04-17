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

namespace DVLD.Applications
{
    public partial class FrmListApplicationTypes : Form
    {
        private DataTable _dtApplicationTypes;

        public FrmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void FrmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvListApplicationTypes.RowTemplate.Height = 30;
            dgvListApplicationTypes.DefaultCellStyle.Font = new Font("arial", 12);
            dgvListApplicationTypes.DataSource = _dtApplicationTypes;
            dgvListApplicationTypes.AllowUserToAddRows = false;
            lblRecordsCount.Text = dgvListApplicationTypes.Rows.Count.ToString();
            if (dgvListApplicationTypes.Columns.Count > 0)
            {
                dgvListApplicationTypes.Columns[0].HeaderText = "ID";
                dgvListApplicationTypes.Columns[0].Width = 130;

                dgvListApplicationTypes.Columns[1].HeaderText = "Application Title";
                dgvListApplicationTypes.Columns[1].Width = 350;

                dgvListApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvListApplicationTypes.Columns[2].Width = 150;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm =new FrmEditApplicationType((int)dgvListApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FrmListApplicationTypes_Load(null, null);
        }
    }
}
