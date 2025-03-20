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
    public partial class f_indanhsachkh : Form
    {
        public f_indanhsachkh()
        {
            InitializeComponent();
        }
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        private void f_indanhsachkh_Load(object sender, EventArgs e)
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(@"D:\20032025-master\baitaplonquanlycuahangbanquanao\CR_dsKh.rpt");
            crystalReportViewer_dsKhachHang.ReportSource = rpt;
            crystalReportViewer_dsKhachHang.Refresh();
        }

        private void button_indskh_Click(object sender, EventArgs e)
        {
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "proc_LayKhachHangTheoNgayDatHang";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Thang", textBox_thangdathang.Text);
                    cmd.Parameters.AddWithValue("@Nam", textBox_NamDatHang.Text);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        CR_dsKh rpt = new CR_dsKh();
                        rpt.SetDataSource(dt);
                        crystalReportViewer_dsKhachHang.ReportSource = rpt;
                        crystalReportViewer_dsKhachHang.Refresh();
                    }
                }

            }
        }
    }
}
