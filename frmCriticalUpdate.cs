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
    
    
    public partial class frmUpdateLimit : Form
    {

        SqlCommand cm;
      
        SqlDataReader dr;
        ListViewItem lst;

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());

        public frmUpdateLimit()
        {
            InitializeComponent();
        }

        private void frmUpdateLimit_Load(object sender, EventArgs e)
        {
          
            cn.Open();
            timer1.Start();
            Search();

        }
        public void getOutStock()
        {
            //displaying data from Database to lstview
            //  try
            // {
            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("Product ID", 100);
            listView2.Columns.Add("Item Name", 150);
            listView2.Columns.Add("Stock", 150);
            listView2.Columns.Add("CritLimit", 150);

            string sql2 = @"Select * from purchasestock where Stock like '" + 0 + "' ";
            cm = new SqlCommand(sql2, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lst = listView2.Items.Add(dr["Stockid"].ToString());//ID
                lst.SubItems.Add(dr["FoodNames"].ToString());//NAme
                lst.SubItems.Add(dr["Stock"].ToString());//Stock
                lst.SubItems.Add(dr["Critical_Limit"].ToString());


                if (Convert.ToInt32(dr["Stock"].ToString()) == 0)
                {

                    lst.ForeColor = Color.Crimson;


                }
                else if (Convert.ToInt32(dr["Stock"].ToString()) < Convert.ToInt32(dr["Critical_Limit"].ToString()))
                {
                    lst.ForeColor = Color.Orange;

                }
            }
            dr.Close();
          
        }
        public void getUnderStock()
        {
           
            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("Product ID", 100);
            listView2.Columns.Add("Product Name", 150);
            listView2.Columns.Add("Stock", 150);
            listView2.Columns.Add("CritLimit", 150);

            string sql2 = @"Select * from purchasestock where DateTime = @dt ";
            cm = new SqlCommand(sql2, cn);
            cm.Parameters.AddWithValue("@dt", DateTime.Now.ToShortDateString());
            dr = cm.ExecuteReader();
            while (dr.Read() == true)
            {

                if ((Convert.ToInt64(dr["Stock"]) <= Convert.ToInt32(dr["Critical_Limit"].ToString())) && Convert.ToInt64(dr["Stock"]) >= 1)
                {

                    lst = listView2.Items.Add(dr["Stockid"].ToString());//ID
                    lst.SubItems.Add(dr["FoodNames"].ToString());//NAme
                    lst.SubItems.Add(dr["Stock"].ToString());//Stock
                    lst.SubItems.Add(dr["Critical_Limit"].ToString());

                    if (Convert.ToInt32(dr["Stock"].ToString()) == 0)
                    {

                        lst.ForeColor = Color.Crimson;


                    }
                    else if (Convert.ToInt32(dr["Stock"].ToString()) < Convert.ToInt32(dr["Critical_Limit"].ToString()))
                    {
                        lst.ForeColor = Color.Orange;

                    }
                }
            }
            dr.Close();
         
        }
        public void Search()
        {
            //displaying data from Database to lstview
          
            listView2.Items.Clear();
            listView2.Columns.Clear();


            listView2.Columns.Add("ID", 100);
            listView2.Columns.Add("Product Name", 150);
            listView2.Columns.Add("Stock", 150);
            listView2.Columns.Add("CritLimit", 150);

            string sql2 = @"Select * from purchasestock where FoodNames like '" + txtSearch.Text + "%' and DateTime = @dt";
            cm = new SqlCommand(sql2, cn);
            cm.Parameters.AddWithValue("@dt", DateTime.Now.ToShortDateString());
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
          
                lst = listView2.Items.Add(dr["Stockid"].ToString());//ID
                lst.SubItems.Add(dr["FoodNames"].ToString());//NAme
                lst.SubItems.Add(dr["Stock"].ToString());//Stock
                lst.SubItems.Add(dr["Critical_Limit"].ToString());

                if (Convert.ToInt32(dr["Stock"].ToString()) == 0)
                {

                    lst.ForeColor = Color.Crimson;


                }
                else if (Convert.ToInt32(dr["Stock"].ToString()) < Convert.ToInt32(dr["Critical_Limit"].ToString()))
                {
                    lst.ForeColor = Color.Orange;

                }
            }
            dr.Close();
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;                    
            lblDate.Text = time.ToString();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblID.Text = listView2.FocusedItem.Text;
            lblName.Text = listView2.FocusedItem.SubItems[1].Text;
            lblLimit.Text = listView2.FocusedItem.SubItems[3].Text;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtNewCritLimit.Text == "")
            {
                MessageBox.Show("Supply Critical Limit");
                return;
               
            }
            try
            {
                if (lblID.Text == "")
                {

                    MessageBox.Show("select listview properly");
                    return;
                }
                string up = @"UPDATE PurchaseStock SET [Critical_Limit]='" + txtNewCritLimit.Text + "' where [StockID]='" + lblID.Text + "'";
                cm = new SqlCommand(up, cn);
                cm.ExecuteNonQuery();

                UpdateTrail();
               
                listView2.Items.Clear();
                Search();
                txtNewCritLimit.Text = "";

                MessageBox.Show("Successfully Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No Items to Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void UpdateTrail()
        {


            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("@Transactype", "Updation");
                cm.Parameters.AddWithValue("@Description", "Item: '" + lblName.Text + "' had its CriticalLimit UPDATED!");
                cm.Parameters.AddWithValue("@Authority", "Admin");

                cm.ExecuteNonQuery();

            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again.!");
                MessageBox.Show(l.Message);
            }
        }

        private void cboItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboItems.Text == "-SELECT-")
            {
                Search();

            }
            else if (cboItems.Text == "Under Stock"){
             
                getUnderStock();
            
            
            }else if(cboItems.Text == "Out of Stock")
            {

                getOutStock();
            
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void txtNewCritLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
