using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_nhanvien : Form
    {
        public f_nhanvien()
        {
            InitializeComponent();
        }
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void f_nhanvien_Load(object sender, EventArgs e)
        {
            hamdungchung dungchung = new hamdungchung();
            dungchung.ketnoi();
            dungchung.loadgridview("btlNhanVien", dataGridView_DSNV);
            dateTimePicker_dNgaySinh.ShowCheckBox = true;
            dateTimePicker_dNgaySinh.Checked = false; // Mặc định không có giá trị
            dateTimePicker_dNgayVaoLam.ShowCheckBox = true;
            dateTimePicker_dNgayVaoLam.Checked = false; // Mặc định không có giá trị
        }
        private void button_Them_Click(object sender, EventArgs e)
        {
            DateTime ngaysinh = dateTimePicker_dNgaySinh.Value;
            DateTime ngayvaolam = dateTimePicker_dNgayVaoLam.Value;

            // Kiểm tra mã nhân viên đã tồn tại chưa
            string checkSql = $"SELECT COUNT(*) FROM btlNhanVien WHERE sMaNV = '{textBox_sMaNV.Text}'";
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                cnn.Open();
                using (SqlCommand checkCommand = new SqlCommand(checkSql,cnn))
                {
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }
                }


                // Tạo câu lệnh SQL để thêm nhân viên
                string sqlInsert = $@"
        INSERT INTO btlNhanVien (sMaNV, sTenNV, sCCCD, sDiaChi, sSDT, dNgaySinh, dNgayVaoLam, sTenDangNhap, sMatKhau) 
        VALUES ('{textBox_sMaNV.Text}', N'{textBox_sTenNV.Text}', '{textBox_sCCCD.Text}', N'{textBox_sDiaChi.Text}', '{textBox_sSDT.Text}', 
                '{ngaysinh:yyyy-MM-dd}', '{ngayvaolam:yyyy-MM-dd}', '{textBox_sTenDangNhap.Text}', '{textBox_sMatKhau.Text}')";

                // Gọi hàm dùng chung để thực thi lệnh
                if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
                {
                    MessageBox.Show("Thêm nhân viên thành công!");
                    f_nhanvien_Load(sender, e); // Load lại danh sách nhân viên
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!");
                }
            }
        }

        private void button_Sua_Click(object sender, EventArgs e)
        {
            string sqlInsert = $@"
        UPDATE btlNhanVien 
        SET 
            sTenNV = N'{textBox_sTenNV.Text}', 
            sCCCD = '{textBox_sCCCD.Text}', 
            sDiaChi = N'{textBox_sDiaChi.Text}', 
            sSDT = '{textBox_sSDT.Text}', 
            dNgaySinh = '{dateTimePicker_dNgaySinh.Value:yyyy-MM-dd}', 
            dNgayVaoLam = '{dateTimePicker_dNgayVaoLam.Value:yyyy-MM-dd}', 
            sTenDangNhap = '{textBox_sTenDangNhap.Text}', 
            sMatKhau = '{textBox_sMatKhau.Text}'
        WHERE sMaNV = '{textBox_sMaNV.Text}'";

            if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
            {
                MessageBox.Show("Sửa nhân viên thành công!");
                f_nhanvien_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Sửa nhân viên thất bại!");
            }
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu mã nhân viên chưa được nhập
            if (string.IsNullOrWhiteSpace(textBox_sMaNV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên cần xóa.");
                return;
            }

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }
            string sqlInsert = "DELETE FROM btlNhanVien" + $" WHERE sMaNV = {textBox_sMaNV.Text}";
            if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
            {
                MessageBox.Show("Xóa nhân viên thành công!");
                f_nhanvien_Load(sender, e); // Load lại danh sách nhân viên
            }
            else
            {
                MessageBox.Show("Xóa nhân viên thất bại!");
            }
        }
        private void button_Reset_Click(object sender, EventArgs e)
        {
            //reset lại datagridview
            f_nhanvien_Load(sender, e);
            //reset lại ô textbox
            textBox_sMaNV.Clear();
            textBox_sTenNV.Clear();
            textBox_sCCCD.Clear();
            textBox_sDiaChi.Clear();
            textBox_sSDT.Clear();
            textBox_sTenDangNhap.Clear();
            textBox_sMatKhau.Clear();
            // Reset DateTimePicker về trạng thái trống
            dateTimePicker_dNgaySinh.Checked = false;
            dateTimePicker_dNgayVaoLam.Checked = false;

        }
        private void button_TimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                cnn.Open();
                string query = @"
                          select sMaNV , sTenNV , sCCCD , sDiaChi , sSDT , dNgaySinh , dNgayVaoLam
                          from btlNhanVien
                          where 1=1";
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Tìm theo mã nhân viên 
                if (!string.IsNullOrEmpty(textBox_sMaNV.Text))
                {
                    query += " AND sMaNV LIKE @sMaNV ";
                    parameters.Add(new SqlParameter("@sMaNV", "%" + textBox_sMaNV.Text + "%"));
                }

                // Tìm theo họ tên nhân viên 
                if (!string.IsNullOrEmpty(textBox_sTenNV.Text))
                {
                    query += " AND sTenNV LIKE @sTenNV ";
                    parameters.Add(new SqlParameter("@sTenNV", "%" + textBox_sTenNV.Text + "%"));
                }

                // Tìm theo căn cước công dân 
                if (!string.IsNullOrEmpty(textBox_sCCCD.Text))
                {
                    query += " AND sCCCD LIKE @sCCCD ";
                    parameters.Add(new SqlParameter("@sCCCD", "%" + textBox_sCCCD.Text + "%"));
                }

                // Tìm theo địa chỉ 
                if (!string.IsNullOrEmpty(textBox_sDiaChi.Text))
                {
                    query += " AND sDiaChi LIKE @sDiaChi ";
                    parameters.Add(new SqlParameter("@sDiaChi", "%" + textBox_sDiaChi.Text + "%"));
                }

                // Tìm theo số điện thoại 
                if (!string.IsNullOrEmpty(textBox_sSDT.Text))
                {
                    query += " AND sSDT LIKE @sSDT ";
                    parameters.Add(new SqlParameter("@sSDT", "%" + textBox_sSDT.Text + "%"));
                }

                // Tìm theo ngày sinh 
                if (dateTimePicker_dNgaySinh.Checked)
                {
                    query += " AND dNgaySinh = @dNgaySinh ";
                    parameters.Add(new SqlParameter("@dNgaySinh", dateTimePicker_dNgaySinh.Value.Date));
                }

                // Tìm theo ngày vào làm 
                if (dateTimePicker_dNgayVaoLam.Checked)
                {
                    query += " AND dNgayVaoLam = @dNgayVaoLam ";
                    parameters.Add(new SqlParameter("@dNgayVaoLam", dateTimePicker_dNgayVaoLam.Value.Date));
                }
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView_DSNV.DataSource = dt; // Cập nhật DataGridView
                    }

                }
            }
        }
          private void button_Indanhsach_Click(object sender, EventArgs e)
        {
            f_indanhsachnv formIn = new f_indanhsachnv();
            formIn.FormClosed += (s, args) => this.Show(); // Hiện lại Form Nhân Viên khi form in đóng
            formIn.Show();
            this.Hide();
        } 
        private void dataGridView_DSNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_DSNV.Rows[e.RowIndex];
                textBox_sMaNV.Text = row.Cells["sMaNV"].Value?.ToString();
                textBox_sTenNV.Text = row.Cells["sTenNV"].Value?.ToString();
                textBox_sCCCD.Text = row.Cells["sCCCD"].Value?.ToString();
                textBox_sDiaChi.Text = row.Cells["sDiaChi"].Value?.ToString();
                textBox_sSDT.Text = row.Cells["sSDT"].Value?.ToString();
                textBox_sTenDangNhap.Text = row.Cells["sTenDangNhap"].Value?.ToString();
                textBox_sMatKhau.Text = row.Cells["sMatKhau"].Value?.ToString();
                dateTimePicker_dNgaySinh.Value = row.Cells["dNgaySinh"].Value != DBNull.Value
                        ? Convert.ToDateTime(row.Cells["dNgaySinh"].Value)
                        : DateTime.Now;
                dateTimePicker_dNgayVaoLam.Value = row.Cells["dNgayVaoLam"].Value != DBNull.Value
                        ? Convert.ToDateTime(row.Cells["dNgayVaoLam"].Value)
                        : DateTime.Now;
            }

        }

     
    }
}
