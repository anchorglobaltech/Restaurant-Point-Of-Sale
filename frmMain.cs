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
using Microsoft.VisualBasic;

namespace UniqueRestaurant
{
    public partial class FrmBookStore : Form
    {
        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;

     
      //  string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";



        ListViewItem lst;
        double Total, temp;
        double _totalPayment, _cash, _change;
        double FinStock;

        double CurrQty;
        double CurrStock;
   
        public FrmBookStore()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
          
            timer1.Start();
        }
        public void pass(string user, string Time) {

            lblUser.Text = user;
            lblTimeLoggedIn.Text = Time;
        
        
        }
        public void getData()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Product ID", 0);//0
                listView1.Columns.Add("Items Names", 210);//1
                listView1.Columns.Add("Price", 90);//2
                listView1.Columns.Add("Stock", 80);//3
                listView1.Columns.Add("CritLimit", 0);//4
                listView1.Columns.Add("Categories", 0);
                string sql2 = @"Select * from PurchaseStock where FoodNames like '" + txtSearch.Text + "%' and DateTime='"+DateTime.Now.ToShortDateString()+"'";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr["StockId"].ToString());
                    lst.SubItems.Add(dr["FoodNames"].ToString());
                    lst.SubItems.Add(dr["Price"].ToString());
                    lst.SubItems.Add(dr["Stock"].ToString());
                    lst.SubItems.Add(dr["Critical_Limit"].ToString());
                    lst.SubItems.Add(dr["categories"].ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

  

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string format = "MM-dd-yyy";
            lblTimer.Text = time.ToString();
            lblDate.Text = time.ToString(format);
        }



        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            txtQuantity.Select();
            txtID.Text = listView1.FocusedItem.Text;
            txtDesc.Text = listView1.FocusedItem.SubItems[1].Text;
            txtPrice.Text = listView1.FocusedItem.SubItems[2].Text;
            txtStock.Text = listView1.FocusedItem.SubItems[3].Text;
            lblCrit.Text = listView1.FocusedItem.SubItems[4].Text;
            lblcat.Text = listView1.FocusedItem.SubItems[5].Text;
           // MessageBox.Show(listView1.FocusedItem.SubItems[1].Text);
            txtQuantity.SelectedIndex = 0;
            txtQuantity.Focus();
            txtSum.Text = "0.00";
            /*
            lst = listView2.Items.Add(txtID.Text);
            lst.SubItems.Add(txtDesc.Text);
            lst.SubItems.Add(txtPrice.Text);
            lst.SubItems.Add(txtQuantity.Text);
            lst.SubItems.Add(txtSum.Text);
            lst.SubItems.Add(txtType.Text);
            lst.SubItems.Add(txtSize.Text);
            lst.SubItems.Add(txtBrand.Text);*/

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.getData();
        }

        double tempQty = 0;
        
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  Interaction.InputBox("Edit Quantity", "Title", txtQuantity.Text, -1, -1); 
            //MessageBox.Show(listView2.FocusedItem.SubItems[0].Text);
            //EditQty edit = new EditQty();
            //edit.ShowDialog();

            string tempID = listView2.FocusedItem.SubItems[0].Text;
            txtID2.Text = tempID;
            tempQty = Convert.ToDouble(listView2.FocusedItem.SubItems[3].Text);
       
            txtQty2.Text = Convert.ToString(tempQty);
            double t = Convert.ToDouble(listView2.FocusedItem.SubItems[4].Text);//Converts and passes the total amount from index 4 from lstview2(Cart)
            temp = t;//passed to t
           txtQuantity.Select();

          
        }

        private void FrmBookStore_Load(object sender, EventArgs e)
        {
            panel2.Hide();
            this.getData();
            for (int i = 0; i <= 1000; i++)
            {
                txtQuantity.Items.Add(i.ToString());
            }
            
        }
        public void decrementStock()
        {

            CurrQty = Convert.ToDouble(txtQuantity.Text);
            CurrStock = Convert.ToDouble(listView1.FocusedItem.SubItems[3].Text);

            FinStock = CurrStock - CurrQty;

            listView1.FocusedItem.SubItems[3].Text = FinStock.ToString();

            //--------only for update------------
            txtStock2.Text = FinStock.ToString();
            //-----------txt above is for update--------

            try
            {

                string up = @"UPDATE Purchasestock SET [Stock] =  @Stock where [StockId]='" + txtID.Text + "'";
                cm = new SqlCommand(up, cn);

                cm.Parameters.AddWithValue("@Stock", txtStock2.Text);
                cm.ExecuteNonQuery();


                getData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No Items to Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
           
            if(txtID.Text == "" || txtQuantity.Text == "0")
            {
                MessageBox.Show("Please select product from the list OR input Quantity if empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return;
            }

            double SaleQty = Convert.ToDouble(txtQuantity.Text);
            CurrQty = Convert.ToDouble(txtQuantity.Text);
            CurrStock = Convert.ToDouble(listView1.FocusedItem.SubItems[3].Text);
             if (SaleQty == 0 || CurrStock == 0)
            {
                MessageBox.Show("Quantity or Stock is unavailable!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return;
            }else if (CurrStock < 0){
                return;
            }
             else if (CurrQty > CurrStock)
             {
                 MessageBox.Show("Limited Stock Available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
             }
             else
             {
                  decrementStock();
                 if (listView2.Items.Count == 0)
                 {
                     lst = listView2.Items.Add(txtID.Text);
                     lst.SubItems.Add(txtDesc.Text);
                     lst.SubItems.Add(txtPrice.Text);
                     lst.SubItems.Add(txtQuantity.Text);
                     lst.SubItems.Add(txtSum.Text);
                     lst.SubItems.Add(lblcat.Text);
                     double total = Convert.ToDouble(txtSum.Text);
                     Total += total;
                     txtBill.Text = Total.ToString("#,###,###0.00");
                     txtDesc.Text = "";
                     txtPrice.Text = "";

                     txtQuantity.SelectedIndex = 0; ;
                     txtSum.Text = "";
                     txtID.Text = "";
                     //txtType.Text = "";

                     //txtSize.Text = "";
                     txtStock.Text = "";

                     lblcat.Text = "";

                     txtChange.Text = "0.00";

                     panel2.Show();
                     //  groupBox3.Visible = false;

                     return;
                 }


                 //Updating quantities and Total amount if same ID.
                 for (int j = 0; j <= listView2.Items.Count - 1; j++)
                 {
                     if (listView2.Items[j].SubItems[0].Text == txtID.Text)
                     {

                         listView2.Items[j].SubItems[3].Text = (Convert.ToInt32(listView2.Items[j].SubItems[3].Text) + Convert.ToInt32(txtQuantity.Text)).ToString();
                         listView2.Items[j].SubItems[4].Text = (Convert.ToDouble(listView2.Items[j].SubItems[4].Text) + Convert.ToDouble(txtSum.Text)).ToString("#,###,##0.00");

                         double tempTotal = Convert.ToDouble(txtSum.Text);

                         txtBill.Text = listView2.Items[j].SubItems[4].Text;

                         Total += tempTotal;
                         txtBill.Text = Total.ToString("#,###,###0.00");

                         txtDesc.Text = "";
                         txtPrice.Text = "";

                         txtQuantity.Text = "";
                         txtSum.Text = "";
                         txtID.Text = "";
                         txtPayment.Text = "";

                         //txtBrand.Text = "";
                         txtChange.Text = "0.00";


                         return;
                     }
                 }


                 ListViewItem lst2 = new ListViewItem();
                 lst2 = listView2.Items.Add(txtID.Text);
                 lst2.SubItems.Add(txtDesc.Text);
                 lst2.SubItems.Add(txtPrice.Text);
                 lst2.SubItems.Add(txtQuantity.Text);
                 lst2.SubItems.Add(txtSum.Text);
                 lst2.SubItems.Add(lblcat.Text);


                 double total2 = Convert.ToDouble(txtSum.Text);
                 Total += total2;
                 txtBill.Text = Total.ToString("#,###,###0.00");

                 txtDesc.Text = "";
                 txtPrice.Text = "";


                 txtQuantity.SelectedIndex = 0;
                 txtSum.Text = "";
                 txtID.Text = "";
                 txtPayment.Text = "";


                 txtChange.Text = "0.00";


                 return;
             }

        }
        public void generateID()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            TRANSACTIONID = "UNQ:" + result;
        }


        public static string TRANSACTIONID;
        public string WriteTxt()
        {
            StringBuilder sb = new StringBuilder();


            String address = "No. 6 Gidado Street";

            string saleID = TRANSACTIONID;
            //  String item = "item";

           
      
            int count = listView2.Items.Count;
           
            string total = txtBill.Text;
            string fukuan = txtPayment.Text;
            sb.Append("======================================\r\n");
            sb.Append("Date:" + DateTime.Now.ToShortDateString() + " \r\n");
            sb.Append("S/N:" + saleID + "\r\n");
            sb.Append("======================================\r\n");
            sb.Append(("ITEMS").PadLeft(12));
            sb.Append(("QTY").PadLeft(5));
            sb.Append(("PRICE").PadLeft(8));
            sb.AppendLine(("TOTAL").PadLeft(10));
            sb.Append("======================================\r\n");
            //to show results:

            for (int i = 0; i < listView2.Items.Count; i++)
            {
                string pd = "";
                if (listView2.Items[i].SubItems[1].Text.Length > 12)
                {

                    pd = listView2.Items[i].SubItems[1].Text.Substring(0, 10);

                }

                else
                {

                    pd = listView2.Items[i].SubItems[1].Text; 
                }
                sb.Append(pd.PadLeft(12));
                sb.Append(listView2.Items[i].SubItems[3].Text.ToString().PadLeft(5));
                sb.Append(listView2.Items[i].SubItems[2].Text.ToString().PadLeft(8));
                sb.AppendLine(listView2.Items[i].SubItems[4].Text.ToString().PadLeft(10));
                }
            for (int i = 0; i < count; i++)
            {
                sb.ToString();
            }
            sb.Append("======================================\r\n");
            sb.Append("Ordered Item(s): " + count + "\r\n");
            sb.Append("Total: " + total + "\r\n");
            sb.Append("Payment: Cash" + " " + fukuan + "\r\n");
            sb.Append("Cash change: " + " " + (txtChange.Text) + "\r\n");
            sb.Append("======================================\r\n");
            sb.Append("Address:" + address + "\r\n");
            sb.Append("Emiola, Area, Offa, KW/ST \r\n");
            sb.Append("phone: 08086600507 \r\n");
            sb.Append("Cashier: " +lblUser.Text+ "\r\n");
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
            generateID();

            if (txtBill.Text == "")
            {
                MessageBox.Show("Enter total payment", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBill.Focus();
                return;
            }
            if (txtPayment.Text == "")
            {
                MessageBox.Show("Enter your Payment.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPayment.Focus();
                return;
            }

            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("No Product to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Convert.ToDouble(txtBill.Text) > Convert.ToDouble(txtPayment.Text))
            {
                MessageBox.Show("Insufficient Cash!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPayment.SelectAll();
                txtPayment.Focus();
                return;

            }
            try
            {
                foreach (ListViewItem li in listView2.Items)
                {
                    string sql = @"INSERT INTO tblRecord([ID],[Description],[Price],[Quantity],[TotalSum],[DateTime], [Time]) VALUES(@row1,@row2,@row3,@row4,@row5,@Date1, @time)";
                    cm = new SqlCommand(sql, cn);

                    cm.Parameters.AddWithValue("@row1", li.SubItems[0].Text);
                    cm.Parameters.AddWithValue("@row2", li.SubItems[1].Text);
                    cm.Parameters.AddWithValue("@row3", li.SubItems[2].Text);
                    cm.Parameters.AddWithValue("@row4", li.SubItems[3].Text);
                    cm.Parameters.AddWithValue("@row5", li.SubItems[4].Text);
                    cm.Parameters.AddWithValue("@Date1", lblDate.Text);
                    cm.Parameters.AddWithValue("@Time", DateTime.UtcNow.ToString("HH:mm:ss tt"));
                    cm.ExecuteNonQuery(); //ExecuteNonQuery passes a connection string to database or SQL.

                    string sql2 = @"INSERT INTO tblCashierRecord([Cashier],[PID],[Descrip],[Price],[Quantity],[TotalSum],[DateTime], [Transactionid], [Time], [Categories]) VALUES(@row1,@row2,@row3,@row4,@row5,@row6,@row10, @row9, @row11, @cat)";
                    cm = new SqlCommand(sql2, cn);
                    cm.Parameters.AddWithValue("@row1", lblUser.Text);
                    cm.Parameters.AddWithValue("@row2", li.SubItems[0].Text);
                    cm.Parameters.AddWithValue("@row3", li.SubItems[1].Text);
                    cm.Parameters.AddWithValue("@row4", li.SubItems[2].Text);
                    cm.Parameters.AddWithValue("@row5", li.SubItems[3].Text);
                    cm.Parameters.AddWithValue("@row6", li.SubItems[4].Text);
                    cm.Parameters.AddWithValue("@row9", TRANSACTIONID);
                    cm.Parameters.AddWithValue("@row10", lblDate.Text);
                    cm.Parameters.AddWithValue("@row11", DateTime.UtcNow.ToString("HH:mm:ss tt"));
                    cm.Parameters.AddWithValue("@cat", li.SubItems[5].Text);
                    cm.ExecuteNonQuery();

                    string sql3 = @"INSERT INTO tblCashierPrint([Cashier],[PID],[Descrip],[Price],[Quantity],[TotalSum],[DateTime], [Transactionid],[Amount_Paid], [Change]) VALUES(@row1,@row2,@row3,@row4,@row5,@row6,@row10, @row9, @row12, @row13)";
                    cm = new SqlCommand(sql3, cn);
                    cm.Parameters.AddWithValue("@row1", lblUser.Text);
                    cm.Parameters.AddWithValue("@row2", li.SubItems[0].Text);
                    cm.Parameters.AddWithValue("@row3", li.SubItems[1].Text);
                    cm.Parameters.AddWithValue("@row4", li.SubItems[2].Text);
                    cm.Parameters.AddWithValue("@row5", li.SubItems[3].Text);
                    cm.Parameters.AddWithValue("@row6", li.SubItems[4].Text);
                    cm.Parameters.AddWithValue("@row9", TRANSACTIONID);
                    cm.Parameters.AddWithValue("@row10", lblDate.Text);
                    cm.Parameters.AddWithValue("@row12", txtPayment.Text);
                    cm.Parameters.AddWithValue("@row13", txtChange.Text);
                    cm.ExecuteNonQuery();
                }

                Total = 0;
                
                getData();

                Print();
                MessageBox.Show("Successfully saved" + "\nYour Change is: " + txtChange.Text, "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                listView2.Items.Clear();

                txtBill.Text = "";
                txtChange.Text = "";
                txtPayment.Text = "";
                //txtStock.Text = "";
                panel2.Visible = false;
            }
            catch
            {
                MessageBox.Show("Itemns not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelOR_Click(object sender, EventArgs e)
        {
            try
            {
                double val1 = 0;
                double val2 = 0;
                /*double.TryParse method converts the string representation of a number in a specified string or style.*/

                double.TryParse(temp.ToString(), out val1);//the value of index 4 from lstview2 and assigned to variable 'temp' is converted to double.
                double.TryParse(txtBill.Text, out val2);//Value from txtBill is converted to double
                listView2.FocusedItem.Remove();
                Total = val2 - val1;
                txtBill.Text = Total.ToString("#,###,##0.00");
             RemoveStock();
            }
            catch (Exception)
            {
                MessageBox.Show("No items to remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void RemoveStock() 
        {


            for (int j = 0; j <= listView1.Items.Count - 1; j++)
            {
                if (listView1.Items[j].SubItems[0].Text == txtID2.Text)
                {

                    listView1.Items[j].SubItems[3].Text = (Convert.ToDouble(listView1.Items[j].SubItems[3].Text) + Convert.ToDouble(txtQty2.Text)).ToString();
               try
                    {
                        string up = @"UPDATE PurchaseStock SET [Stock] = @stock where [StockID]='" + txtID2.Text + "'";
                        cm = new SqlCommand(up, cn);
                        cm.Parameters.AddWithValue("@Stock", listView1.Items[j].SubItems[3].Text);
                        cm.ExecuteNonQuery();

                        txtID2.Text = "";
                        getData();

                        //        MessageBox.Show("Successfully Updated!");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "No Items to Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
         
                    return;
                }
            }
        }

       

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {
            if (txtPayment.Text == "")
            {
            }
            else
            {
                try
                {
                    double t = Convert.ToDouble(txtBill.Text);
                    double c = Convert.ToDouble(txtPayment.Text);
                    if (c < t) //If Cash amount is Less Than to that of Total payment.
                    {
                        txtChange.Text = "0.00";
                    }else if(t < 1 || c < 1)
                    {
                        txtChange.Text = "0.00";
                    }
                    else
                    {
                        _totalPayment = Convert.ToDouble(txtBill.Text);
                        _cash = Convert.ToDouble(txtPayment.Text);
                        _change = _cash - _totalPayment;
                        txtChange.Text = _change.ToString("#,###,##0.00");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Numerics Only.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtChange.Text = "0.00";
                    txtPayment.Text = String.Empty;
                }
            }
        }

    

      
    

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //getBrand();
        }
        public void InsertTrail()
        {

            try
            {
                string sql = @"INSERT INTO tblLogTrail VALUES(@Dater,@Descrip,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblTimer.Text);
                cm.Parameters.AddWithValue("@Descrip", "User: " + lblUser.Text + " has Logged out!");
                cm.Parameters.AddWithValue("@Authority", "Cashier");


                cm.ExecuteNonQuery();
                //   MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }

      

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar!='.';
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.MeasureString(WriteTxt(), this.Font, e.MarginBounds.Size, StringFormat.GenericTypographic);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cashierreport CASHREP = new cashierreport();
            CASHREP.pass(lblUser.Text);
            CASHREP.ShowDialog();
        }

        private void txtPayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count > 0)
            {

                MessageBox.Show("Transactions is still in progress. Remove item(s) from CART.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }
            else
            {


                InsertTrail();

                // cm.ExecuteNonQuery();

                this.Close();

                Frmlogin frm1 = new Frmlogin();
                frm1.Show();
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            ReprintForm frmprint = new ReprintForm();
            frmprint.pass(lblUser.Text);
            frmprint.Show();
        }

        private void txtQuantity_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            double price = 0;
            double qty = 0;
            double.TryParse(txtPrice.Text, out price);
            double.TryParse(txtQuantity.Text, out qty);
            double SumP = (price * qty);
            txtSum.Text = SumP.ToString("#,###,##0.00");

            if (qty < 0)
            {

                MessageBox.Show("Positive Integers only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        
        private void listView2_Click(object sender, EventArgs e)
        {
            tempQty = Convert.ToDouble(listView2.FocusedItem.SubItems[3].Text);
            MessageBox.Show(tempQty.ToString());
        }

      
    }
}
