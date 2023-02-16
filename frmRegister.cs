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
using System.Security.Cryptography;
using System.IO;    
namespace UniqueRestaurant
{
    public partial class FrmRegister : Form
    {

        SqlConnection cn;
        SqlCommand cm;
       // SqlDataReader dr;
      //  string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";

        Frmlogin login = new Frmlogin();
       
        public FrmRegister()
        {
            InitializeComponent();

            cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()); 
            cn.Open();
            txtCreatePass.PasswordChar = '●';
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        public void generateID()
        {

            var chars = "0923213USERASDASDQ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            txtUserID.Text ="Unq:" + result;
            
        }
    

        public void InsertTrail()
        {
            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblTimer.Text);
                cm.Parameters.AddWithValue("@Transactype", "Insertion");
                cm.Parameters.AddWithValue("@Description", "User:" + txtCreateUser.Text + " has been sent registered!");
                cm.Parameters.AddWithValue("@Authority", "Admin");


                cm.ExecuteNonQuery();
                //   MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }
        public bool Checkusername(string user)
        {
            bool username = false;
          
            string sql = @"Select * from login where username =@user";
            cm = new SqlCommand(sql, cn);
            cm.Parameters.AddWithValue("@user", txtCreateUser.Text);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {

                username = true;
            }
            dr.Close();
            return username;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {

            if (txtCreatePass.Text == "" || txtCreateUser.Text == "" || txtAddress.Text == ""  || txtContact.Text == "" || txtFullName.Text == "" || cbodes.SelectedItem.ToString()=="Select")
            {
                  MessageBox.Show("Please Fill all the provided blanks!");

            }

            else {
                //Insertion to DataBase
                try
                {
                    if (Checkusername(txtCreateUser.Text))
                    {
                        MessageBox.Show("Username Not Available", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    string key = txtpass.Text;
                    string Password = EncDec.Encrypt(txtCreatePass.Text.ToString(), key); 
                    string sql = @"INSERT INTO Register VALUES(@ID,@FullName,@Designation,@Address, @Contact, @Username, @Password)";
                    cm = new SqlCommand(sql, cn);
                    string sql2 = @"INSERT INTO login (ID, Username, Password, UserType) VALUES(@ID, @Username, @Password, @Designation)";
                  SqlCommand  cm2 = new SqlCommand(sql2, cn);
                    cm.Parameters.AddWithValue("@ID", txtUserID.Text);
                    cm.Parameters.AddWithValue("@FullName", txtFullName.Text);
                    cm.Parameters.AddWithValue("@Designation", cbodes.SelectedItem.ToString());
                    cm.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cm.Parameters.AddWithValue("@Username", txtCreateUser.Text);
                    cm.Parameters.AddWithValue("@Password", Password);
                    cm2.Parameters.AddWithValue("@ID", txtUserID.Text);
                    cm2.Parameters.AddWithValue("@Username", txtCreateUser.Text);
                    cm2.Parameters.AddWithValue("@Password", Password);
                    cm2.Parameters.AddWithValue("@Designation", cbodes.Text);
                    cm.ExecuteNonQuery();
                    cm2.ExecuteNonQuery();
                    MessageBox.Show("Record successfully saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    InsertTrail();
                    this.Clear();
                    generateID();
                }
                catch (SqlException l)
                {
                    MessageBox.Show("Re-input again. your username may already be taken!");
                    MessageBox.Show(l.Message);
                }
            
            }

        }
        public void Clear()
        {
            txtContact.Text = "";
            txtAddress.Text = "";
  
            txtFullName.Text = "";
            txtCreateUser.Text = "";
            txtCreatePass.Text = "";
           
        
            }

        private void txtCreatePass_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmRegister_Load(object sender, EventArgs e)
        {
            timer1.Start();
            generateID();
            cbodes.Items.Clear();
          
            if (Frmlogin.usertype.Equals("Manager"))
            {
                cbodes.Items.Add("Cashier");
                cbodes.Items.Add("Waiter");
                cbodes.Items.Add("Server");
            }
            else
            {
                cbodes.Items.Add("Cashier");
                cbodes.Items.Add("Manager");
                cbodes.Items.Add("Waiter");
                cbodes.Items.Add("Server");
            }
            

        }

        private void txtCreateUser_TextChanged(object sender, EventArgs e)
        {

        }

      

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            //   string format = "MMM-dd-yyy HH:mm:ss";
            lblTimer.Text = time.ToString();
         
        }

        private void txtpass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
