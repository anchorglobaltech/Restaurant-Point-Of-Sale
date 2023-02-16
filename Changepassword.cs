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
    public partial class Changepassword : Form
    {
        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
        ListViewItem lst;
        //string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        Frmlogin login = new Frmlogin();

        public Changepassword()
        {
            InitializeComponent();
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()); 
            cn.Open();
        }
      
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Fields cannot be Empty");
                return;
            }
            if (txtpass.Text != txtPassword.Text)
            {
                MessageBox.Show("Password not match!");
           
            }
            else
            {
                string up = @"UPDATE Register SET [username]='" + txtUsername.Text + "', [Password]='"+txtPassword.Text+"' where id='"+Frmlogin.userid+"'";
                cm = new SqlCommand(up, cn);
                cm.ExecuteNonQuery();
                string upd = @"UPDATE Login SET [username]='" + txtUsername.Text + "', [Password]='" + txtPassword.Text + "' where id='" + Frmlogin.userid + "'";
               SqlCommand cm2 = new SqlCommand(upd, cn);
                cm2.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated!");
                txtPassword.Text = "";
                txtpass.Text ="";
                txtUsername.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void Changepassword_Load(object sender, EventArgs e)

        {
            txtUsername.Enabled = false;
            txtUsername.Text = Frmlogin.passusername;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }
    }
}
