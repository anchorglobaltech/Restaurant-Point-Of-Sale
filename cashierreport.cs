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
    public partial class cashierreport : Form
    {
       
            SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
        ListViewItem lst;

        double listAmount = 0;

     
        public cashierreport()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
        }
        public void pass(string user)
        {

            lblUser.Text = user;
          


        }
        //public void getCashier()
        //{

        //    try
        //    {


        //        string sql2 = @"Select * from tblLogin";
        //        cm = new SqlCommand(sql2, cn);
        //        dr = cm.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            lblUser.Items.Add(dr[5].ToString());

        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}

        private void cashierreport_Load(object sender, EventArgs e)
        {

            cn.Close();
            cn.Open();
           // getCashier();
            getData3();
            timer1.Start();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");

        }
        public void getData2()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("Product ID", 120);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("Date", 207);

            string sql = @"Select * from tblCashierRecord Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())

            {
                lst = listView1.Items.Add(dr[0].ToString());
                lst.SubItems.Add(dr[1].ToString());
                lst.SubItems.Add(dr[2].ToString());
                lst.SubItems.Add(dr[3].ToString());
                lst.SubItems.Add(dr[4].ToString());
                lst.SubItems.Add(dr[5].ToString());
               
                lst.SubItems.Add(dr[8].ToString());
               
            }
            dr.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");

        }
        //public void getCashierResult()
        //{
        //    try
        //    {

        //        listView1.Items.Clear();
        //        listView1.Columns.Clear();

        //        listView1.Columns.Add("Cashier", 120);
        //        listView1.Columns.Add("Product ID", 120);
        //        listView1.Columns.Add("Product Name ", 120);
        //        listView1.Columns.Add("Unit Price", 120);
        //        listView1.Columns.Add("Quantity", 120);
        //        listView1.Columns.Add("Total Amount", 120);
        //        listView1.Columns.Add("Type", 120);
        //        listView1.Columns.Add("Size", 80);
        //        listView1.Columns.Add("Brand", 100);
        //        listView1.Columns.Add("Date", 207);

        //        string sql = @"Select * from tblCashierRecord Order by DateTime DESC";
        //        cm = new SqlCommand(sql, cn);
        //        dr = cm.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            lst = listView1.Items.Add(dr[0].ToString());
        //            lst.SubItems.Add(dr[1].ToString());
        //            lst.SubItems.Add(dr[2].ToString());
        //            lst.SubItems.Add(dr[3].ToString());
        //            lst.SubItems.Add(dr[4].ToString());
        //            lst.SubItems.Add(dr[5].ToString());
        //            lst.SubItems.Add(dr[6].ToString());
        //            lst.SubItems.Add(dr[7].ToString());
        //            lst.SubItems.Add(dr[8].ToString());
        //            lst.SubItems.Add(dr[9].ToString());
        //        }
        //        dr.Close();

        //        double value = 0;
        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            value += double.Parse(listView1.Items[i].SubItems[5].Text);
        //        }
        //        lblTotal.Text = value.ToString("#,###,##0.00");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        public void getData()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("Product ID", 120);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("TransactionID", 207);
            listView1.Columns.Add("Date", 207);

            string sql = @"Select * from tblCashierRecord where Descrip like '" + txtSearch.Text + "%' and DateTime  >= '" + dateTimePicker1.Value.ToShortDateString() + "' and DateTime  <='" + dateTimePicker2.Value.ToShortDateString() + "' Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
            SqlDataReader dr1 = cm.ExecuteReader();
            while (dr1.Read())
            {

                lst = listView1.Items.Add(dr1[1].ToString());
                lst.SubItems.Add(dr1[2].ToString());
                lst.SubItems.Add(dr1[3].ToString());
                lst.SubItems.Add(dr1[4].ToString());
                lst.SubItems.Add(dr1[5].ToString());
                lst.SubItems.Add(dr1[6].ToString());
                lst.SubItems.Add(dr1[8].ToString());
                string getdate = Convert.ToDateTime(dr1[7].ToString()).ToShortDateString();
                lst.SubItems.Add(getdate);
        
            }
            dr1.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");


        }

        public void getData3()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("Product ID", 120);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
    
            listView1.Columns.Add("TransactionId", 80);
            listView1.Columns.Add("Date", 207);

            cn.Close();
            cn.Open();
            string sql = @"Select * from tblCashierRecord where Cashier like '" + lblUser.Text + "%' and DateTime  >= '" + dateTimePicker1.Value.ToShortDateString() + "' and DateTime  <='" + dateTimePicker2.Value.ToShortDateString() + "' Order by id desc";
            cm = new SqlCommand(sql, cn);
            SqlDataReader dr1 = cm.ExecuteReader();
            while (dr1.Read())
            {
                lst = listView1.Items.Add(dr1[1].ToString());
                lst.SubItems.Add(dr1[2].ToString());
                lst.SubItems.Add(dr1[3].ToString());
                lst.SubItems.Add(dr1[4].ToString());
                lst.SubItems.Add(dr1[5].ToString());
                lst.SubItems.Add(dr1[6].ToString());
                lst.SubItems.Add(dr1[8].ToString());
                string getdate = Convert.ToDateTime(dr1[7].ToString()).ToShortDateString();
                lst.SubItems.Add(getdate);
                //DateTime mydate = new DateTime();
                //mydate =DateTime.Now;
                //lst.SubItems.Add(mydate.ToShortDateString()).ToString();
            }
            dr1.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            double t = Convert.ToDouble(listView1.FocusedItem.SubItems[5].Text);
            listAmount = t;
            lblTempID.Text = listView1.FocusedItem.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string format = "MM-dd-yyy";
            lblTimer.Text = time.ToString(format);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void lblUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            getData3();

            if (lblUser.Text == "-SELECT-")
            {

                getData2();
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Close();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            getData3();
        }

        private void RemoveAll_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Do you really want to delete ALL records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                DelTrail();
                deleteAll();
            }
        }

        public void DelTrail()
        {

            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@TransacType,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblTimer.Text);
                cm.Parameters.AddWithValue("@TransacType", "Deletion");
                cm.Parameters.AddWithValue("@Description", "All data has been DELETED from Users Record!");
                cm.Parameters.AddWithValue("@Authority", "Admin");

                cm.ExecuteNonQuery();

            }
            catch (SqlException l)
            {
                MessageBox.Show(l.Message);
            }
        }
        public void deleteAll()
        {
            try
            {
                foreach (ListViewItem del in listView1.Items)
                {
                    //  listView1.FocusedItem.Remove();
                    string del1 = "delete from tblCashierRecord";
                    cm = new SqlCommand(del1, cn);
                    cm.ExecuteNonQuery();
                }
                lblTotal.Text = "0.00";
                getData2();

            }
            catch (Exception)
            {
                MessageBox.Show("No Items To Remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
           DateTime date1 =Convert.ToDateTime(dateTimePicker1.Value.ToShortDateString());
           DateTime date2 = Convert.ToDateTime(dateTimePicker2.Value.ToShortDateString());
           cn.Close();
            cn.Open();
            listView1.Items.Clear();
            string sql = "Select * from tblCashierRecord where Cashier like '" + lblUser.Text + "' and DateTime  >= '" + date1 + "' and DateTime  <='" + date2 + "'";
            cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                cn.Close();
                cn.Open();
                sql = "Select * from tblCashierRecord where Cashier like '" + lblUser.Text + "' and DateTime  >= '" + date1 + "' and DateTime  <= '" + date2 + "'";
                cm = new SqlCommand(sql, cn);
              SqlDataReader  dr1 = cm.ExecuteReader();
                while (dr1.Read())
                {

                    lst = listView1.Items.Add(dr1[1].ToString());
                    lst.SubItems.Add(dr1[2].ToString());
                    lst.SubItems.Add(dr1[3].ToString());
                    lst.SubItems.Add(dr1[4].ToString());
                    lst.SubItems.Add(dr1[5].ToString());
                    lst.SubItems.Add(dr1[6].ToString());
                    lst.SubItems.Add(dr1[8].ToString());
                    string getdate = Convert.ToDateTime(dr1[7].ToString()).ToShortDateString();
                    lst.SubItems.Add(getdate);

                }
                dr1.Close();

                double value = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    value += double.Parse(listView1.Items[i].SubItems[5].Text);
                }
                lblTotal.Text = value.ToString("#,###,##0.00");

            }
            else
            {
                lblTotal.Text = "0.00";
             
            }
            dr.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       
    }
}
