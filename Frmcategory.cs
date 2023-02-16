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
    public partial class Frmcategory : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        ListViewItem lst;
        public Frmcategory()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
        }

        private void Frmcategory_Load(object sender, EventArgs e)
        {
            timer1.Start();
            generateID();
            getData();
            EncDec.GetCategory(cboitems);
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
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
        public void getData()
        {
            try
            {

                listView1.Items.Clear();
                listView1.Columns.Clear();

                listView1.Columns.Add("ID ", 100);
                listView1.Columns.Add("FoodNames ", 120);
                listView1.Columns.Add("Price ", 120);
                listView1.Columns.Add("DateTime ", 200);
                listView1.Columns.Add("Categories ", 200);
                string sql = @"Select * from Product  where Foodnames like '" + txtSearch.Text + "%' or Categories Like '" + txtSearch.Text + "%'";
                cm = new SqlCommand(sql, cn);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Fill textboxes to proceed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    string sql = @"INSERT INTO Product (ID, FoodNames, Price, DateTime, Categories) VALUES(@ID,@FoodName,@Price, @DateTime,@Categories)";
                    cm = new SqlCommand(sql, cn);
                    cm.Parameters.AddWithValue("@ID", txtID.Text);
                    cm.Parameters.AddWithValue("@FoodName", txtName.Text);
                    cm.Parameters.AddWithValue("@Price", txtPrice.Text);
                    cm.Parameters.AddWithValue("@DateTime", lblDate.Text);
                    cm.Parameters.AddWithValue("@Categories", cboitems.Text);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Clear();
                    generateID();
                }
                catch (SqlException l)
                {
                   MessageBox.Show(l.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        public void generateID()
        {

            var chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            txtID.Text = "Unq/" + result;

        }
        public void Clear()
        {
            this.Refresh();
            txtID.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            generateID();
            getData();
        }
        public void Delete()
        {

            try
            {
                listView1.FocusedItem.Remove();
                string del = "delete from Product where ID='" + txtID.Text + "'";
                cm = new SqlCommand(del, cn); cm.ExecuteNonQuery();
               
                Clear();
               
            }
            catch (Exception)
            {
                MessageBox.Show("No User to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);



            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Can't Delete if TextBox(es) are/is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete this User?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                  
                    Delete();
                }
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtID.Text = listView1.FocusedItem.Text;
            txtName.Text = listView1.FocusedItem.SubItems[1].Text;
            txtPrice.Text = listView1.FocusedItem.SubItems[2].Text;
            cboitems.SelectedItem = listView1.FocusedItem.SubItems[4].Text;
            btnAdd.Visible = false;
            btnDelete.Visible = true;
            btnUpdate.Visible = true;   
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string format = "MM-dd-yyy hh:mm";
            lblDate.Text = time.ToString(format);
        }
        public void UpdateTrail()
        {


            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("@Transactype", "Updation");
                cm.Parameters.AddWithValue("@Description", "Item: '" + txtName.Text + "' was UPDATED!");
                cm.Parameters.AddWithValue("@Authority", "Admin");

                cm.ExecuteNonQuery();

            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again.!");
                MessageBox.Show(l.Message);
            }
        }
        public void DeleteTrail()
        {

            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("Transactype", "Deletion");
                cm.Parameters.AddWithValue("@Description", "Item: '" + txtName.Text + "' was DELETED from inventory!");
                cm.Parameters.AddWithValue("@Authority", "Admin");


                cm.ExecuteNonQuery();



            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again.!");
                MessageBox.Show(l.Message);
            }

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                return;
            }
            try
            {
                if (txtID.Text == "")
                {

                    MessageBox.Show("select listview properly");
                    return;
                }
                string up = @"UPDATE Product SET [Price]=@price, FoodNames=@FoodName, Categories=@cat where [ID]='" + txtID.Text + "'";
                cm = new SqlCommand(up, cn);

                cm.Parameters.AddWithValue("@Price", txtPrice.Text);
                cm.Parameters.AddWithValue("@Foodname", txtName.Text);
                cm.Parameters.AddWithValue("@cat", cboitems.Text);
                cm.ExecuteNonQuery();
                UpdateTrail();
                Clear();
                getData();

                MessageBox.Show("Successfully Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No Items to Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }
    }
}
