using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace UniqueRestaurant
{
    
    class EncDec
    {
        static SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        static SqlDataReader dr;
        static Random r = new Random();
        
        public static string Decrypt(string cipherText, string key)
        {
            try
            {
                string EncryptionKey = key;  //we can change the code converstion key as per our requirement, but the decryption key should be same as encryption key    
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {      
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76      
        });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }

            catch (Exception er)
            {

            }
            return cipherText;

        }
        public static string Encrypt(string encryptString, string key)
        {
            string EncryptionKey = key; //we can change the code converstion key as per our requirement    
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {      
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76      
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        public static void GetCategory(ComboBox itemslist)
        {
            try
            {
                con.Close();
                con.Open();
                itemslist.Items.Clear();
                itemslist.Items.Add("-Select-");
                string sqlstmt = "SELECT DISTINCT categories FROM Category";
                SqlCommand com = new SqlCommand(sqlstmt, con);
                dr = com.ExecuteReader();
                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        itemslist.Items.Add(dr["Categories"].ToString());
                    }
                }
                con.Close();
            }
            catch
            {
            }
        }
        public static void GetFooditems(ComboBox itemslist, ComboBox prodlist)
        {
            try
            {
                con.Close();
                con.Open();
                prodlist.Items.Clear();
                prodlist.Items.Add("-Select-");
                string sqlstmt = "SELECT DISTINCT Foodnames FROM Product Where Categories = @cat ";
                SqlCommand com = new SqlCommand(sqlstmt, con);
                com.Parameters.AddWithValue("cat", itemslist.SelectedItem.ToString());
                dr = com.ExecuteReader();
                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        prodlist.Items.Add(dr["FoodNames"].ToString());
                    }
                }
                con.Close();
            }
            catch
            {
            }
        }
        public static void GetFoodcat(ComboBox itemslist, ComboBox prodlist)
        {
            try
            {
                con.Close();
                con.Open();
                prodlist.Items.Clear();
                prodlist.Items.Add("-Select-");
                string sqlstmt = "SELECT DISTINCT Foodnames FROM Stock Where Categories = @cat ";
                SqlCommand com = new SqlCommand(sqlstmt, con);
                com.Parameters.AddWithValue("cat", itemslist.SelectedItem.ToString());
                dr = com.ExecuteReader();
                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        prodlist.Items.Add(dr["FoodNames"].ToString());
                    }
                }
                con.Close();
            }
            catch
            {
            }
        }
    }
   
}
