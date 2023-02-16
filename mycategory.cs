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
    public partial class mycategory : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        ListViewItem lst;
        public mycategory()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIDCode.Text = listView1.FocusedItem.Text;
            txtName.Text = listView1.FocusedItem.SubItems[1].Text;
            btnAdd.Visible = false;
        }

        public void generateID()
        {

            var chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            txtIDCode.Text = "Unq/" + result;

        }
        
        private void mycategory_Load(object sender, EventArgs e)
        {
            timer1.Start();
            generateID();
            getData();
        }
        public void getData()
        {
            try
            {

                listView1.Items.Clear();
                listView1.Columns.Clear();

                listView1.Columns.Add("ID ", 100);
                listView1.Columns.Add("Category ", 120);
                listView1.Columns.Add("DateTime ", 200);
                listView1.Columns.Add("Time ", 200);
                string sql = "Select * from Category  where Categories like @cat";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("cat", "%"+txtSearch.Text+"%"); 
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr["id"].ToString());
                    lst.SubItems.Add(dr["Categories"].ToString());
                    lst.SubItems.Add(dr["DateTime"].ToString());
                    lst.SubItems.Add(dr["Time"].ToString());
                 }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Refreshme()
        {
            this.Refresh();
            txtName.Text = "";
            txtSearch.Text = "";
            generateID();
            btnAdd.Visible = true;
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
                    string sql = @"INSERT INTO Category(ID, Categories, DateTime, Time) VALUES(@ID,@Categories, @Date, @Time)";
                    cm = new SqlCommand(sql, cn);
                    cm.Parameters.AddWithValue("@ID", txtIDCode.Text);
                    cm.Parameters.AddWithValue("@Categories", txtName.Text);
                    cm.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString());
                    cm.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss tt"));
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Refreshme();
                    generateID();
                    getData();
                }
                catch (SqlException l)
                {
                    MessageBox.Show("Re-input again. your items may already be added to database!");
                    MessageBox.Show(l.Message);
                }
            }
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
        public void Delete()
        {
            try
            {
                listView1.FocusedItem.Remove();
                string del = "delete from category where ID='" + txtIDCode.Text + "'";
                cm = new SqlCommand(del, cn); cm.ExecuteNonQuery();
                Refreshme();
            }
            catch (Exception)
            {
                MessageBox.Show("No items to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void DeleteAll()
        {
            try
            {
                string del = "delete from category";
                cm = new SqlCommand(del, cn); 
                cm.ExecuteNonQuery();
                Refreshme();
            }
            catch (Exception)
            {
                MessageBox.Show("No items to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (txtIDCode.Text == "")
            {
                MessageBox.Show("Can't Delete if TextBox(es) are/is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete this Items?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Delete();
                }
            }

        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (txtIDCode.Text == "")
            {
                MessageBox.Show("Can't Delete if TextBox(es) are/is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete All Items", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteAll();
                }
            }
        }

        private void btnref_Click(object sender, EventArgs e)
        {
            Refreshme();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MM-dd-yyy HH:mm:ss";
            lblDate.Text = time.ToString();
        }
    }
}
