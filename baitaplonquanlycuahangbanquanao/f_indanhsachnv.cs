using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Configuration;

namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_indanhsachnv : Form
    {
        public f_indanhsachnv()
        {
            InitializeComponent();
        }
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        private void f_indanhsachnv_Load(object sender, EventArgs e)
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(@"D:\20032025-master\baitaplonquanlycuahangbanquanao\CR_dsNV.rpt");
            crystalReportViewer_dsNhanVien.ReportSource = rpt;
            crystalReportViewer_dsNhanVien.Refresh();
            
        }

        private void button_indsnv_Click(object sender, EventArgs e)
        {
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "proc_LayNhanVienTheoNamVaoLam";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NamVaoLam", textBox_NamVaoLam.Text);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        CR_dsNV rpt = new CR_dsNV();
                        rpt.SetDataSource(dt);
                        crystalReportViewer_dsNhanVien.ReportSource = rpt;
                        crystalReportViewer_dsNhanVien.Refresh();
                    }
                }

            }
        }
    }
}
