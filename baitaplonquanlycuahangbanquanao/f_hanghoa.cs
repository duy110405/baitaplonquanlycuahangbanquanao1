using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_hanghoa : Form
    {
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        public f_hanghoa()
        {
            InitializeComponent();
        }

        private void f_hanghoa_Load(object sender, EventArgs e)
        {
            hamdungchung dungchung = new hamdungchung();
            dungchung.ketnoi();
            dungchung.loadgridview("v_HangHoa", dgv_HangHoaTonKho);
            string[] columnsToCheck = { "Mã Hàng", "Kích Thước", "Màu Sắc", "Tên Mặt Hàng", "Giá Bán", "Giá Nhập", "Số Lượng Tồn", "Chất Liệu", "Loại Hàng" };
            RemoveDuplicateRowsByMultipleColumns(dgv_HangHoaTonKho, columnsToCheck);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f_main mainForm = this.MdiParent as f_main;
            if (mainForm != null)
            {
                //mainForm.OpenReportForm1();
            }
            else
            {
                MessageBox.Show("Lỗi: Không tìm thấy MainForm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các TextBox
            string maMH = txtTimKiemID.Text.Trim();
            string kichThuoc = txtSize.Text.Trim();
            string mauSac = txtMauSac.Text.Trim();

            // Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(maMH) || string.IsNullOrEmpty(kichThuoc) || string.IsNullOrEmpty(mauSac))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Hàng, Kích Thước và Màu Sắc!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận xóa mềm
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa mềm mặt hàng '{maMH}' với Kích Thước '{kichThuoc}' và Màu Sắc '{mauSac}'?",
                "Xác nhận xóa mềm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // Kiểm tra xem biến thể đó có tồn tại và đang hoạt động không (bTrangThai = 1)
                    using (SqlCommand checkCmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM btlChiTietMatHang 
                WHERE sMaMH = @maMH 
                  AND sSize = @size 
                  AND sMauSac = @mauSac 
                  AND bTrangThai = 1", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@maMH", maMH);
                        checkCmd.Parameters.AddWithValue("@size", kichThuoc);
                        checkCmd.Parameters.AddWithValue("@mauSac", mauSac);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Không tìm thấy mặt hàng cần xóa (hoặc đã bị xóa mềm)!",
                                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Thực hiện cập nhật bTrangThai = 0 để xóa mềm biến thể đó
                    using (SqlCommand cmd = new SqlCommand(@"
                UPDATE btlChiTietMatHang 
                SET bTrangThai = 0 
                WHERE sMaMH = @maMH 
                  AND sSize = @size 
                  AND sMauSac = @mauSac", conn))
                    {
                        cmd.Parameters.AddWithValue("@maMH", maMH);
                        cmd.Parameters.AddWithValue("@size", kichThuoc);
                        cmd.Parameters.AddWithValue("@mauSac", mauSac);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa mềm mặt hàng thành công!",
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Xóa mềm không thành công!",
                                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // Cập nhật lại dữ liệu hiển thị
                btn_ResetKhoHang_Click(sender, e);
            }
        }

        private void btn_ResetKhoHang_Click(object sender, EventArgs e)
        {
            hamdungchung dungchung = new hamdungchung();
            dungchung.ketnoi();
            dungchung.loadgridview("v_HangHoa", dgv_HangHoaTonKho);
            txtChatLieu.Clear();
            txtMauSac.Clear();
            txtSize.Clear();
            txtTimKiemGiaBan.Clear();
            txtTimKiemGiaNhap.Clear();
            txtTimKiemID.Clear();
            txtTimKiemLoaiHang.Clear();
            txtTimKiemMH.Clear();
            txtTimKiemSoLuongTon.Clear();
            string[] columnsToCheck = { "Mã Hàng", "Kích Thước", "Màu Sắc", "Tên Mặt Hàng", "Giá Bán", "Giá Nhập", "Số Lượng Tồn", "Chất Liệu", "Loại Hàng" };
            RemoveDuplicateRowsByMultipleColumns(dgv_HangHoaTonKho, columnsToCheck);
        }

        private void btn_TimKiemTonKho_Click(object sender, EventArgs e)
        {
            string config = ConfigurationManager.ConnectionStrings["baitaplonquanlycuahangbanquanao"].ConnectionString;
            using (SqlConnection cnn = new SqlConnection(config))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.Text;

                    string sql = "select * from v_HangHoa where 1=1";

                    if (!string.IsNullOrWhiteSpace(txtTimKiemID.Text))
                    {
                        sql += " and [Mã Hàng] like @sMaMH";
                        cmd.Parameters.AddWithValue("@sMaMH", "%" + txtTimKiemID.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtTimKiemMH.Text))
                    {
                        sql += " and [Tên Mặt Hàng] like @sTenMH";
                        cmd.Parameters.AddWithValue("@sTenMH", "%" + txtTimKiemMH.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtTimKiemGiaBan.Text))
                    {
                        if (float.TryParse(txtTimKiemGiaBan.Text.Trim(), out float giaBan))
                        {
                            sql += " and [Giá Bán] = @fGiaBan";
                            cmd.Parameters.AddWithValue("@fGiaBan", giaBan);
                        }
                        else
                        {
                            MessageBox.Show("Giá bán không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtTimKiemSoLuongTon.Text))
                    {
                        if (int.TryParse(txtTimKiemSoLuongTon.Text.Trim(), out int soLuongTon))
                        {
                            sql += " and [Số Lượng Tồn] = @iSoluong";
                            cmd.Parameters.AddWithValue("@iSoluong", soLuongTon);
                        }
                        else
                        {
                            MessageBox.Show("Số lượng tồn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtChatLieu.Text))
                    {
                        sql += " and [Chất Liệu] like @sChatLieu";
                        cmd.Parameters.AddWithValue("@sChatLieu", "%" + txtChatLieu.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtSize.Text))
                    {
                        sql += " and [Kích Thước] like @sSize";
                        cmd.Parameters.AddWithValue("@sSize", "%" + txtSize.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtMauSac.Text))
                    {
                        sql += " and [Màu Sắc] like @sMauSac";
                        cmd.Parameters.AddWithValue("@sMauSac", "%" + txtMauSac.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtTimKiemLoaiHang.Text))
                    {
                        sql += " and [Loại Hàng] like @sLoaiHang";
                        cmd.Parameters.AddWithValue("@sLoaiHang", "%" + txtTimKiemLoaiHang.Text.Trim() + "%");
                    }

                    cmd.CommandText = sql;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable tb = new DataTable();
                        da.Fill(tb);
                        string[] columnsToCheck = { "Mã Hàng", "Kích Thước", "Màu Sắc", "Tên Mặt Hàng", "Giá Bán", "Giá Nhập", "Số Lượng Tồn", "Chất Liệu", "Loại Hàng" };
                        tb = RemoveDuplicateRows(tb, columnsToCheck);
                        dgv_HangHoaTonKho.DataSource = tb;
                        MessageBox.Show($"Tim thay {tb.Rows.Count} san pham phu hop!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private DataTable RemoveDuplicateRows(DataTable dt, string[] columnNames)
        {
            return dt.AsEnumerable()
                     .GroupBy(r => string.Join("_", columnNames.Select(c => r[c].ToString())))
                     .Select(g => g.First())
                     .CopyToDataTable();
        }

        private void RemoveDuplicateRowsByMultipleColumns(DataGridView dgv, string[] columnNames)
        {
            HashSet<string> seen = new HashSet<string>();

            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgv.Rows[i];
                string key = string.Join("_", columnNames.Select(c => row.Cells[c].Value?.ToString()));

                if (seen.Contains(key))
                {
                    dgv.Rows.RemoveAt(i); // Xóa dòng nếu dữ liệu đã tồn tại
                }
                else
                {
                    seen.Add(key);
                }
            }
        }

        private void dgv_HangHoaTonKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_HangHoaTonKho.Rows[e.RowIndex];
                txtTimKiemID.Text = row.Cells["Mã Hàng"].Value.ToString();
                txtTimKiemMH.Text = row.Cells["Tên Mặt Hàng"].Value.ToString();
                txtTimKiemGiaBan.Text = Convert.ToString(row.Cells["Giá Bán"].Value);
                txtTimKiemGiaNhap.Text = Convert.ToString(row.Cells["Giá Nhập"].Value);
                txtTimKiemSoLuongTon.Text = Convert.ToString(row.Cells["Số Lượng Tồn"].Value);
                txtChatLieu.Text = row.Cells["Chất Liệu"].Value.ToString();
                txtSize.Text = row.Cells["Kích Thước"].Value.ToString();
                txtMauSac.Text = row.Cells["Màu Sắc"].Value.ToString();
                txtTimKiemLoaiHang.Text = row.Cells["Loại Hàng"].Value.ToString();
            }
        }

        private void dgv_HangHoaTonKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_HangHoaTonKho.Rows[e.RowIndex];
                txtTimKiemID.Text = row.Cells["Mã Hàng"].Value.ToString();
                txtTimKiemMH.Text = row.Cells["Tên Mặt Hàng"].Value.ToString();
                txtTimKiemGiaBan.Text = Convert.ToString(row.Cells["Giá Bán"].Value);
                txtTimKiemGiaNhap.Text = Convert.ToString(row.Cells["Giá Nhập"].Value);
                txtTimKiemSoLuongTon.Text = Convert.ToString(row.Cells["Số Lượng Tồn"].Value);
                txtChatLieu.Text = row.Cells["Chất Liệu"].Value.ToString();
                txtSize.Text = row.Cells["Kích Thước"].Value.ToString();
                txtMauSac.Text = row.Cells["Màu Sắc"].Value.ToString();
                txtTimKiemLoaiHang.Text = row.Cells["Loại Hàng"].Value.ToString();
            }
        }
    }
}
