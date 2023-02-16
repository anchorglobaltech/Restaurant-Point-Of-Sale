using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Collections;
using System.Configuration;
using System.Globalization;
namespace UniqueRestaurant
{
    public partial class ReprintForm : Form
    {
        SqlCommand cm;
     
     
        SqlConnection cn;
       
        SqlDataReader dr;
        ListViewItem lst;
    
        double listAmount = 0;

      
        public ReprintForm()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        }
        public void pass(string user)
        {
            lblUser.Text = user;
        }
        public void getData3()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("TransactionId", 180);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("Date", 207);
            listView1.Columns.Add("Amount Paid", 120);
            listView1.Columns.Add("Change", 120);

           
            cn.Open();
            string sql = @"Select * from tblCashierPrint where Cashier like '" + lblUser.Text + "%' Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
            SqlDataReader dr1 = cm.ExecuteReader();
            while (dr1.Read())
            {
                lst = listView1.Items.Add(dr1[0].ToString());
                lst.SubItems.Add(dr1[7].ToString());
                lst.SubItems.Add(dr1[2].ToString());
                lst.SubItems.Add(dr1[3].ToString());
                lst.SubItems.Add(dr1[4].ToString());
                lst.SubItems.Add(dr1[5].ToString());
                lst.SubItems.Add(dr1[6].ToString());
                lst.SubItems.Add(dr1[8].ToString());
                lst.SubItems.Add(dr1[9].ToString());
               
         
            }
            dr1.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");
            cn.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Supply Transaction ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearch.Focus();
                return;
            }
            cn.Close();
            cn.Open();
            listView1.Items.Clear();
                
               string sql = "Select * from tblCashierPrint where Cashier like '" + lblUser.Text + "' and Transactionid LIKE '%" + txtSearch.Text + "%' and DateTime like'" + dateTimePicker1.Value + "'";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[7].ToString());
                   
                    lst.SubItems.Add(dr[2].ToString());
                    lst.SubItems.Add(dr[3].ToString());
                    lst.SubItems.Add(dr[4].ToString());
                    lst.SubItems.Add(dr[5].ToString());
                    lst.SubItems.Add(dr[6].ToString());
                    
                    lst.SubItems.Add(dr[8].ToString());
                    lst.SubItems.Add(dr[9].ToString());
                  
                  
                    if (listView1.Items.Count - 1 != 0)
                    {
                        btnSettle.Visible = true;
                    }
                    else
                    {
                        btnSettle.Visible = false;

                    }

                }
                dr.Close();

                double value = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    value += double.Parse(listView1.Items[i].SubItems[5].Text);
                }
                lblTotal.Text = value.ToString("#,###,##0.00");

              }

        private void ReprintForm_Load(object sender, EventArgs e)
        {
         

        
            getData3();
                    
            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");
        }
        public string WriteTxt()
        {
            StringBuilder sb = new StringBuilder();
             
        
            String address = "No. 6 Gidado Street";

            string saleID = listView1.Items[0].SubItems[1].Text;
            //  String item = "item";

            int count = listView1.Items.Count;
           // MessageBox.Show(count.ToString());
            string total = lblTotal.Text;
            string fukuan = listView1.Items[0].SubItems[7].Text;
        
            sb.Append("======================================\r\n");
            sb.Append("date:" + listView1.Items[0].SubItems[6].Text + "\r\n");
            sb.Append("S/N:" + saleID + "\r\n");

            sb.Append("======================================\r\n");
        
            sb.Append(("ITEMS").PadLeft(12));
            sb.Append(("QTY").PadLeft(5));
            sb.Append(("PRICE").PadLeft(8));
            sb.AppendLine(("TOTAL").PadLeft(10));
            sb.Append("======================================\r\n");
            //sb.Append("Items" + "\t\t" + "quantity" + "\t" + "unitprice" + "\t" + "subtotal" + "\r\n");
          
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string pd = "";
                if (listView1.Items[i].SubItems[2].Text.Length > 12)
                {

                    pd = listView1.Items[i].SubItems[2].Text.Substring(0, 10);

                }

                else
                {

                    pd = listView1.Items[i].SubItems[2].Text;
                }
                sb.Append(pd.PadLeft(12));
                sb.Append(listView1.Items[i].SubItems[4].Text.ToString().PadLeft(5));
                sb.Append(listView1.Items[i].SubItems[3].Text.ToString().PadLeft(8));
                sb.AppendLine(listView1.Items[i].SubItems[5].Text.ToString().PadLeft(10));
               
            }
            for (int i = 0; i < count; i++)
            {
                sb.ToString();
            }

            sb.Append("======================================\r\n");
            sb.Append("Ordered Item(s): " + count + "\r\n");
            sb.Append("Total: " + total + "\r\n");
            sb.Append("Payment: Cash" + " " + fukuan+ "\r\n");
            sb.Append("Cash change: " + " " + (listView1.Items[0].SubItems[8].Text) + "\r\n");
            sb.Append("======================================\r\n");
            sb.Append("Address:" + address + "\r\n");
            sb.Append("Emiola, Area, Offa, KW/ST \r\n");
            sb.Append("phone: 08086600507 \r\n");
            sb.Append("Cashier: "+ (listView1.Items[0].SubItems[0].Text) +  "\r\n");
            sb.Append("======================================\r\n");
            sb.Append("Thank you for your Patronage!!!");
            return sb.ToString();
        }
        public void Print()
        {
            PrintService ps = new PrintService();
            //ps.StartPrint("33333","txt");//Print text
        
            ps.StartPrint(WriteTxt(), "txt");
            //  ps.StartPrint(Image.FromFile(Application.StartupPath + "\\2.jpg"), "image");//Print pictures
        }
        private void btnSettle_Click(object sender, EventArgs e)
        {
            Print();
        }
        public void getData()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();


            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("TransactionId", 180);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("Date", 207);
            
            listView1.Columns.Add("Amount Paid", 120);
            listView1.Columns.Add("Change", 120);
            cn.Close();
            cn.Open();
            string sql = @"Select * from tblCashierPrint where Cashier like '" + lblUser.Text + "' and  Transactionid like '" + txtSearch.Text + "%' Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
           SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lst = listView1.Items.Add(dr[0].ToString());
                lst.SubItems.Add(dr[7].ToString());
                         
                lst.SubItems.Add(dr[2].ToString());
                lst.SubItems.Add(dr[3].ToString());
                lst.SubItems.Add(dr[4].ToString());
                lst.SubItems.Add(dr[5].ToString());
                lst.SubItems.Add(dr[6].ToString());
               
                lst.SubItems.Add(dr[8].ToString());
                lst.SubItems.Add(dr[9].ToString());

            }
            dr.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            if (listView1.Items.Count - 1 != 0)
            {
                btnSettle.Visible = true;
            }
            else
            {
                btnSettle.Visible = false;

            }
            lblTotal.Text = value.ToString("#,###,##0.00");


        }
        public void getData4()
        {


            listView1.Items.Clear();
            listView1.Columns.Clear();


            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("TransactionId", 180);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("Date", 207);

            listView1.Columns.Add("Amount Paid", 120);
            listView1.Columns.Add("Change", 120);
            cn.Close();
            cn.Open();
            string sql = @"Select * from tblCashierPrint where Cashier like '" + lblUser.Text + "' and  Transactionid like '" + t + "%' Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lst = listView1.Items.Add(dr[0].ToString());
                lst.SubItems.Add(dr[7].ToString());

                lst.SubItems.Add(dr[2].ToString());
                lst.SubItems.Add(dr[3].ToString());
                lst.SubItems.Add(dr[4].ToString());
                lst.SubItems.Add(dr[5].ToString());
                lst.SubItems.Add(dr[6].ToString());

                lst.SubItems.Add(dr[8].ToString());
                lst.SubItems.Add(dr[9].ToString());

            }
            dr.Close();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            if (listView1.Items.Count != 0)
            {
                btnSettle.Visible = true;
            }
            else
            {
                btnSettle.Visible = false;

            }
            lblTotal.Text = value.ToString("#,###,##0.00");


        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }
        static string t;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            t = listView1.FocusedItem.SubItems[1].Text;
            getData4();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cn.Close();
      
            getData3();

            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[5].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");
        }
    }
}
