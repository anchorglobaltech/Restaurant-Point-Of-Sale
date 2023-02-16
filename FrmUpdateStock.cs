using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;


namespace UniqueRestaurant
{
    public partial class FrmUpdateStock : Form
    {

        SqlCommand cm;
        SqlDataReader dr;
        // string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        ListViewItem lst;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        public FrmUpdateStock()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        static string stockid = "";
        public void generateID()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            stockid = "STK-" + result;



        }
        public void getData()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("STOCKID", 90);
                listView1.Columns.Add("Item Name", 190);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Stock", 80);
                listView1.Columns.Add("NetPrice", 80);
                listView1.Columns.Add("Date", 100);
                listView1.Columns.Add("Time", 80);
                //listView1.Columns.Add("CritLimit", 90);

                string sql2 = @"Select * from stock where Foodnames like '" + txtSearch.Text + "%' and DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr["STOCKID"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    lst.SubItems.Add(dr["NetPrice"].ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    string tim = dr["Time"].ToString();
                    DateTime time = Convert.ToDateTime(tim).ToLocalTime();
                    lst.SubItems.Add(time.ToShortTimeString());
                  //  lst.SubItems.Add(dr["Critical_Limit"].ToString());
                                      //lst.SubItems.Add(dr[12].ToString());
                 
                     if (listView1.Items.Count >= 1)
                    {
                        double value = 0;
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                            value += double.Parse(listView1.Items[i].SubItems[4].Text);
                        }
                        lblTotal.Text = value.ToString("#,###,##0.00");

                    }
                    else
                    {
                        lblTotal.Text = "0.00";
                        //btnReport.Visible = false;
                    }
               
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void getDatasearch()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("STOCKID", 90);
                listView1.Columns.Add("Item Name", 190);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Stock", 80);
                listView1.Columns.Add("NetPrice", 80);
                listView1.Columns.Add("Date", 100);
                listView1.Columns.Add("Time", 80);
                //listView1.Columns.Add("CritLimit", 90);

                string sql2 = @"Select * from stock where Categories = '" + cbocatsearch.Text + "' and DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr["STOCKID"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    lst.SubItems.Add(dr["NetPrice"].ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    string tim = dr["Time"].ToString();
                    DateTime time = Convert.ToDateTime(tim).ToLocalTime();
                    lst.SubItems.Add(time.ToShortTimeString());
                    //  lst.SubItems.Add(dr["Critical_Limit"].ToString());
                    //lst.SubItems.Add(dr[12].ToString());

                    if (listView1.Items.Count >= 1)
                    {
                        double value = 0;
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                            value += double.Parse(listView1.Items[i].SubItems[4].Text);
                        }
                        lblTotal.Text = value.ToString("#,###,##0.00");

                    }
                    else
                    {
                        lblTotal.Text = "0.00";
                        //btnReport.Visible = false;
                    }

                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        private void FrmUpdateStock_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            timer1.Start();
            getData();
            generateID();
            EncDec.GetCategory(cbocat1);
            EncDec.GetCategory(cbocatsearch);
            try
            {

                cn.Open();
                string sql2 = @"Select * from Product order by FoodNames ASC";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                cbocat.Items.Add(dr["FoodNames"].ToString());
                }
                cn.Close();
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbocat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                cn.Open();
                string sql2 = @"Select * from Product where FoodNames=@a";
                cm = new SqlCommand(sql2, cn);
                cm.Parameters.AddWithValue("@a", cbocat.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                   lblprice.Text= dr["Price"].ToString();
                   
                   
                }
                
                cn.Close();
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static double netpay = 0;
        //string stockitems = string.Empty;
        //string newstockid = string.Empty;
        //string oldstockprice = string.Empty;
           public void Clear()
        {
            lblid.Text = "";
               cbocat.SelectedIndex = 0;
            //stockitems = string.Empty;
            //newstockid = string.Empty;
            //oldstockprice = string.Empty;
            txtStock.Text = "";
            lblprice.Text = "";
            //txtCritLimit.Text = "";
            panel2.Visible = false;
          
            }
           
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ( txtStock.Text=="")
            {
                MessageBox.Show("Fill textboxes to proceed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                
                string stockitems = string.Empty;
                string newstockid = string.Empty;
                string oldstockprice = string.Empty;
                CheckStockDate(out newstockid, out stockitems, out oldstockprice);
                if (string.IsNullOrEmpty(stockitems))
                { 
                    string sql2 = @"INSERT INTO PurchaseStock (Stockid, FoodNames, Price, Stock, Netprice, DateTime, Time, Categories) VALUES(@stockid, @FoodNames,@Price,@Stock, @NetPrice, @DateTime,@Time, @cat)";
                    SqlCommand cm2 = new SqlCommand(sql2, cn);
                    cm2.Parameters.AddWithValue("@stockid", stockid);
                    cm2.Parameters.AddWithValue("@FoodNames", cbocat.Text);
                    cm2.Parameters.AddWithValue("@Price", lblprice.Text);
                    cm2.Parameters.AddWithValue("@Stock", txtStock.Text);
                    cm2.Parameters.AddWithValue("@NetPrice", lbltp.Text);
                    cm2.Parameters.AddWithValue("@DateTime", DateTime.UtcNow.ToShortDateString());
                    cm2.Parameters.AddWithValue("@Time", DateTime.UtcNow.ToString("HH:mm:ss tt"));
                    cm2.Parameters.AddWithValue("@Cat", cbocat1.Text);
                    cn.Open();
                    cm2.ExecuteNonQuery();
                    cn.Close();
                }
                else
                {
                    double oldstock = Convert.ToDouble(stockitems);
                    double oldstockp = Convert.ToDouble(oldstockprice);
                    double newstock = oldstock + Convert.ToDouble(txtStock.Text);
                    double newstockNet = oldstockp + Convert.ToDouble(lbltp.Text);
                    //MessageBox.Show(newstock.ToString());
                    string sql2 = @"Update PurchaseStock Set Stock = @stock, NetPrice=@netprice where stockid=@stockid";
                    SqlCommand cm2 = new SqlCommand(sql2, cn);
                    cm2.Parameters.AddWithValue("@stock", newstock.ToString());
                    cm2.Parameters.AddWithValue("@stockid", newstockid);
                    cm2.Parameters.AddWithValue("@Netprice", newstockNet);
                    cn.Open();
                    cm2.ExecuteNonQuery();
                    cn.Close();
                }
                string sql = @"INSERT INTO Stock (Stockid, FoodNames, Price, Stock, Netprice, DateTime, Time, Categories) VALUES(@stockid, @FoodNames,@Price,@Stock, @NetPrice, @DateTime,@Time,@cat)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@stockid", stockid);
                cm.Parameters.AddWithValue("@FoodNames", cbocat.Text);
                cm.Parameters.AddWithValue("@Price", lblprice.Text);
                cm.Parameters.AddWithValue("@Stock", txtStock.Text);
                cm.Parameters.AddWithValue("@NetPrice", lbltp.Text);
                cm.Parameters.AddWithValue("@DateTime", DateTime.UtcNow.ToShortDateString());
                cm.Parameters.AddWithValue("@Time", DateTime.UtcNow.ToString("HH:mm:ss tt"));
                cm.Parameters.AddWithValue("@Cat", cbocat1.Text); 
                cn.Open();
                cm.ExecuteNonQuery();
                MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
                generateID();
                InsertTrail();
                this.Clear();
                getData();
            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }

        public void InsertTrail()
        {
            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("@Transactype", "Insertion");
                cm.Parameters.AddWithValue("@Description", "Stock: has been sent to updated!");
                cm.Parameters.AddWithValue("@Authority", "Admin");

                cn.Open();
                cm.ExecuteNonQuery();
                //   MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();

            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

      
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MMM-dd-yyy HH:mm:ss";
            //lblTimer.Text = time.ToString(format);
            lblDate.Text = time.ToString();
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            cn.Close();
            cn.Open();
            string sql = "Select * from stock where DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "' order by id desc";
            cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
           
            while (dr.Read())
            {
                lst = listView1.Items.Add(dr["STOCKID"].ToString());
                lst.SubItems.Add(dr["FoodNames"].ToString());
                lst.SubItems.Add(dr["Price"].ToString());
                lst.SubItems.Add(dr["Stock"].ToString());
                lst.SubItems.Add(dr["NetPrice"].ToString());
                lst.SubItems.Add(dr["DateTime"].ToString());
                string tim = dr["Time"].ToString();
                DateTime time = Convert.ToDateTime(tim).ToLocalTime();
                lst.SubItems.Add(time.ToShortTimeString());
            }


            if (listView1.Items.Count >= 1)
            {
                double value = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                   value += double.Parse(listView1.Items[i].SubItems[4].Text);
                }
                lblTotal.Text = value.ToString("#,###,##0.00");

            }
            else
            {
                lblTotal.Text = "0.00";
                //btnReport.Visible = false;
            }
            cn.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }
        public void CheckStockDate(out string newstockid, out string stockitems, out string oldstockprice)
        {
            newstockid = stockitems = oldstockprice = string.Empty;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()))
            {
                cn.Open();
                string sqlStmt = "SELECT * FROM PurchaseStock where FoodNames = @item and DateTime =@datetime";
                using (SqlCommand sqlcom = new SqlCommand(sqlStmt, cn))
                {

                    sqlcom.Parameters.AddWithValue("item", cbocat.Text);
                    sqlcom.Parameters.AddWithValue("DateTime", DateTime.Now.ToShortDateString());
                    using (SqlDataReader dr = sqlcom.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                newstockid = dr["stockId"].ToString();
                                stockitems = dr["stock"].ToString();
                                oldstockprice = dr["NetPrice"].ToString();
                            }  
                        }
                    }
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {
            netpay = 0;
            if (txtStock.Text == "")
            {
                //MessageBox.Show("Fill textboxes to proceed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                netpay = Convert.ToDouble(txtStock.Text) * Convert.ToDouble(lblprice.Text);
                lbltp.Text = netpay.ToString();
            }
        }
        public void Deletestock()
        {

            try
            {
                cn.Open();
                string del = "delete from Stock where StockID='" + lblid.Text + "'";
                cm = new SqlCommand(del, cn); cm.ExecuteNonQuery();
                MessageBox.Show("Successfully Deleted!");
                listView1.FocusedItem.Remove();
                cn.Close();
                Clear();
                getData();

            }
            catch (Exception)
            {
                MessageBox.Show("No item to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblid.Text == "")
            {
                MessageBox.Show("Please Select Item to Delete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Deletestock();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
          lblid.Text=listView1.FocusedItem.SubItems[0].Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
            StockRecords rec = new StockRecords();
            rec.ShowDialog();
           
        }

        private void cbocat1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Clear();
            EncDec.GetFooditems(cbocat1, cbocat);
            panel2.Visible = true;
        }

       
        private void cbocatsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbocatsearch.Text == "-Select-")
            {
                getData();
            }
            else
            {
                getDatasearch();
            }
        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        }

    }
