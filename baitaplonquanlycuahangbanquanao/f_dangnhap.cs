using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_dangnhap : Form
    {

        string connectionString = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        public string MaNhanVien { get; private set; } = "";
        public string TenNguoiDung { get; private set; } = "";
        public f_dangnhap()
        {
            InitializeComponent();
            txbmatkhau.UseSystemPasswordChar = true;

        }

        //public string TenNguoiDung { get; private set; }  // Biến lưu tên đăng nhập

        private void button_dangnhap_Click(object sender, EventArgs e)
        {
            
            string tenDangNhap = txbtendangnhap.Text;
            string matKhau = txbmatkhau.Text;

            if (KiemTraDangNhap(tenDangNhap, matKhau, out string maNV, out string tenNV))
            {
                MaNhanVien = maNV;
                TenNguoiDung = tenNV;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private bool KiemTraDangNhap(string tenDangNhap, string matKhau, out string maNV, out string tenNV)
        {
            maNV = "";
            tenNV = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT sMaNV, sTenNV FROM btlNhanVien WHERE sTenDangNhap = @TenDangNhap AND sMatKhau = @MatKhau";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            maNV = reader["sMaNV"].ToString();
                            tenNV = reader["sTenNV"].ToString();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void checkhienthimk_CheckedChanged(object sender, EventArgs e)
        {
            txbmatkhau.UseSystemPasswordChar = !checkhienthimk.Checked;
        }

        private void f_dangnhap_Load(object sender, EventArgs e)
        {

        }
    }
}
