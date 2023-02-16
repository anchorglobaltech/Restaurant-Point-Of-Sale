using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace UniqueRestaurant
{
    public partial class ManagerMenu : Form
    {

        SqlCommand cm;
        SqlConnection cn;
        public ManagerMenu()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();

            timer1.Start();
        }
        public void pass(string user)
        {

            lblUser.Text = user;
            //  lblTimeLoggedIn.Text = Time;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MM-dd-yyy HH:mm:ss";
            label2.Text = time.ToString();
            //  lblDate.Text = time.ToString();
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Frmcategory rec = new Frmcategory();
            rec.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmRecordSales sales = new frmRecordSales();
            sales.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmRecordCashier rec = new frmRecordCashier();
            rec.ShowDialog();
        }

        private void bankRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BankDet rec = new BankDet();
            rec.ShowDialog();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Expenses rec = new Expenses();
            rec.ShowDialog();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InsertTrail();
            Frmlogin frmAL = new Frmlogin();
            this.Dispose();
            frmAL.Show();
        }
        public void InsertTrail()
        {

            try
            {
                string sql = @"INSERT INTO tblLogTrail VALUES(@Dater,@Descrip,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", label2.Text);
                cm.Parameters.AddWithValue("@Descrip", "User: " + lblUser.Text + " has successfully logged Out!");
                cm.Parameters.AddWithValue("@Authority", "Admin");


                cm.ExecuteNonQuery();
                //   MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmRegister rec = new FrmRegister();
            rec.ShowDialog();
        }

        private void manageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmManageUser rec = new frmManageUser();
            rec.ShowDialog();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            FrmUpdateStock rec = new FrmUpdateStock();
            rec.ShowDialog();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            StockRecords rec = new StockRecords();
            rec.ShowDialog();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MM-dd-yyy HH:mm:ss";
            label2.Text = time.ToString();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            mycategory rec = new mycategory();
            rec.ShowDialog();
        }

    }
}
