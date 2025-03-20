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
    public partial class f_khachhang : Form
    {
        public f_khachhang()
        {
            InitializeComponent();
        }
        string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        private void f_khachhang_Load(object sender, EventArgs e)
        {
            hamdungchung dungchung = new hamdungchung();
            dungchung.ketnoi();
            dungchung.loadgridview("btlKhachHang", dataGridView_DSKH);
            dateTimePicker_dNgaySinh.ShowCheckBox = true;
            dateTimePicker_dNgaySinh.Checked = false; // Mặc định không có giá trị
        }

        private void button_Them_Click(object sender, EventArgs e)
        {
            DateTime ngaysinh = dateTimePicker_dNgaySinh.Value;
            string gioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ"; 

            // Kiểm tra mã khách hàng đã tồn tại chưa
            string checkSql = $"SELECT COUNT(*) FROM btlKhachHang WHERE sMaKH = '{textBox_sMaKH.Text}'";
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                cnn.Open();
                using (SqlCommand checkCommand = new SqlCommand(checkSql, cnn))
                {
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã khách hàng đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }
                }

                // Tạo câu lệnh SQL để thêm khách hàng
                string sqlInsert = $@"
        INSERT INTO btlKhachHang (sMaKH, sTenKH, sDiaChi, sSDT, dNgaySinh, sGioiTinh) 
        VALUES ('{textBox_sMaKH.Text}', N'{textBox_sTenKH.Text}', N'{textBox_sDiaChi.Text}', '{textBox_sSDT.Text}', 
                '{ngaysinh:yyyy-MM-dd}', N'{gioiTinh}')";

                // Gọi hàm dùng chung để thực thi lệnh
                if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
                {
                    MessageBox.Show("Thêm khách hàng thành công!");
                    f_khachhang_Load(sender, e); // Load lại danh sách khách hàng
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!");
                }
            }
        }

            private void button_Sua_Click(object sender, EventArgs e)
        {
            string gioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ";
            string sqlInsert = $@"
        UPDATE btlKhachHang 
        SET 
            sTenKH= N'{textBox_sTenKH.Text}', 
            sDiaChi = N'{textBox_sDiaChi.Text}', 
            dNgaySinh = '{dateTimePicker_dNgaySinh.Value:yyyy-MM-dd}', 
            sSDT = '{textBox_sSDT.Text}', 
            sGioiTinh = N'{gioiTinh}'
        WHERE sMaKH = '{textBox_sMaKH.Text}'";
            if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
            {
                MessageBox.Show("Sửa khách hàng thành công!");
                f_khachhang_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Sửa khách hàng thất bại!");
            }
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu mã khách hàng chưa được nhập
            if (string.IsNullOrWhiteSpace(textBox_sMaKH.Text))
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng cần xóa.");
                return;
            }

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }
            string sqlInsert = "DELETE FROM btlKhachHang" + $" WHERE sMaKH = {textBox_sMaKH.Text}";
            if (hamdungchung.thuchiendoanmasql(constr, sqlInsert))
            {
                MessageBox.Show("Xóa nhân viên thành công!");
                f_khachhang_Load(sender, e); // Load lại danh sách khách hàng
            }
            else
            {
                MessageBox.Show("Xóa khách hàng thất bại!");
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            //reset lại datagridview
            f_khachhang_Load(sender, e);
            //reset lại ô textbox
            textBox_sMaKH.Clear();
            textBox_sTenKH.Clear();
            textBox_sDiaChi.Clear();
            textBox_sSDT.Clear();

            // Reset DateTimePicker về trạng thái trống
            dateTimePicker_dNgaySinh.Checked = false;
            //bỏ chọn
            radioButton_Nam.Checked = false;
            radioButton_Nu.Checked = false;
        }

        private void button_TimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                cnn.Open();
                string query = @"
                          select sMaKH , sTenKH , dNgaySinh ,  sDiaChi , sSDT , sGioiTinh
                          from btlKhachHang
                          where 1=1";
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Tìm theo mã nhân viên 
                if (!string.IsNullOrEmpty(textBox_sMaKH.Text))
                {
                    query += " AND sMaKH LIKE @sMaKH ";
                    parameters.Add(new SqlParameter("@sMaKH", "%" + textBox_sMaKH.Text + "%"));
                }

                // Tìm theo họ tên nhân viên 
                if (!string.IsNullOrEmpty(textBox_sTenKH.Text))
                {
                    query += " AND sTenKH LIKE @sTenKH ";
                    parameters.Add(new SqlParameter("@sTenKH", "%" + textBox_sTenKH.Text + "%"));
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
                // Tìm theo giới tính
                if (radioButton_Nam.Checked)
                {
                    query += " AND sGioiTinh = @sGioiTinh ";
                    parameters.Add(new SqlParameter("@sGioiTinh", "Nam"));
                }
                else if (radioButton_Nu.Checked)
                {
                    query += " AND sGioiTinh = @sGioiTinh ";
                    parameters.Add(new SqlParameter("@sGioiTinh", "Nữ"));
                }

                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView_DSKH.DataSource = dt; // Cập nhật DataGridView
                    }

                }
            }
        }

        private void dataGridView_DSKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_DSKH.Rows[e.RowIndex];
                textBox_sMaKH.Text = row.Cells["sMaKH"].Value?.ToString();
                textBox_sTenKH.Text = row.Cells["sTenKH"].Value?.ToString();
                textBox_sDiaChi.Text = row.Cells["sDiaChi"].Value?.ToString();
                textBox_sSDT.Text = row.Cells["sSDT"].Value?.ToString();
                dateTimePicker_dNgaySinh.Value = row.Cells["dNgaySinh"].Value != DBNull.Value
                        ? Convert.ToDateTime(row.Cells["dNgaySinh"].Value)
                        : DateTime.Now;
                string gioiTinh = row.Cells["sGioiTinh"].Value?.ToString();
                if (gioiTinh == "Nam")
                {
                    radioButton_Nam.Checked = true;
                    radioButton_Nu.Checked = false;
                }
                else if (gioiTinh == "Nữ")
                {
                    radioButton_Nam.Checked = false;
                    radioButton_Nu.Checked = true;
                }
                else
                {
                    // Trường hợp không xác định giới tính (nếu có)
                    radioButton_Nam.Checked = false;
                    radioButton_Nu.Checked = false;
                }
            }

        }

        private void button_Indanhsach_Click(object sender, EventArgs e)
        {
            f_indanhsachkh formInKh = new f_indanhsachkh();
            formInKh.FormClosed += (s, args) => this.Show(); // Hiện lại Form khách hàng khi form in đóng
            formInKh.Show();
            this.Hide();
        }
    }
}
