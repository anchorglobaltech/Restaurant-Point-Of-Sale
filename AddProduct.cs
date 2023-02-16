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
    public partial class AddProduct : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        ListViewItem lst;
        public AddProduct()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
            cn.Open();
        }
        public void generateID()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            txtIDCode.Text = "Unq/" + result;



        }
        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

     
        double stock = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboitems.SelectedItem.ToString() == "Select Items")
            {
                MessageBox.Show("Supply required Feilds!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtStock.Text != "")
            {
                
            }
        }
        public void getproduct()
        {

            try
            {


                string sql2 = @"Select * from Category";
                cm = new SqlCommand(sql2, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cboitems.Items.Add(dr[1].ToString());

                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AddProduct_Load(object sender, EventArgs e)
        {
            cboitems.SelectedIndex = 0;
            
            //getData();
            generateID();
          
            getproduct();
            timer1.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
