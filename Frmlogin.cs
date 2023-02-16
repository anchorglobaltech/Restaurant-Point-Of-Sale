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
using System.Security.Cryptography;
namespace UniqueRestaurant
{
    public partial class Frmlogin : Form
    {
      SqlCommand cm;
       
        SqlDataReader dr;
        static string path = Path.GetFullPath(Environment.CurrentDirectory);
        
        //static string dbname = "Whales.mdf";
        //public string connection = @"Data Source=(LocalDB)\v11.0;AttachDbFilename="+path+@"\"+dbname+"; Integrated Security=True";
      // public string connection = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=Whales;Integrated Security=True";
         SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        public Frmlogin()
        {

            InitializeComponent();
            timer1.Start();
        }

        public static string passusername = "";
        public static string userid = "";
        public static string usertype = "";
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmStart frmStart = new frmStart();
            frmStart.Show();


        }

       
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string key = txtkey.Text;
            string remd;
            if (WarnDueDate(out remd))
            {
                MessageBox.Show("Please you have ("+remd+") Days Left to Contact the Developer For Monthly Maintenance.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               
            }
            if (CheckDueDate())
            {
                MessageBox.Show("Please Contact the Developer for Application Maintenance", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            cn.Close();

            string Password ="";
                string pdbcipher =""; 
            bool IsExist = false;
           
            cn.Open();
            SqlCommand cmd = new SqlCommand("select * from login where UserName='" + txtUsername.Text + "'", cn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                pdbcipher = dr["password"].ToString();
                usertype = dr["usertype"].ToString(); 
               IsExist = true;
            }
            Password = EncDec.Decrypt(pdbcipher, key); 
            if (IsExist)  //if record exis in db , it will return true, otherwise it will return false  
            {
                
                if ((Password).Equals(txtPassword.Text) && usertype == "Super Admin" )
                {

                    passusername = txtUsername.Text;
                    userid = dr["ID"].ToString();
                    FrmAdminMenu frm7 = new FrmAdminMenu();
                    frm7.pass(txtUsername.Text);
                    frm7.Show();
                    this.Hide();
                    dr.Close();
                    InsertTrail();
                    dr.Close();
                  
                    MessageBox.Show("Welcome Admin", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else if (Password.Equals(txtPassword.Text) && usertype == "Cashier" )
                {
                   passusername = txtUsername.Text;
                    userid = dr["ID"].ToString();
                    FrmBookStore frm7 = new FrmBookStore();
                    frm7.pass(txtUsername.Text, lblTime.Text);
                    frm7.Show();
                    this.Hide();
                    dr.Close();
                    InsertTrail();
                    dr.Close();
                    MessageBox.Show("Welcome Cashier", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else if (Password.Equals(txtPassword.Text) && usertype == "Manager" )
                {
                     passusername = txtUsername.Text;
                    userid = dr["ID"].ToString();
                    ManagerMenu frm7 = new ManagerMenu();
                    frm7.pass(txtUsername.Text);
                    frm7.Show();
                    this.Hide();
                    dr.Close();
                    InsertTrail();
                   
                    MessageBox.Show("Welcome Manager", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Password is wrong!...", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else  //showing the error message if user credential is wrong  
            {
                MessageBox.Show("Please enter the valid credentials", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            cn.Close();
         }
        public void InsertTrail()
        {

            try
            {
                string sql = @"INSERT INTO tblLogTrail VALUES(@Dater,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblTime.Text);
                cm.Parameters.AddWithValue("@Description", "User: " + txtUsername.Text + " has successfully Logged In!");
                cm.Parameters.AddWithValue("@Authority", "Cashier");
                cm.ExecuteNonQuery();

            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again. your username may already be taken!");
                MessageBox.Show(l.Message);
            }
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
           // MessageBox.Show(path);
            if (CheckDueDate())
            {
                MessageBox.Show("Please Contact the Developer for Application Maintenance", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            cn.Open();
            txtPassword.PasswordChar = '●';
       
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            //FrmRegister frm4 = new FrmRegister();
            //frm4.ShowDialog();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            //FrmAdminLogin frm5 = new FrmAdminLogin();
            //frm5.ShowDialog();
        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            // string format = "MM-dd-yyy HH:mm:ss";
            lblTime.Text = time.ToString();
        }

        public bool WarnDueDate(out string remd)
        {
            bool remdays = false;
            remd = string.Empty;
            string dueDt = string.Empty;
            using (SqlConnection conDue = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()))
            {
                conDue.Open();
                string sqlStmt = "SELECT * FROM ddd";
                using (SqlCommand comDue = new SqlCommand(sqlStmt, conDue))
                {
                    comDue.CommandType = CommandType.Text;
                    using (SqlDataReader drDue = comDue.ExecuteReader())
                    {
                        if (drDue.HasRows)
                        {
                            while (drDue.Read())
                            {
                                dueDt = drDue["dueDate"].ToString();
                            }
                            //var dueDtHere = new System.DateTime (dueDt);
                            string cur_Dt = DateTime.Now.Date.ToString();
                            DateTime dt = Convert.ToDateTime(dueDt);
                            var diffofdate = dt - Convert.ToDateTime(cur_Dt); 
                            if (Convert.ToInt32(diffofdate.Days) <= 7)
                            {
                                remd = diffofdate.ToString().Substring(0,1);
                                remdays = true;
                            }
                        }
                    }
                }
            }
            return remdays;
        }
        public bool CheckDueDate()
        {
            bool dueDate = false;
            string dueDt=string.Empty;
            using (SqlConnection conDue = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString()))
            {
                conDue.Open();
                string sqlStmt = "SELECT * FROM ddd";
                using (SqlCommand comDue = new SqlCommand(sqlStmt, conDue))
                {
                    comDue.CommandType = CommandType.Text;
                    using (SqlDataReader drDue=comDue.ExecuteReader())
                    {
                        if (drDue.HasRows)
                        {
                            while (drDue.Read())
                            {
                                dueDt = drDue["dueDate"].ToString();
                            }
                            //var dueDtHere = new System.DateTime (dueDt);
                            string cur_Dt= DateTime.Now.Date.ToString();
                            DateTime dt = Convert.ToDateTime(dueDt);
                            if (dt <= Convert.ToDateTime(cur_Dt))
                            {
                                dueDate = true;
                            }
                        }  
                    }
                }
            }
            return dueDate;
        }
     }
}
