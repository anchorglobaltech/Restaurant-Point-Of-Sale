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
    public partial class StockRecords : Form
    {
        SqlCommand cm;
        SqlDataReader dr;
        // string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        ListViewItem lst;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        public StockRecords()
        {
            InitializeComponent();
        }

        private void StockRecords_Load(object sender, EventArgs e)
        {
            getData();
            timer1.Start();
            EncDec.GetCategory(cbocatsearch);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MMM-dd-yyy HH:mm:ss";
            //lblTimer.Text = time.ToString(format);
            lblDate.Text = time.ToString();
        }
        public void getData()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("STOCKID", 50);
                listView1.Columns.Add("Item Name", 150);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Stock", 80);
                listView1.Columns.Add("AmountSold", 120);
                listView1.Columns.Add("NetPrice", 120);
                listView1.Columns.Add("ClosedStock", 120);
                listView1.Columns.Add("Date", 100);
                listView1.Columns.Add("Critical Limit", 30);
                string sql2 = @"Select * from purchasestock where Foodnames like '" + txtSearch.Text + "%' and DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                double grossprice=0, price=0, stock=0, closedstock=0, netprice=0;
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    price = Convert.ToDouble(dr["Price"].ToString());
                    stock = Convert.ToDouble(dr["Stock"].ToString());
                    netprice = Convert.ToDouble(dr["NetPrice"].ToString());
                    lst = listView1.Items.Add(dr["STOCKID"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    closedstock = price * stock;
                    grossprice = netprice - closedstock;
                    lst.SubItems.Add(grossprice.ToString());
                    lst.SubItems.Add(dr["NetPrice"].ToString());
                    lst.SubItems.Add(closedstock.ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    lst.SubItems.Add(dr["Critical_Limit"].ToString());
                    //lst.SubItems.Add(dr[12].ToString());

                    if (listView1.Items.Count >= 1)
                    {
                        double value = 0, value2 = 0, value3 = 0;
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                            value += double.Parse(listView1.Items[i].SubItems[4].Text);
                            value2 += double.Parse(listView1.Items[i].SubItems[5].Text);
                            value3 += double.Parse(listView1.Items[i].SubItems[6].Text);

                        }
                        lblsales.Text = value.ToString("#,###,##0.00");
                        lblTotal.Text = value2.ToString("#,###,##0.00");
                        lblbal.Text = value3.ToString("#,###,##0.00");
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

        private void button2_Click(object sender, EventArgs e)
        {
            getData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void getOutStock()
        {
         
             try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("STOCKID", 50);
                listView1.Columns.Add("Item Name", 150);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Stock", 80);
                listView1.Columns.Add("AmountSold", 120);
                listView1.Columns.Add("NetPrice", 120);
                listView1.Columns.Add("ClosedStock", 120);
                listView1.Columns.Add("Date", 100);
                listView1.Columns.Add("Critical Limit", 30);
                string sql2 = @"Select * from purchasestock  where Stock like '" + 0 + "' and DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                double grossprice=0, price=0, stock=0, closedstock=0, netprice=0;
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    price = Convert.ToDouble(dr["Price"].ToString());
                    stock = Convert.ToDouble(dr["Stock"].ToString());
                    netprice = Convert.ToDouble(dr["NetPrice"].ToString());
                    lst = listView1.Items.Add(dr["STOCKID"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    closedstock = price * stock;
                    grossprice = netprice - closedstock;
                    lst.SubItems.Add(grossprice.ToString());
                    lst.SubItems.Add(dr["NetPrice"].ToString());
                    lst.SubItems.Add(closedstock.ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    lst.SubItems.Add(dr["Critical_Limit"].ToString());
                    //lst.SubItems.Add(dr[12].ToString());

                    if (listView1.Items.Count >= 1)
                    {
                        double value = 0, value2 = 0, value3 = 0;
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                            value += double.Parse(listView1.Items[i].SubItems[4].Text);
                            value2 += double.Parse(listView1.Items[i].SubItems[5].Text);
                            value3 += double.Parse(listView1.Items[i].SubItems[6].Text);

                        }
                        lblsales.Text = value.ToString("#,###,##0.00");
                        lblTotal.Text = value2.ToString("#,###,##0.00");
                        lblbal.Text = value3.ToString("#,###,##0.00");
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
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("STOCKID", 50);
                listView1.Columns.Add("Item Name", 150);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Stock", 80);
                listView1.Columns.Add("AmountSold", 120);
                listView1.Columns.Add("NetPrice", 120);
                listView1.Columns.Add("ClosedStock", 120);
                listView1.Columns.Add("Date", 100);
                listView1.Columns.Add("Critical Limit", 30);
                string sql2 = @"Select * from purchasestock where Categories='"+cbocatsearch.Text+"' and DateTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.ToShortDateString() + "'  Order by DateTime DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                double grossprice = 0, price = 0, stock = 0, closedstock = 0, netprice = 0;
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    price = Convert.ToDouble(dr["Price"].ToString());
                    stock = Convert.ToDouble(dr["Stock"].ToString());
                    netprice = Convert.ToDouble(dr["NetPrice"].ToString());
                    lst = listView1.Items.Add(dr["STOCKID"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    closedstock = price * stock;
                    grossprice = netprice - closedstock;
                    lst.SubItems.Add(grossprice.ToString());
                    lst.SubItems.Add(dr["NetPrice"].ToString());
                    lst.SubItems.Add(closedstock.ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    lst.SubItems.Add(dr["Critical_Limit"].ToString());
                    //lst.SubItems.Add(dr[12].ToString());

                    if (listView1.Items.Count >= 1)
                    {
                        double value = 0, value2 = 0, value3 = 0;
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            //MessageBox.Show(listView1.Items[i].SubItems["Netprice"].Text);
                            value += double.Parse(listView1.Items[i].SubItems[4].Text);
                            value2 += double.Parse(listView1.Items[i].SubItems[5].Text);
                            value3 += double.Parse(listView1.Items[i].SubItems[6].Text);

                        }
                        lblsales.Text = value.ToString("#,###,##0.00");
                        lblTotal.Text = value2.ToString("#,###,##0.00");
                        lblbal.Text = value3.ToString("#,###,##0.00");
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
        public void getUnderStock()
        {

        }

        private void cboItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboItems.Text == "Out of Stock")
            {
                getOutStock();
            }
          
            else if (cboItems.Text == "-SELECT-")
            {

                getData();


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            frmUpdateLimit UL = new frmUpdateLimit();
            UL.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
           
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

    }
}
