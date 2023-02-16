using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniqueRestaurant
{
    public partial class frmStart : Form
    {
        public frmStart()
        {
            InitializeComponent();
        }

        private void loginAsCashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frmlogin frmUserLog = new Frmlogin();
            frmUserLog.Show();
        }

        private void loginAsAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //FrmAdminLogin frmAD = new FrmAdminLogin();
            //frmAD.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //FrmRegister frmReg = new FrmRegister();
            //frmReg.Show();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {

        }

    }
}
