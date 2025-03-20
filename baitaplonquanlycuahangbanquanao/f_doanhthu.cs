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
using System.Globalization;
using System.Configuration;

namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_doanhthu : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        hamdungchung dc = new hamdungchung();
        public f_doanhthu()
        {
            InitializeComponent();
            //Thu
            dc.loadgridview("v_HoaDon", dataGridView_Thu);
            TinhTongTien_Thu();

            for (int i = 1; i <= 31; i++)
                comboBox_Ngay_Thu.Items.Add(i);
            for (int i = 1; i <= 12; i++)
                comboBox_Thang_Thu.Items.Add(i);
            for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                comboBox_Nam_Thu.Items.Add(nam);

            //Chi
            dc.loadgridview("v_DonNhapHang", dataGridView_Chi);
            TinhTongTien_Chi();

            for (int i = 1; i <= 31; i++)
                comboBox_Ngay_Chi.Items.Add(i);
            for (int i = 1; i <= 12; i++)
                comboBox_Thang_Chi.Items.Add(i);
            for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                comboBox_Nam_Chi.Items.Add(nam);

            //Doanh thu
            TinhDoanhThu();
        }

        private void comboBox_Ngay_Thu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Ngay_Thu.SelectedItem != null)
            {
                int ngay = Convert.ToInt32(comboBox_Ngay_Thu.SelectedItem);

                // Nếu chọn 31, chỉ hiển thị các tháng có 31 ngày
                if (ngay == 31)
                {
                    comboBox_Thang_Thu.Items.Clear();
                    int[] thang31 = { 1, 3, 5, 7, 8, 10, 12 };
                    foreach (int thang in thang31)
                        comboBox_Thang_Thu.Items.Add(thang);
                }
                else
                {
                    comboBox_Thang_Thu.Items.Clear();
                    for (int i = 1; i <= 12; i++)
                        comboBox_Thang_Thu.Items.Add(i);
                }

                comboBox_Thang_Thu.Enabled = true;
                comboBox_Thang_Thu.SelectedIndex = -1;
                comboBox_Nam_Thu.Enabled = false;
            }
        }

        private void comboBox_Thang_Thu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Thang_Thu.SelectedItem != null)
            {
                int ngay = comboBox_Ngay_Thu.SelectedItem != null ? Convert.ToInt32(comboBox_Ngay_Thu.SelectedItem) : 0;
                int thang = Convert.ToInt32(comboBox_Thang_Thu.SelectedItem);

                // Nếu chọn ngày 29 và tháng 2, chỉ cho phép chọn năm nhuận
                if (ngay == 29 && thang == 2)
                {
                    comboBox_Nam_Thu.Items.Clear();
                    for (int nam = 2000; nam <= DateTime.Now.Year; nam += 4) // Chỉ thêm năm nhuận
                        comboBox_Nam_Thu.Items.Add(nam);
                }
                else
                {
                    comboBox_Nam_Thu.Items.Clear();
                    for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                        comboBox_Nam_Thu.Items.Add(nam);
                }

                comboBox_Nam_Thu.Enabled = true;
                comboBox_Nam_Thu.SelectedIndex = -1;
            }
        }


        private void comboBox_Nam_Thu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Nam_Thu.SelectedItem != null)
            {
                LoadDataGridView_Thu();
            }
        }




        private void LoadDataGridView_Thu()
        {
            DataTable dt = new DataTable();

            int ngay = -1, thang = -1, nam = -1;

            // Kiểm tra giá trị được chọn trong các ComboBox
            if (comboBox_Ngay_Thu.SelectedItem != null)
                ngay = Convert.ToInt32(comboBox_Ngay_Thu.SelectedItem);

            if (comboBox_Thang_Thu.SelectedItem != null)
                thang = Convert.ToInt32(comboBox_Thang_Thu.SelectedItem);

            if (comboBox_Nam_Thu.SelectedItem != null)
                nam = Convert.ToInt32(comboBox_Nam_Thu.SelectedItem);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ngay != -1 && thang != -1 && nam != -1)
                    {
                        // Truy vấn theo ngày, tháng, năm
                        cmd.CommandText = "sp_LayHoaDonTheoNgayThangNam";
                        cmd.Parameters.AddWithValue("@ngay", ngay);
                        cmd.Parameters.AddWithValue("@thang", thang);
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien();
                    }
                    else if (thang != -1 && nam != -1)
                    {
                        // Truy vấn theo tháng, năm
                        cmd.CommandText = "sp_LayHoaDonTheoThangNam";
                        cmd.Parameters.AddWithValue("@thang", thang);
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien();
                    }
                    else if (nam != -1)
                    {
                        // Truy vấn theo năm
                        cmd.CommandText = "sp_LayHoaDonTheoNam";
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien();
                    }
                    else
                    {
                        // Không có lựa chọn hợp lệ
                        MessageBox.Show("Vui lòng chọn ít nhất một tiêu chí thời gian!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
            }

            // Kiểm tra nếu không có dữ liệu
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có hóa đơn nào trong thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridView_Thu.DataSource = dt;
            TinhTongTien();
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;

            // Kiểm tra nếu dataGridView_Thu có dữ liệu
            if (dataGridView_Thu.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView_Thu.Rows)
                {
                    if (row.Cells["Tổng tiền"].Value != null && row.Cells["Tổng tiền"].Value != DBNull.Value)
                    {
                        tongTien += Convert.ToDecimal(row.Cells["Tổng tiền"].Value);
                    }
                }
            }

            // Hiển thị tổng tiền lên label_TongTien
            label_TongThu.Text = $"TỔNG THU: {tongTien:N0} VND";
        }

        private void button_XoaLuaChon_Click(object sender, EventArgs e)
        {
            // Xóa lựa chọn của tất cả các comboBox
            comboBox_Ngay_Thu.SelectedIndex = -1;
            comboBox_Thang_Thu.SelectedIndex = -1;
            comboBox_Nam_Thu.SelectedIndex = -1;

            dc.loadgridview("v_HoaDon", dataGridView_Thu);
            comboBox_Ngay_Thu.Items.Clear();
            comboBox_Thang_Thu.Items.Clear();
            comboBox_Nam_Thu.Items.Clear();
            for (int i = 1; i <= 31; i++)
                comboBox_Ngay_Thu.Items.Add(i);
            for (int i = 1; i <= 12; i++)
                comboBox_Thang_Thu.Items.Add(i);
            for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                comboBox_Nam_Thu.Items.Add(nam);
            TinhTongTien_Thu();
            TinhDoanhThu();
        }

        private void f_doanhthu_Load(object sender, EventArgs e)
        {

        }

        private void button_XoaLuaChon_Chi_Click(object sender, EventArgs e)
        {
            // Xóa lựa chọn của tất cả các comboBox
            comboBox_Ngay_Chi.SelectedIndex = -1;
            comboBox_Thang_Chi.SelectedIndex = -1;
            comboBox_Nam_Chi.SelectedIndex = -1;

            dc.loadgridview("v_DonNhapHang", dataGridView_Chi);
            comboBox_Ngay_Chi.Items.Clear();
            comboBox_Thang_Chi.Items.Clear();
            comboBox_Nam_Chi.Items.Clear();
            for (int i = 1; i <= 31; i++)
                comboBox_Ngay_Chi.Items.Add(i);
            for (int i = 1; i <= 12; i++)
                comboBox_Thang_Chi.Items.Add(i);
            for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                comboBox_Nam_Chi.Items.Add(nam);
            TinhTongTien_Chi();
            TinhDoanhThu();
        }

        private void comboBox_Ngay_Chi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Ngay_Chi.SelectedItem != null)
            {
                int ngay = Convert.ToInt32(comboBox_Ngay_Chi.SelectedItem);

                // Nếu chọn 31, chỉ hiển thị các tháng có 31 ngày
                if (ngay == 31)
                {
                    comboBox_Thang_Chi.Items.Clear();
                    int[] thang31 = { 1, 3, 5, 7, 8, 10, 12 };
                    foreach (int thang in thang31)
                        comboBox_Thang_Chi.Items.Add(thang);
                }
                else
                {
                    comboBox_Thang_Chi.Items.Clear();
                    for (int i = 1; i <= 12; i++)
                        comboBox_Thang_Chi.Items.Add(i);
                }

                comboBox_Thang_Chi.Enabled = true;
                comboBox_Thang_Chi.SelectedIndex = -1;
                comboBox_Nam_Chi.Enabled = false;
            }
        }

        private void comboBox_Thang_Chi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Thang_Chi.SelectedItem != null)
            {
                int ngay = comboBox_Ngay_Chi.SelectedItem != null ? Convert.ToInt32(comboBox_Ngay_Chi.SelectedItem) : 0;
                int thang = Convert.ToInt32(comboBox_Thang_Chi.SelectedItem);

                // Nếu chọn ngày 29 và tháng 2, chỉ cho phép chọn năm nhuận
                if (ngay == 29 && thang == 2)
                {
                    comboBox_Nam_Chi.Items.Clear();
                    for (int nam = 2000; nam <= DateTime.Now.Year; nam += 4) // Chỉ thêm năm nhuận
                        comboBox_Nam_Chi.Items.Add(nam);
                }
                else
                {
                    comboBox_Nam_Chi.Items.Clear();
                    for (int nam = 2000; nam <= DateTime.Now.Year; nam++)
                        comboBox_Nam_Chi.Items.Add(nam);
                }

                comboBox_Nam_Chi.Enabled = true;
                comboBox_Nam_Chi.SelectedIndex = -1;
            }
        }

        private void comboBox_Nam_Chi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Nam_Chi.SelectedItem != null)
            {
                LoadDataGridView_Chi();
            }
        }

        private void LoadDataGridView_Chi()
        {
            DataTable dt = new DataTable();

            int ngay = -1, thang = -1, nam = -1;

            // Kiểm tra giá trị được chọn trong các ComboBox
            if (comboBox_Ngay_Chi.SelectedItem != null)
                ngay = Convert.ToInt32(comboBox_Ngay_Chi.SelectedItem);

            if (comboBox_Thang_Chi.SelectedItem != null)
                thang = Convert.ToInt32(comboBox_Thang_Chi.SelectedItem);

            if (comboBox_Nam_Chi.SelectedItem != null)
                nam = Convert.ToInt32(comboBox_Nam_Chi.SelectedItem);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ngay != -1 && thang != -1 && nam != -1)
                    {
                        // Truy vấn theo ngày, tháng, năm
                        cmd.CommandText = "sp_LayDNHTheoNgayThangNam";
                        cmd.Parameters.AddWithValue("@ngay", ngay);
                        cmd.Parameters.AddWithValue("@thang", thang);
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien_Chi();
                        TinhDoanhThu();
                    }
                    else if (thang != -1 && nam != -1)
                    {
                        // Truy vấn theo tháng, năm
                        cmd.CommandText = "sp_LayDNHTheoThangNam";
                        cmd.Parameters.AddWithValue("@thang", thang);
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien_Chi();
                        TinhDoanhThu();
                    }
                    else if (nam != -1)
                    {
                        // Truy vấn theo năm
                        cmd.CommandText = "sp_LayDNHTheoNam";
                        cmd.Parameters.AddWithValue("@nam", nam);
                        TinhTongTien_Chi();
                        TinhDoanhThu();
                    }
                    else
                    {
                        // Không có lựa chọn hợp lệ
                        MessageBox.Show("Vui lòng chọn ít nhất một tiêu chí thời gian!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
            }

            // Kiểm tra nếu không có dữ liệu
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có hóa đơn nào trong thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridView_Chi.DataSource = dt;
            TinhTongTien_Chi();
        }


        private void TinhTongTien_Chi()
        {
            decimal tongTien = 0;

            // Kiểm tra nếu dataGridView_Chi có dữ liệu
            if (dataGridView_Chi.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView_Chi.Rows)
                {
                    if (row.Cells["Tổng tiền"].Value != null && row.Cells["Tổng tiền"].Value != DBNull.Value)
                    {
                        tongTien += Convert.ToDecimal(row.Cells["Tổng tiền"].Value);
                    }
                }
            }

            // Hiển thị tổng tiền lên label_TongTien
            label_TongChi.Text = $"TỔNG CHI: {tongTien:N0} VND";
        }

        private void button_XemChiTiet_Chi_Click(object sender, EventArgs e)
        {
            string maDNH = textBox_NhapMaHD_Chi.Text.Trim();

            if (string.IsNullOrEmpty(maDNH))
            {
                MessageBox.Show("Vui lòng nhập mã đơn nhập hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form f_doanhthu_chi và truyền mã hóa đơn vào
            f_doanhthu_chi formChiTiet = new f_doanhthu_chi(maDNH);
            formChiTiet.ShowDialog(); // Hiển thị dạng popup
        }

        private void TinhDoanhThu()
        {
            // Gọi hai hàm tính tổng thu và tổng chi
            TinhTongTien_Thu();
            TinhTongTien_Chi();

            // Lấy giá trị từ label_TongThu và label_TongChi
            decimal tongThu = 0, tongChi = 0;

            // Kiểm tra và chuyển đổi giá trị từ label_TongThu
            if (decimal.TryParse(label_TongThu.Text.Replace("TỔNG THU: ", "").Replace(" VND", "").Replace(",", ""), out decimal thu))
            {
                tongThu = thu;
            }

            // Kiểm tra và chuyển đổi giá trị từ label_TongChi
            if (decimal.TryParse(label_TongChi.Text.Replace("TỔNG CHI: ", "").Replace(" VND", "").Replace(",", ""), out decimal chi))
            {
                tongChi = chi;
            }

            // Tính doanh thu = tổng thu - tổng chi
            decimal doanhThu = tongThu - tongChi;

            // Hiển thị doanh thu lên label_DoanhThu
            label_DoanhThu.Text = $"DOANH THU: {doanhThu:N0} VND";
        }

        private void TinhTongTien_Thu()
        {
            decimal tongTien = 0;

            // Kiểm tra nếu dataGridView_Thu có dữ liệu
            if (dataGridView_Thu.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView_Thu.Rows)
                {
                    if (row.Cells["Tổng tiền"].Value != null && row.Cells["Tổng tiền"].Value != DBNull.Value)
                    {
                        tongTien += Convert.ToDecimal(row.Cells["Tổng tiền"].Value);
                    }
                }
            }

            // Hiển thị tổng tiền lên label_TongTien
            label_TongThu.Text = $"TỔNG THU: {tongTien:N0} VND";
        }

        private void button_XemChiTiet_Thu_Click(object sender, EventArgs e)
        {
            string maHD = textBox_NhapMaHD_Thu.Text.Trim();

            if (string.IsNullOrEmpty(maHD))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form f_doanhthu_thu và truyền mã hóa đơn vào
            f_doanhthu_thu formChiTiet = new f_doanhthu_thu(maHD);
            formChiTiet.ShowDialog(); // Hiển thị dạng popup
        }
    }
}
