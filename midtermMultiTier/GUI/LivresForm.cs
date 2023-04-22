using midtermMultiTier.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace midtermMultiTier.GUI
{
    public partial class LivresForm : Form
    {
        private LivresManager livresManager;
        public LivresForm(LivresManager livresManager)
        {
            this.livresManager = livresManager;
            InitializeComponent();
        }

        public void LinkTableToGridView(DataTable livresTable)
        {
            this.dataGridView1.DataSource = livresTable;
            this.dataGridView1.Refresh();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.livresManager.LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.livresManager.SaveData();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.livresManager.ExitApplication();
        }
    }
}
