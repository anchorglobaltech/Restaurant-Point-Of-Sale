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
    public partial class LoginTrail : Form
    {

        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
       // string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        ListViewItem lst;
        public LoginTrail()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
            getData();
        }
        public void getData()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Date", 200);
                listView1.Columns.Add("Description", 365);
                listView1.Columns.Add("Authority", 90);
              

                string sql2 = @"Select * from tblLogTrail order by Dater DESC";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[2].ToString());
                               
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginTrail_Load(object sender, EventArgs e)
        {

        }

        private void cboSort_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboSort.Text == "Default")
            {
                getData();

            }
            else 
            {

                getLogTrail();
            
            }
            
        }
        public void getLogTrail()
        {
            //displaying data from Database to lstview
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Date", 200);
                listView1.Columns.Add("Description", 350);
                listView1.Columns.Add("Authority", 90);


                string sql2 = @"Select * from tblLogTrail where Authority like '" + cboSort.Text + "' order by Dater DESC";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[2].ToString());

                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Do you really want to Delete ALL items?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                //   AllDelTrail();
                try
                {
                    //  listView1.FocusedItem.Remove();
                    string del = "DELETE from tblLogTrail";
                    cm = new SqlCommand(del, cn); cm.ExecuteNonQuery();

                    MessageBox.Show("Successfully Deleted!");
                    getData();
                }
                catch (Exception)
                {
                    MessageBox.Show("No Item to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }

   
     
    }
}
