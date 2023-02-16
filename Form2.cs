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
    public partial class Form2 : Form
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlcon"].ToString());
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (c == "")
            {
                DataSet2 cashier = GetData();
                ReportDataSource datasource = new ReportDataSource("DataSet2", cashier.Tables[0]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.RefreshReport();
            }
            else
            {
                DataSet2 cashier = GetData2();
                ReportDataSource datasource = new ReportDataSource("DataSet2", cashier.Tables[0]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.RefreshReport();
            }
     
        }
        DateTime a, b;
        string c ="";
        public void pass( DateTime startdate, DateTime EndDate)
        {
            a = startdate;
            b = EndDate;
         
        }
        public void pass2(string search, DateTime startdate, DateTime EndDate)
        {
            a = startdate;
            b = EndDate;
            c = search;
        }
        private DataSet2 GetData()
        {

            using (SqlCommand cmd = new SqlCommand("Select * from tblRecord where  DateTime between '" + a + "' and '" + b + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = cn;
                    sda.SelectCommand = cmd;
                    using (DataSet2 dsCustomers = new DataSet2())
                    {
                        sda.Fill(dsCustomers, "DataTable2");
                        return dsCustomers;
                    }
                }
            }
        }

        private DataSet2 GetData2()
        {

            using (SqlCommand cmd = new SqlCommand("Select * from tblRecord where Description like '" + c + "%' and DateTime between '" + a + "' and '" + b + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = cn;
                    sda.SelectCommand = cmd;
                    using (DataSet2 dsCustomers = new DataSet2())
                    {
                        sda.Fill(dsCustomers, "DataTable2");
                        return dsCustomers;
                    }
                }
            }
        }
    }
}
