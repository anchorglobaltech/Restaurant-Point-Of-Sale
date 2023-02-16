using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace UniqueRestaurant
{
    public partial class FrmAdminMenu : Form
    {
        SqlCommand cm;
        SqlConnection cn;

        Frmlogin login = new Frmlogin();
        //   string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";

        public FrmAdminMenu()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()); 
            cn.Open();
            
            timer1.Start();
        }
        public void pass(string user, string Time)
        {

            lblUser.Text = user;
            lblTimeLoggedIn.Text = Time;


        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //AdminView AdminSearch = new AdminView();
            //AdminSearch.Show();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Frmlogin frm5 = new Frmlogin();
            frm5.Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //////FrmUpdate frm8 = new FrmUpdate();
            //////frm8.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //frmAddProduct frm11 = new frmAddProduct();
            //frm11.Show();
        }

        private void btnViewRecords_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //frmRecordSales frmRecord = new frmRecordSales();
            //frmRecord.Show();
        }
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //AdminView AdminSearch = new AdminView();
            //AdminSearch.Show();
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

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //frmRecordSales frmRecord = new frmRecordSales();
            //frmRecord.Show();
        }



        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //frmRecordSales frmRS = new frmRecordSales();
            //frmRS.Show();
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateAndDeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //FrmUpdate frm8 = new FrmUpdate();
            //frm8.Show();
        }

        private void addToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            //this.Dispose();
            //frmAddProduct frmAP = new frmAddProduct();
            //frmAP.Show();
        }

        private void userLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

     
       

        private void requestOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  frmCriticalItems frmcrit = new frmCriticalItems();
            // this.Dispose();
            //  frmcrit.Show();
        }

        private void receiveItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void criticalItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //frmCriticalItems frmC = new frmCriticalItems();
            //frmC.Show();
        }

        private void newItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //ReceiveOrder frmRE = new ReceiveOrder();
            //frmRE.Show();
        }

        private void oldItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //frmReceiveCrit frmInvent = new frmReceiveCrit();
            //frmInvent.Show();
        }

        private void logTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
        private void trailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logTrailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //this.Dispose();
            //LoginTrail LT = new LoginTrail();
            //LT.Show();
        }

        private void auditTrailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //this.Dispose();
            //AuditTrail AT = new AuditTrail();
            //AT.Show();
        }

        private void trailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //    this.Dispose();
            //    frmUserLog frmUL = new frmUserLog();
            //    frmUL.Show();
        }

        private void FrmAdminMenu_Load(object sender, EventArgs e)
        {
            
        }

        private void restockToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manufacturerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            //frmAddManufac frmMF = new frmAddManufac();
            //frmMF.Show();
        }

        private void expensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Expenses exp = new Expenses();
            //exp.ShowDialog();

        }

        private void bankRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BankDet frm = new BankDet();
            //frm.ShowDialog();
        }

        private void changeAdminPAsswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Changepassword pwd = new Changepassword();
            //pwd.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            FrmRegister frmRegis = new FrmRegister();
            
            frmRegis.ShowDialog();
        }

        private void manageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmManageUser frmManage = new frmManageUser();
            frmManage.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Changepassword frmpass = new Changepassword();
          
            frmpass.ShowDialog();
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

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Expenses rec = new Expenses();
            rec.ShowDialog();
        }

        private void bankRecordsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            BankDet rec = new BankDet();
            rec.ShowDialog();
        }

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            Expenses rec = new Expenses();
            rec.ShowDialog();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Frmcategory rec = new Frmcategory();
            rec.ShowDialog();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            AddProduct rec = new AddProduct();
            rec.ShowDialog();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            LoginTrail rec = new LoginTrail();
            rec.ShowDialog();
        }

        private void audTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditTrail rec = new AuditTrail();
            rec.ShowDialog();
        }

        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            FrmUpdateStock rec = new FrmUpdateStock();
            rec.ShowDialog();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            StockRecords rec = new StockRecords();
            rec.ShowDialog();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            mycategory rec = new mycategory();
            rec.ShowDialog();
        }

    
    }
}
