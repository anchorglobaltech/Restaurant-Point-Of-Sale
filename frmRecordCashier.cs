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
    public partial class frmRecordCashier : Form
    {

        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
        ListViewItem lst;
        
        double listAmount = 0;

     

        public frmRecordCashier()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
         
        }
        public void getCashier()
        {

            try
            {


                string sql2 = @"Select * from Login where UserType='Cashier'";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cboSort.Items.Add(dr[1].ToString());

                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void frmRecordCashier_Load(object sender, EventArgs e)
        {
                      
            cn.Open();
            getCashier();
           getData2();
           timer1.Start();
           EncDec.GetCategory(cbocatsearch);
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
                listView1.Columns.Add("Product ID", 120);
                listView1.Columns.Add("Cashier", 120);
                listView1.Columns.Add("Product Name ", 120);
                listView1.Columns.Add("Unit Price", 120);
                listView1.Columns.Add("Quantity", 120);
                listView1.Columns.Add("Total Amount", 120);
                listView1.Columns.Add("TransactionID", 100);
                listView1.Columns.Add("Date", 120);
                listView1.Columns.Add("Time", 150);
                cn.Close();
                cn.Open();
                string sql = @"Select * from tblCashierRecord Where DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())

                {
                    lst = listView1.Items.Add(dr[2].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[3].ToString());
                    lst.SubItems.Add(dr[4].ToString());
                    lst.SubItems.Add(dr[5].ToString());
                    lst.SubItems.Add(dr[6].ToString());
                    lst.SubItems.Add(dr[8].ToString());
                    lst.SubItems.Add(dr[7].ToString());
                    lst.SubItems.Add(dr["Time"].ToString());
                  
                }
                dr.Close();

                double value = 0;
                double value2 = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {

                    value += double.Parse(listView1.Items[i].SubItems[5].Text);
                    value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
                }
                lblTotal.Text = value.ToString("#,###,##0.00");
                lbltqy.Text = value2.ToString();
        }

          public void getData()
          {
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("Product ID", 120);
            listView1.Columns.Add("Cashier", 120);
            listView1.Columns.Add("Product Name ", 120);
            listView1.Columns.Add("Unit Price", 120);
            listView1.Columns.Add("Quantity", 120);
            listView1.Columns.Add("Total Amount", 120);
            listView1.Columns.Add("TransactionID", 100);
            listView1.Columns.Add("Date", 120);
            listView1.Columns.Add("Time", 150);
            string sql = @"Select * from tblCashierRecord where Descrip like '" + txtSearch.Text + "%' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by DateTime DESC";
            cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lst = listView1.Items.Add(dr[2].ToString());
                lst.SubItems.Add(dr[1].ToString());
                lst.SubItems.Add(dr[3].ToString());
                lst.SubItems.Add(dr[4].ToString());
                lst.SubItems.Add(dr[5].ToString());
                lst.SubItems.Add(dr[6].ToString());
                lst.SubItems.Add(dr[8].ToString());
                lst.SubItems.Add(dr[7].ToString());
                lst.SubItems.Add(dr["Time"].ToString());
            }
            dr.Close();

                double value = 0;
                double value2 = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {

                    value += double.Parse(listView1.Items[i].SubItems[5].Text);
                    value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
                }
                lblTotal.Text = value.ToString("#,###,##0.00");
                lbltqy.Text = value2.ToString();
          }
          public void getData4()
          {
              listView1.Items.Clear();
              listView1.Columns.Clear();
              listView1.Columns.Add("Product ID", 120);
              listView1.Columns.Add("Cashier", 120);
              listView1.Columns.Add("Product Name ", 120);
              listView1.Columns.Add("Unit Price", 120);
              listView1.Columns.Add("Quantity", 120);
              listView1.Columns.Add("Total Amount", 120);
              listView1.Columns.Add("TransactionID", 100);
              listView1.Columns.Add("Date", 120);
              listView1.Columns.Add("Date", 150);
              string sql = @"Select * from tblCashierRecord where Descrip like '" + txtSearch.Text + "%' and Cashier='"+cboSort.Text+"' and categories ='"+cbocatsearch.Text+"' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by DateTime DESC";
              cm = new SqlCommand(sql, cn);
              dr = cm.ExecuteReader();
              while (dr.Read())
              {
                  lst = listView1.Items.Add(dr[2].ToString());
                  lst.SubItems.Add(dr[1].ToString());
                  lst.SubItems.Add(dr[3].ToString());
                  lst.SubItems.Add(dr[4].ToString());
                  lst.SubItems.Add(dr[5].ToString());
                  lst.SubItems.Add(dr[6].ToString());
                  lst.SubItems.Add(dr[8].ToString());
                  lst.SubItems.Add(dr[7].ToString());
                  lst.SubItems.Add(dr["Time"].ToString());
              }
              dr.Close();

              double value = 0;
              double value2 = 0;
              for (int i = 0; i < listView1.Items.Count; i++)
              {

                  value += double.Parse(listView1.Items[i].SubItems[5].Text);
                  value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
              }
              lblTotal.Text = value.ToString("#,###,##0.00");
              lbltqy.Text = value2.ToString();

          }

          public void getData5()
          {


              listView1.Items.Clear();
              listView1.Columns.Clear();
              listView1.Columns.Add("Product ID", 120);
              listView1.Columns.Add("Cashier", 120);
              listView1.Columns.Add("Product Name ", 120);
              listView1.Columns.Add("Unit Price", 120);
              listView1.Columns.Add("Quantity", 120);
              listView1.Columns.Add("Total Amount", 120);
              listView1.Columns.Add("TransactionID", 100);
              listView1.Columns.Add("Date", 120);
              listView1.Columns.Add("Time", 150);
              string sql = @"Select * from tblCashierRecord where Descrip like '" + txtSearch.Text + "%' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by DateTime DESC";
              cm = new SqlCommand(sql, cn);
              dr = cm.ExecuteReader();
              while (dr.Read())
              {
                  lst = listView1.Items.Add(dr[2].ToString());
                  lst.SubItems.Add(dr[1].ToString());
                  lst.SubItems.Add(dr[3].ToString());
                  lst.SubItems.Add(dr[4].ToString());
                  lst.SubItems.Add(dr[5].ToString());
                  lst.SubItems.Add(dr[6].ToString());
                  lst.SubItems.Add(dr[8].ToString());
                  lst.SubItems.Add(dr[7].ToString());
                  lst.SubItems.Add(dr["Time"].ToString());

              }
              dr.Close();

              double value = 0;
              double value2 = 0;
              for (int i = 0; i < listView1.Items.Count; i++)
              {

                  value += double.Parse(listView1.Items[i].SubItems[5].Text);
                  value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
              }
              lblTotal.Text = value.ToString("#,###,##0.00");
              lbltqy.Text = value2.ToString();

          }

          public void getData3()
          {
                  listView1.Items.Clear();
                  listView1.Columns.Clear();
                  listView1.Columns.Add("Product ID", 120);
                  listView1.Columns.Add("Cashier", 120);
                  listView1.Columns.Add("Product Name ", 120);
                  listView1.Columns.Add("Unit Price", 120);
                  listView1.Columns.Add("Quantity", 120);
                  listView1.Columns.Add("Total Amount", 120);
                  listView1.Columns.Add("TransactionID", 100);
                  listView1.Columns.Add("Date", 120);
                  listView1.Columns.Add("Time", 150);
                  cn.Close();
                  cn.Open();
                  string sql = @"Select * from tblCashierRecord where Cashier like '" + cboSort.Text + "%' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by ID DESC";
                  cm = new SqlCommand(sql, cn);
                  SqlDataReader dr1 = cm.ExecuteReader();
                  while (dr1.Read())
                  {
                      lst = listView1.Items.Add(dr1[2].ToString());
                      lst.SubItems.Add(dr1[1].ToString());
                      lst.SubItems.Add(dr1[3].ToString());
                      lst.SubItems.Add(dr1[4].ToString());
                      lst.SubItems.Add(dr1[5].ToString());
                      lst.SubItems.Add(dr1[6].ToString());
                      lst.SubItems.Add(dr1[8].ToString());
                      lst.SubItems.Add(dr1[7].ToString());
                      lst.SubItems.Add(dr1["Time"].ToString());
                  }
                  dr1.Close();

                  double value = 0;
                  double value2 = 0;
                  for (int i = 0; i < listView1.Items.Count; i++)
                  {

                      value += double.Parse(listView1.Items[i].SubItems[5].Text);
                      value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
                  }
                  lblTotal.Text = value.ToString("#,###,##0.00");
                  lbltqy.Text = value2.ToString();
            
          }


          public void getDataCategory()
          {
             
              cn.Close();
              cn.Open();
              listView1.Items.Clear();
              string sql = @"Select * from tblCashierRecord where Cashier like '" + cboSort.Text + "%' and Categories='"+cbocatsearch.Text+"' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by ID DESC";
              cm = new SqlCommand(sql, cn);
              SqlDataReader dr1 = cm.ExecuteReader();
              while (dr1.Read())
              {
                  lst = listView1.Items.Add(dr1[2].ToString());
                  lst.SubItems.Add(dr1[1].ToString());
                  lst.SubItems.Add(dr1[3].ToString());
                  lst.SubItems.Add(dr1[4].ToString());
                  lst.SubItems.Add(dr1[5].ToString());
                  lst.SubItems.Add(dr1[6].ToString());
                  lst.SubItems.Add(dr1[8].ToString());
                  lst.SubItems.Add(dr1[7].ToString());
                  lst.SubItems.Add(dr1["Time"].ToString());
              }
              dr1.Close();

              double value = 0;
              double value2 = 0;
              for (int i = 0; i < listView1.Items.Count; i++)
              {

                  value += double.Parse(listView1.Items[i].SubItems[5].Text);
                  value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
              }
              lblTotal.Text = value.ToString("#,###,##0.00");
              lbltqy.Text = value2.ToString();

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
              if (cboSort.Text == "-SELECT-")
              {
                  getData5();
              }
              else
              {
                  getData4();
              }
          }

          private void cboSort_SelectedIndexChanged(object sender, EventArgs e)
          {            
              getData3();

              if(cboSort.Text == "-SELECT-")
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
              getData2();
              cboSort.SelectedIndex = 0;

              if(cboSort.Text == "-SELECT-")
              {
                  getData2();
              }
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
                  lbltqy.Text = "0";
                  getData2();

              }
              catch (Exception)
              {
                  MessageBox.Show("No Items To Remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }


          }

          private void button2_Click(object sender, EventArgs e)
          {
              if (cboSort.Text == "")
              {
                  MessageBox.Show("Select Cashier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  cboSort.Focus();
                  return;
                  
              }
              listView1.Items.Clear();
             
                  cn.Close();
                  cn.Open();
           string sql = "Select * from tblCashierRecord where Cashier like '" + cboSort.Text + "' and DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'";
                  cm = new SqlCommand(sql, cn);
                  dr = cm.ExecuteReader();

                  while (dr.Read())
                  {
                      lst = listView1.Items.Add(dr[2].ToString());
                      lst.SubItems.Add(dr[1].ToString());
                      lst.SubItems.Add(dr[3].ToString());
                      lst.SubItems.Add(dr[4].ToString());
                      lst.SubItems.Add(dr[5].ToString());
                      lst.SubItems.Add(dr[6].ToString());
                      lst.SubItems.Add(dr[8].ToString());
                      lst.SubItems.Add(dr[7].ToString());
                      lst.SubItems.Add(dr["Time"].ToString());
                  }

                 
                  if(listView1.Items.Count >=1)
                  {
                     
                      double value = 0;
                      double value2 = 0;
                      for (int i = 0; i < listView1.Items.Count; i++)
                      {

                          value += double.Parse(listView1.Items[i].SubItems[5].Text);
                          value2 += double.Parse(listView1.Items[i].SubItems[4].Text);
                      }
                      lblTotal.Text = value.ToString("#,###,##0.00");
                      lbltqy.Text = value2.ToString();

              }
              else
              {
                  lblTotal.Text = "0.00";
                  lbltqy.Text = "0";
                
              }
              dr.Close();
          }
          
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (cboSort.Text == "" || cboSort.Text == "-SELECT-")
            {
                MessageBox.Show("Please Select Cashier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string cashier = cboSort.Text;
           Form1 purchased = new Form1();
            purchased.pass(cashier, dateTimePicker1.Value, dateTimePicker2.Value);
          //purchased.LoadData("Select * from tblCashierRecord where DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'");
            purchased.ShowDialog();
        }

       
        private void cbocatsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (cbocatsearch.Text == "-Select-")
            {

                getData2();
            }
            getDataCategory();

        }
    }
}
