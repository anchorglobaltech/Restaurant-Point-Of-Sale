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
    public partial class Expenses : Form
    {
        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
        ListViewItem lst;
      
        //string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        double listAmount = 0;
        public Expenses()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());


        }
 

      

        private void btnUpdateI_Click(object sender, EventArgs e)
        {

        }
        public void CLear()
        {
            //  txtIDCode.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
         
          
         


        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {


            if (txtPrice.Text == "")
            {


            }
            else
            {

                try
                {


                }
                catch (FormatException)
                {
                    MessageBox.Show("Enter Numbers Only.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPrice.Text = "0.00";


                }

            }



        }

        private void txtIDCode_TextChanged(object sender, EventArgs e)
        {

        }
      
      
       

       
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }

            if (!char.IsControl(e.KeyChar))
            {

                TextBox textBox = (TextBox)sender;

                if (textBox.Text.IndexOf('.') > -1 &&
                         textBox.Text.Substring(textBox.Text.IndexOf('.')).Length >= 3)
                {
                    e.Handled = true;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            FrmAdminMenu frmAM = new FrmAdminMenu();
            frmAM.Show();
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
         //   displaying data from Database to lstview
            try
            {
                cn.Close();
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("id", 100);
                listView1.Columns.Add("Item Name", 100);
                listView1.Columns.Add("Category", 150);
                listView1.Columns.Add("Price", 120);
                listView1.Columns.Add("Date", 100);

                string sql2 = @"Select * from Expenses";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[2].ToString());
                    lst.SubItems.Add(dr[3].ToString());
                    lst.SubItems.Add(dr[4].ToString());
                }
                dr.Close();
                cn.Close();
                double value = 0;
             for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[3].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");
            }
           
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddOrder(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                if (txtName.Text == "" || txtPrice.Text == "" || txtcat.Text == "")
                {
                    MessageBox.Show("Fill textboxes to proceed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string sql = "Insert into Expenses(Itemname, Category, Price, DateTime) Values ('" + txtName.Text + "', '" + txtcat.Text + "', '" + txtPrice.Text + "', '" + dtpdate.Text + "')";
                cm = new SqlCommand(sql, cn);
                cn.Open();
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getData();
            }
            catch
            {
                MessageBox.Show("Recheck Instance!", "Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }

       
         

        }
        public void InsertTrail()
        {
            
        }
     

        private void timer2_Tick(object sender, EventArgs e)
        {
         
        }
        public void DeleteAll()
        {
            cn.Close();
            cn.Open();
            // listView1.FocusedItem.Remove();
            string del = "DELETE from Expenses";
            cm = new SqlCommand(del, cn);
            cm.ExecuteNonQuery();

            MessageBox.Show("Successfully Deleted!");
            getData();
            cn.Close();

        }
         public void deleteRecords()
        {
            try
            {
                cn.Close();
                cn.Open();
                //   listView1.FocusedItem.Remove();
                string del = "DELETE from Expenses where id='" +focus+ "'";
                cm = new SqlCommand(del, cn); cm.ExecuteNonQuery();
                MessageBox.Show("Successfully Deleted!");
                getData();
                cn.Close();
                //generateOrderID();

            }
            catch (Exception)
            {
                MessageBox.Show("No Item to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
         private void btnRemove_Click(object sender, EventArgs e)
         {
             if (listView1.Items.Count == 0)
             {
                 MessageBox.Show("Nothing to Delete!. Please Select an item.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 return;
             }
             else
             {
                 if (MessageBox.Show("Do you really want to delete this Order?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                 {

                     deleteRecords();
                 }

             }

         }
        private void Expenses_Load(object sender, EventArgs e)
        {
            cn.Open();
            getData();
            timer1.Start();
        }
        public void getData2()
        {

           // displaying data from Database to lstview
            //try
            //{
                cn.Close();
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Id", 100);
                listView1.Columns.Add("Item Name", 100);
                listView1.Columns.Add("Category", 150);
                listView1.Columns.Add("Price", 90);
                listView1.Columns.Add("Date", 90);

                string sql2 = @"Select * from Expenses Where DateTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' Order by id DESC";
                cm = new SqlCommand(sql2, cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[2].ToString());
                    lst.SubItems.Add(dr[3].ToString());
                    lst.SubItems.Add(dr[4].ToString());
                }
                dr.Close();
                cn.Close();
           
            double value = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                value += double.Parse(listView1.Items[i].SubItems[3].Text);
            }
            lblTotal.Text = value.ToString("#,###,##0.00");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        //public void 
        private void button2_Click(object sender, EventArgs e)
        {
            getData2();
        }

        private void btnreport_Click(object sender, EventArgs e)
        {
            ExReport purchased = new ExReport();
            purchased.pass(dateTimePicker1.Value, dateTimePicker2.Value);
            purchased.ShowDialog();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
              if (listView1.Items.Count == 0)
            {
                 return;
            }

            if (MessageBox.Show("Do you really want to delete ALL Order?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
               
                DeleteAll();
             
                //generateOrderID();

            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcat_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpdate_ValueChanged(object sender, EventArgs e)
        {

        }
            static string focus;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
             focus = listView1.FocusedItem.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Dispose();
        }

     

       
    }
}
