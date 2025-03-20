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
    public partial class f_nhacungcap : Form
    {
        hamdungchung ham = new hamdungchung();
        public f_nhacungcap()
        {
            InitializeComponent();
        }
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        private void f_nhacungcap_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            ham.loadgridview("vw_DanhSachNhaCungCap", dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string maNCC = txbmanhacungcap.Text.Trim();
            string tenNCC = txbtennhacungcap.Text.Trim();
            string diaChi = txbdiachi.Text.Trim();
            string soDT = txbsodienthoai.Text.Trim();
            string email = txbemail.Text.Trim();

            if (string.IsNullOrEmpty(maNCC) || string.IsNullOrEmpty(tenNCC))
            {
                MessageBox.Show("Mã và Tên nhà cung cấp không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = $"INSERT INTO btlNhaCungCap (sMaNCC, sTenNCC, sDiaChi, sSDT, sEmail) VALUES " +
                         $"('{maNCC}', N'{tenNCC}', N'{diaChi}', '{soDT}', '{email}')";

            if (hamdungchung.thuchiendoanmasql(constr, sql))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string maNCC = txbmanhacungcap.Text.Trim();
            string tenNCC = txbtennhacungcap.Text.Trim();
            string diaChi = txbdiachi.Text.Trim();
            string soDT = txbsodienthoai.Text.Trim();
            string email = txbemail.Text.Trim();

            if (string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = $"UPDATE btlNhaCungCap SET sTenNCC = N'{tenNCC}', sDiaChi = N'{diaChi}', sSDT = '{soDT}', sEmail = '{email}' " +
                         $"WHERE sMaNCC = '{maNCC}'";

            if (hamdungchung.thuchiendoanmasql(constr, sql))
            {
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string maNCC = txbmanhacungcap.Text.Trim();

            if (string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No) return;

            string sql = $"DELETE FROM btlNhaCungCap WHERE sMaNCC = '{maNCC}'";

            if (hamdungchung.thuchiendoanmasql(constr, sql))
            {
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string maNCC = txbmanhacungcap.Text.Trim();

            if (string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng nhập Mã Nhà Cung Cấp để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = $"SELECT * FROM vw_DanhSachNhaCungCap WHERE MaNhaCungCap = '{maNCC}'";

            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
                {
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy Nhà Cung Cấp với Mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = null;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txbmanhacungcap.Text = dataGridView1.Rows[e.RowIndex].Cells["MaNhaCungCap"].Value.ToString();
                txbtennhacungcap.Text = dataGridView1.Rows[e.RowIndex].Cells["TenNhaCungCap"].Value.ToString();
                txbdiachi.Text = dataGridView1.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
                txbsodienthoai.Text = dataGridView1.Rows[e.RowIndex].Cells["SoDienThoai"].Value.ToString();
                txbemail.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            }
        }

        private void f_nhacungcap_Load_1(object sender, EventArgs e)
        {

        }
    }
}
