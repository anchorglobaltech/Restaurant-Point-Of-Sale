using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using System.Configuration;
namespace UniqueRestaurant
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataSet1 cashier = GetData();
            ReportDataSource datasource = new ReportDataSource("DataSet1", cashier.Tables[0]);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
        }
        DateTime a, b;
        string c;
        public void pass(string cashier, DateTime startdate, DateTime EndDate)
        {
            a = startdate;
            b = EndDate;
            c = cashier;
        }
        private DataSet1 GetData()
        {

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblCashierRecord  where cashier='"+c+"' and DateTime between '" + a + "' and '" + b + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = cn;
                    sda.SelectCommand = cmd;
                    using (DataSet1 dsCustomers = new DataSet1())
                    {
                        sda.Fill(dsCustomers, "DataTable1");
                        return dsCustomers;
                    }
                }
            }
        }
    }
}
