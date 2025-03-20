using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_donhang : Form
    {
        hamdungchung hamdungchung = new hamdungchung();

        string connectionString = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        string MANV;
        public f_donhang(string manhanvien)
        {
            InitializeComponent();
            this.MANV = manhanvien;

            //hamdungchung.loadcombobox("btlLoaiHang", combobox_loaihang, "sMaLoaiHang", "sTenLoaiHang");

            //hamdungchung.loadcombobox("btlNhaCungCap", comboBox_tenNCC, "sMaNCC", "sTenNCC");

        }

        //private List<GioHang> listgiohang = new List<GioHang>();
        // chat gpt
        private BindingList<GioHang> listgiohang = new BindingList<GioHang>();

        private void DoiTenCot()
        {
            datagridview_giohang.Columns["MaHang"].HeaderText = "Mã hàng";
            datagridview_giohang.Columns["LoaiHang"].HeaderText = "Loại hàng";
            datagridview_giohang.Columns["TenSanPham"].HeaderText = "Tên sản phẩm";
            datagridview_giohang.Columns["TenNCC"].HeaderText = "Tên nhà cung cấp";
            datagridview_giohang.Columns["SoLuong"].HeaderText = "Số lượng";
            datagridview_giohang.Columns["GiaHang"].HeaderText = "Giá hàng";
            datagridview_giohang.Columns["Size"].HeaderText = "Size";
            datagridview_giohang.Columns["MauSac"].HeaderText = "Màu sắc";
            datagridview_giohang.Columns["ChatLieu"].HeaderText = "Chất liệu";
        }

        private void lammoinut()
        {
            datagridview_giohang.ClearSelection();
            datagridview_giohang.CurrentCell = null;
            dataGridView_mathang.ClearSelection();
            dataGridView_mathang.CurrentCell = null;
        }

        private void f_donhang_Load(object sender, EventArgs e)
        {
            hamdungchung dungchung = new hamdungchung();
            dungchung.ketnoi();
            dungchung.loadgridview("v_MatHang_ChiTiet", dataGridView_mathang);
            //datagridview_giohang.DataSource = new BindingList<GioHang>(listgiohang);
            // chat gpt
            datagridview_giohang.DataSource = listgiohang;
            datagridview_giohang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagridview_giohang.DataSource = listgiohang;
            datagridview_giohang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dungchung.loadcombobox("btlLoaihang", combobox_loaihang);
            combobox_loaihang.ValueMember = "sMaLoaiHang";
            combobox_loaihang.DisplayMember = "sTenLoaiHang";


            dungchung.loadcombobox("v_mausac", combobox_mausac);
            combobox_mausac.DisplayMember = "sMauSac";
            combobox_mausac.ValueMember = "sMauSac";

            dungchung.loadcombobox("v_size", combobox_size);
            combobox_size.ValueMember = "sSize";
            combobox_size.DisplayMember = "sSize";

            dungchung.loadcombobox("btlNhaCungCap", comboBox_tenNCC);
            comboBox_tenNCC.ValueMember = "sMaNCC";
            comboBox_tenNCC.DisplayMember = "sTenNcc";
        }

        private void LoadComboBoxSize(string maMH)
        {

            string query = "SELECT * from btlChiTietMatHang WHERE sMaMH = @MaMH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.SelectCommand.Parameters.AddWithValue("@MaMH", maMH);
                    adapter.Fill(dt);

                    combobox_size.DataSource = dt;
                    combobox_size.DisplayMember = "sSize";  // Hiển thị size
                    combobox_size.ValueMember = "sSize";    // Giá trị cũng là size
                    combobox_mausac.DataSource = dt;
                    combobox_mausac.DisplayMember = "sMauSac";
                    combobox_mausac.ValueMember = "sMauSac";
                }
            }
        }

        private DataGridViewRow GetSelectedRow(DataGridView gridView)
        {
            if (gridView.CurrentRow != null)
            {
                return gridView.CurrentRow;
            }
            else
            {
                MessageBox.Show("Không có dòng nào được chọn!");
                return null;
            }
        }

        private void CapNhatTongTien()
        {
            int tongTien = listgiohang.Sum(sp => sp.GiaHang * sp.SoLuong);
            textbox_tongtien.Text = tongTien.ToString("N0"); // Hiển thị số có dấu phân cách
        }

        private void button_them_Click(object sender, EventArgs e)
        {


                
                if (textbox_soluong.Text == "" || textbox_gia.Text == "")
            {
                MessageBox.Show("Vui lòng nhập thông tin số lượng và giá!");
                return;
            }

            DataGridViewRow row = GetSelectedRow(dataGridView_mathang);
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trước khi thêm!");
                return;
            }

            // Lấy mã mặt hàng
            string maMH = row.Cells["Mã mặt hàng"].Value?.ToString();
            if (string.IsNullOrEmpty(maMH)) return;

            // Cập nhật danh sách kích thước vào ComboBoxCell
            var sizeCell = (DataGridViewComboBoxCell)row.Cells["sSize"];
            sizeCell.DataSource = GetSizeList(maMH);

            // Cập nhật danh sách màu sắc vào ComboBoxCell
            var mauSacCell = (DataGridViewComboBoxCell)row.Cells["sMauSac"];
            mauSacCell.DataSource = GetMauSacList(maMH);

            string mahang = row.Cells["Mã mặt hàng"].Value?.ToString();
            string loaihang = row.Cells["Loại hàng"].Value?.ToString();
            string tensanpham = row.Cells["Tên sản phẩm"].Value?.ToString();
            string tenNCC = row.Cells["Tên nhà cung cấp"].Value?.ToString();
            int soluong = int.Parse(textbox_soluong.Text);

            string size = combobox_size.Text;
            string mausac = combobox_mausac.Text;


            string chatlieu = row.Cells["Chất liệu"].Value?.ToString();
            int giahang = int.Parse(textbox_gia.Text.Trim());

            // Thêm sản phẩm vào BindingList
            listgiohang.Add(new GioHang(mahang, loaihang, tensanpham, tenNCC, soluong, giahang, size, mausac, chatlieu));

            datagridview_giohang.DataSource = null;
            datagridview_giohang.DataSource = listgiohang;

            // Gọi hàm cập nhật số lượng trong database
            hamdungchung ham = new hamdungchung();
            ham.ketnoi();
            ham.CapNhatSoLuongMatHang(mahang, -soluong);
            dataGridView_mathang.Refresh();
            ham.loadgridview("v_MatHang_ChiTiet", dataGridView_mathang);
            // Không cần gán lại DataSource
            CapNhatTongTien();
            DoiTenCot();
            lammoinut();
        }

        private void label_dongia_Click(object sender, EventArgs e)
        {

        }

        private void button_xoasanpham_Click(object sender, EventArgs e)
        {


            if (datagridview_giohang.CurrentCell != null)
            {
                int rowIndex = datagridview_giohang.CurrentCell.RowIndex;

                if (rowIndex >= 0 && rowIndex < listgiohang.Count)
                {
                    GioHang sanpham = listgiohang[rowIndex]; // Lấy sản phẩm cần xóa
                    string mahang = sanpham.MaHang;
                    int soluong = sanpham.SoLuong;

                    // Xóa sản phẩm khỏi danh sách giỏ hàng
                    listgiohang.RemoveAt(rowIndex);
                    datagridview_giohang.DataSource = null;
                    datagridview_giohang.DataSource = new BindingList<GioHang>(listgiohang);

                    // Cập nhật lại số lượng trong bảng hàng hóa
                    hamdungchung ham = new hamdungchung();
                    ham.ketnoi();
                    ham.CapNhatSoLuongMatHang(mahang, soluong); // Tăng lại số lượng hàng trong kho
                    ham.loadgridview("v_MatHang_ChiTiet", dataGridView_mathang);
                    // Cập nhật tổng tiền
                    CapNhatTongTien();
                    DoiTenCot();

                    // Bỏ chọn dòng
                    datagridview_giohang.ClearSelection();
                    datagridview_giohang.CurrentCell = null;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void dataGridView_mathang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            hamdungchung hamdungchung = new hamdungchung();
            hamdungchung.ketnoi();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Đảm bảo không phải tiêu đề cột
            {
                DataGridViewRow row = dataGridView_mathang.Rows[e.RowIndex];

                // Lấy mã mặt hàng
                string maMH = row.Cells["Mã mặt hàng"].Value?.ToString();
                if (string.IsNullOrEmpty(maMH)) return;

                // Cập nhật danh sách kích thước vào ComboBoxCell
                var sizeCell = (DataGridViewComboBoxCell)row.Cells["sSize"];
                sizeCell.DataSource = GetSizeList(maMH);

                // Cập nhật danh sách màu sắc vào ComboBoxCell
                var mauSacCell = (DataGridViewComboBoxCell)row.Cells["sMauSac"];
                mauSacCell.DataSource = GetMauSacList(maMH);

                // Nếu có giá trị chọn sẵn, lấy số lượng tương ứng
                string selectedSize = sizeCell.Value?.ToString();
                string selectedMauSac = mauSacCell.Value?.ToString();

                if (!string.IsNullOrEmpty(selectedSize) && !string.IsNullOrEmpty(selectedMauSac))
                {
                    row.Cells["Tổng số lượng"].Value = GetSoLuong(maMH, selectedSize, selectedMauSac);
                }
                textbox_mamathang.Text = row.Cells["Mã mặt hàng"].Value.ToString();
                combobox_loaihang.Text = row.Cells["Loại hàng"].Value.ToString();
                textbox_tensanpham.Text = row.Cells["Tên sản phẩm"].Value.ToString();
                comboBox_tenNCC.Text = row.Cells["Tên nhà cung cấp"].Value.ToString();
                textbox_soluong.Text = row.Cells["Tổng số lượng"].Value.ToString();
                textbox_gia.Text = row.Cells["Giá hàng"].Value.ToString();
                textbox_chatlieu.Text = row.Cells["Chất liệu"].Value.ToString();
                combobox_size.Text = sizeCell.Value?.ToString();
                combobox_mausac.Text = mauSacCell.Value?.ToString();
                LoadComboBoxSize(maMH);
            }
        }

        // Hàm lấy số lượng theo mã mặt hàng, kích thước và màu sắc
        private int GetSoLuong(string maMH, string size, string mauSac)
        {
            int soLuong = 0;
            string query = "SELECT iSoluong FROM btlChiTietMatHang WHERE sMaMH = @maMH AND sSize = @size AND sMauSac = @mauSac";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maMH", maMH);
                cmd.Parameters.AddWithValue("@size", size);
                cmd.Parameters.AddWithValue("@mauSac", mauSac);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    soLuong = Convert.ToInt32(result);
                }
            }
            return soLuong;
        }

        private void dgvmathang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvmathang.Refresh();
            // Kiểm tra nếu chưa có cột "Kích thước" và "Màu sắc" thì thêm vào DataGridView
            if (!dataGridView_mathang.Columns.Contains("sSize"))
            {
                DataGridViewComboBoxColumn sizeColumn = new DataGridViewComboBoxColumn
                {
                    Name = "sSize",
                    HeaderText = "Kích thước",
                    DataPropertyName = "sSize"
                };
                dataGridView_mathang.Columns.Add(sizeColumn);
            }

            if (!dataGridView_mathang.Columns.Contains("sMauSac"))
            {
                DataGridViewComboBoxColumn mauSacColumn = new DataGridViewComboBoxColumn
                {
                    Name = "sMauSac",
                    HeaderText = "Màu sắc",
                    DataPropertyName = "sMauSac"
                };
                dataGridView_mathang.Columns.Add(mauSacColumn);
            }

            // Duyệt qua từng dòng để cập nhật danh sách kích thước & màu sắc
            foreach (DataGridViewRow row in dataGridView_mathang.Rows)
            {
                if (row.IsNewRow) continue;
                object value = row.Cells["Mã mặt hàng"].Value;
                if (value == null) continue;

                string maMH = value.ToString();

                // Gán danh sách kích thước vào cột "cSize"
                DataGridViewComboBoxCell sizeCell = new DataGridViewComboBoxCell();
                sizeCell.DataSource = GetSizeList(maMH);
                row.Cells["sSize"] = sizeCell;

                // Gán danh sách màu sắc vào cột "cMauSac"
                DataGridViewComboBoxCell mauSacCell = new DataGridViewComboBoxCell();
                mauSacCell.DataSource = GetMauSacList(maMH);
                row.Cells["sMauSac"] = mauSacCell;
            }
        }
        private List<string> GetMauSacList(string maMH)
        {
            List<string> list = new List<string>();
            string query = "SELECT DISTINCT sMauSac FROM btlChiTietMatHang WHERE sMaMH = @maMH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maMH", maMH);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader["sMauSac"].ToString());
                    }
                }
            }
            return list;
        }

        private List<string> GetSizeList(string maMH)
        {
            List<string> list = new List<string>();
            string query = "SELECT DISTINCT sSize FROM btlChiTietMatHang WHERE sMaMH = @maMH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maMH", maMH);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader["sSize"].ToString());
                    }
                }
            }
            return list;
        }

        private void dataGridView_mathang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (textbox_soluong.Text == "" || textbox_gia.Text == "")
            //{
            //    MessageBox.Show("Vui lòng nhập số lượng và giá!");
            //    return;

            //}
            //else
            //{
            //    if (e.RowIndex >= 0) // Đảm bảo không bấm vào tiêu đề cột
            //    {
            //        // Lấy dữ liệu của hàng được chọn

            //        DataGridViewRow row = dataGridView_mathang.Rows[e.RowIndex];

            //        // Lấy mã mặt hàng
            //        string maMH = row.Cells["Mã mặt hàng"].Value?.ToString();
            //        if (string.IsNullOrEmpty(maMH)) return;

            //        // Cập nhật danh sách kích thước vào ComboBoxCell
            //        var sizeCell = (DataGridViewComboBoxCell)row.Cells["sSize"];
            //        sizeCell.DataSource = GetSizeList(maMH);

            //        // Cập nhật danh sách màu sắc vào ComboBoxCell
            //        var mauSacCell = (DataGridViewComboBoxCell)row.Cells["sMauSac"];
            //        mauSacCell.DataSource = GetMauSacList(maMH);

            //        string mahang = row.Cells["Mã mặt hàng"].Value?.ToString();
            //        string loaihang = row.Cells["Loại hàng"].Value?.ToString();
            //        string tensanpham = row.Cells["Tên sản phẩm"].Value?.ToString();
            //        string tenNCC = row.Cells["Tên nhà cung cấp"].Value?.ToString();
            //        int soluong = 1;
            //        string size = sizeCell.Value?.ToString();
            //        string mausac = mauSacCell.Value?.ToString();
            //        string chatlieu = row.Cells["Chất liệu"].Value?.ToString();
            //        int giahang = int.Parse(textbox_gia.Text.Trim());

            //        listgiohang.Add(new GioHang(mahang, loaihang, tensanpham, tenNCC, soluong, giahang, size, mausac, chatlieu));
            //        datagridview_giohang.DataSource = null;
            //        datagridview_giohang.DataSource = listgiohang;
            //        CapNhatTongTien();
            //        DoiTenCot();
            //        dataGridView_mathang.ClearSelection();

            //        // 🔽 Đặt lại con trỏ chuột không chọn dòng nào
            //        dataGridView_mathang.CurrentCell = null;
            //        lammoinut();
            //    }
            //}
            
        }
        //

        private void button_lammoi_Click(object sender, EventArgs e)
        {
            //listgiohang.Clear();
            //datagridview_giohang.DataSource = new BindingList<GioHang>(listgiohang);

            combobox_loaihang.SelectedIndex = -1;
            comboBox_tenNCC.SelectedIndex = -1;
            combobox_size.SelectedIndex = -1;
            combobox_mausac.SelectedIndex = -1;
            textbox_chatlieu.Clear();
            dataGridView_mathang.Columns["sSize"].Visible = true;
            dataGridView_mathang.Columns["sMausac"].Visible = true;

            // Load lại dữ liệu gốc
            string query = "SELECT * FROM v_MatHang_ChiTiet";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView_mathang.DataSource = dt;
                    }
                }
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_thanhtoan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textbox_makhachhang.Text) ||
        string.IsNullOrWhiteSpace(textbox_tenkhachhang.Text) ||
        string.IsNullOrWhiteSpace(textbox_diachi.Text) ||
        string.IsNullOrWhiteSpace(textbox_sodienthoai.Text) ||
        (!checkbox_nam.Checked && !checkbox_nu.Checked) ||  // Phải chọn giới tính
        listgiohang.Count == 0) // Phải có sản phẩm trong giỏ hàng
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng và thêm sản phẩm vào giỏ hàng trước khi thanh toán!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string makhachhang = textbox_makhachhang.Text;
            string tenkhachhang = textbox_tenkhachhang.Text;
            string diachi = textbox_diachi.Text;
            int sodienthoai = int.Parse(textbox_sodienthoai.Text);
            string gioitinh;
            gioitinh = string.Empty;
            if (checkbox_nam.Checked == true)
            {
                gioitinh = "Nam";
            }
            else if (checkbox_nu.Checked == true)
            {
                gioitinh = "Nữ";
            }
            string ngaysinh = dateTimePicker1.Value.ToString("MM/dd/yyyy");
            string sql = "INSERT INTO btlKhachHang (sMaKH, sTenKH, dNgaySinh, sDiaChi, sSDT, sGioiTinh) " +
             "VALUES (@MaKH, @TenKH, @NgaySinh, @DiaChi, @SDT, @GioiTinh)";




            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Thêm tham số vào câu lệnh SQL
                cmd.Parameters.AddWithValue("@MaKH", makhachhang);
                cmd.Parameters.AddWithValue("@TenKH", tenkhachhang);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                cmd.Parameters.AddWithValue("@DiaChi", diachi);
                cmd.Parameters.AddWithValue("@SDT", sodienthoai);
                cmd.Parameters.AddWithValue("@GioiTinh", gioitinh);

                // Mở kết nối và thực thi lệnh
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm khách hàng thành công!");
            }


            string mahoadon = textbox_hoadon.Text;
            string manhanvien = MANV;
            string ngaylap = DateTime.Now.ToString("yyyy-MM-dd");
            double tongtien = float.Parse(textbox_tongtien.Text) * (0.01 * int.Parse(combobox_magiamgia.SelectedItem.ToString()));
            string phuongthucthanhtoan = combobox_phuongthuc.SelectedItem.ToString();
            string ngaydathang = datetime_ngaytao.Value.ToString("yyyy-MM-dd");
            string ngaygiaohang = datetime_ngaygiao.Value.ToString("yyyy-MM-dd");
            string magiamgia = combobox_magiamgia.SelectedItem.ToString();

            //string sql1 = @"INSERT INTO btlHoaDon 
            //                   (sMaHD, sMaKH, sMaNVLapHoaDon, dNgayLap, fTongTien, 
            //                    sPhuongThucThanhToan, dNgayDatHang, dNgayGiaoHang, sMaGiamGia) 
            //                   VALUES 
            //                   (@mahoadon, @makhachhang, @manhanvien, @ngaylap, @tongtien, 
            //                    @phuongthucthanhtoan, @ngaydathang, @ngaygiaohang, @magiamgia)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql12 = @"INSERT INTO btlHoaDon 
                               (sMaHD, sMaKH, sMaNVLapHoaDon, dNgayLap, fTongTien, 
                                sPhuongThucThanhToan, dNgayDatHang, dNgayGiaoHang, sMaGiamGia) 
                               VALUES 
                               (@mahoadon, @makhachhang, @manhanvien, @ngaylap, @tongtien, 
                                @phuongthucthanhtoan, @ngaydathang, @ngaygiaohang, @magiamgia)";

                using (SqlCommand cmd = new SqlCommand(sql12, conn))
                {
                    // Thêm tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@mahoadon", mahoadon);
                    cmd.Parameters.AddWithValue("@makhachhang", makhachhang);
                    cmd.Parameters.AddWithValue("@manhanvien", manhanvien);
                    cmd.Parameters.AddWithValue("@ngaylap", DateTime.Parse(ngaylap)); // Chuyển đổi chuỗi thành DateTime
                    cmd.Parameters.AddWithValue("@tongtien", tongtien);
                    cmd.Parameters.AddWithValue("@phuongthucthanhtoan", phuongthucthanhtoan);
                    cmd.Parameters.AddWithValue("@ngaydathang", string.IsNullOrEmpty(ngaydathang) ? (object)DBNull.Value : DateTime.Parse(ngaydathang));
                    cmd.Parameters.AddWithValue("@ngaygiaohang", string.IsNullOrEmpty(ngaygiaohang) ? (object)DBNull.Value : DateTime.Parse(ngaygiaohang));
                    cmd.Parameters.AddWithValue("@magiamgia", string.IsNullOrEmpty(magiamgia) ? (object)DBNull.Value : magiamgia);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }



            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // Mở kết nối một lần để tăng hiệu suất

                foreach (GioHang item in listgiohang)
                {
                    try
                    {
                        string sql2 = @"INSERT INTO btlChiTietHoaDon 
                               (sMaHD, sMaMH, sMauSac, sSize, sTenMatHang, fGiaBan, iSoLuong, sMaGiamGia, sChatLieu) 
                               VALUES 
                               (@sMaHD, @sMaMH, @sMauSac, @sSize, @sTenMatHang, @fGiaBan, @iSoLuong, @sMaGiamGia, @sChatLieu)";

                        using (SqlCommand cmd = new SqlCommand(sql2, conn))
                        {
                            // Thêm tham số từ danh sách GioHang
                            cmd.Parameters.AddWithValue("@sMaHD", mahoadon);
                            cmd.Parameters.AddWithValue("@sMaMH", item.MaHang);
                            cmd.Parameters.AddWithValue("@sMauSac", item.MauSac);
                            cmd.Parameters.AddWithValue("@sSize", item.Size);
                            cmd.Parameters.AddWithValue("@sTenMatHang", item.TenSanPham);
                            cmd.Parameters.AddWithValue("@fGiaBan", item.GiaHang);
                            cmd.Parameters.AddWithValue("@iSoLuong", item.SoLuong);
                            cmd.Parameters.AddWithValue("@sChatLieu", item.ChatLieu);

                            // Thêm thuộc tính ngoài lớp GioHang
                            cmd.Parameters.AddWithValue("@sMaGiamGia", string.IsNullOrEmpty(magiamgia) ? (object)DBNull.Value : magiamgia);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi thêm sản phẩm {item.TenSanPham}: {ex.Message}");
                    }
                }

                MessageBox.Show("Thêm toàn bộ chi tiết hóa đơn hoàn tất!");
            }
        }

        private void button_timkiem_Click(object sender, EventArgs e)
        {
            textbox_mamathang.Clear();
            textbox_tensanpham.Clear();
            textbox_chatlieu.Clear();
            textbox_gia.Clear();
            textbox_soluong.Clear();

            // Đặt ComboBox về trạng thái chưa chọn gì
            combobox_loaihang.SelectedIndex = -1;
            comboBox_tenNCC.SelectedIndex = -1;
            combobox_size.SelectedIndex = -1;
            combobox_mausac.SelectedIndex = -1;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button_timkiem_Click_1(object sender, EventArgs e)
        {
            dataGridView_mathang.Columns["sSize"].Visible = false;
            dataGridView_mathang.Columns["sMauSac"].Visible = false;

            string query = @"SELECT mh.sMaMH AS [Mã mặt hàng], lh.sTenLoaiHang AS [Loại hàng], 
             mh.sTenMH AS [Tên sản phẩm], ncc.sTenNCC AS [Tên nhà cung cấp], 
            ctmh.iSoluong AS [Tổng số lượng], mh.fGiaHang AS [Giá hàng], 
            mh.sChatLieu AS [Chất liệu], ctmh.sSize AS [Size], ctmh.sMauSac AS [Màu sắc]
            FROM btlMatHang mh
            JOIN btlLoaiHang lh ON mh.sMaLoaiHang = lh.sMaLoaiHang
            JOIN btlNhaCungCap ncc ON mh.sMaNCC = ncc.sMaNCC
            JOIN btlChiTietMatHang ctmh ON mh.sMaMH = ctmh.sMaMH
            WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(combobox_loaihang.Text))
            {
                query += " AND lh.sTenLoaiHang = @LoaiHang";
                parameters.Add(new SqlParameter("@LoaiHang", combobox_loaihang.Text));
            }

            if (!string.IsNullOrEmpty(comboBox_tenNCC.Text))
            {
                query += " AND ncc.sTenNCC = @TenNCC";
                parameters.Add(new SqlParameter("@TenNCC", comboBox_tenNCC.Text));
            }

            if (!string.IsNullOrEmpty(textbox_chatlieu.Text))
            {
                query += " AND mh.sChatLieu = @ChatLieu";
                parameters.Add(new SqlParameter("@ChatLieu", textbox_chatlieu.Text));
            }

            if (!string.IsNullOrEmpty(combobox_size.Text))
            {
                query += " AND ctmh.sSize = @Size";
                parameters.Add(new SqlParameter("@Size", combobox_size.Text));
            }

            if (!string.IsNullOrEmpty(combobox_mausac.Text))
            {
                query += " AND ctmh.sMauSac = @MauSac";
                parameters.Add(new SqlParameter("@MauSac", combobox_mausac.Text));
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView_mathang.DataSource = dt;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mamathang = textbox_mamathang.Text;
            int soluong = listgiohang.Count;
            string size = combobox_size.Text;
            string mausac = combobox_mausac.Text;
            hamdungchung.CapNhatSoLuongMatHang(mamathang, soluong, mausac, size);
            listgiohang.Clear();
            datagridview_giohang.DataSource = new BindingList<GioHang>(listgiohang);
        }

        private void textbox_makhachhang_TextChanged(object sender, EventArgs e)
        {
            string maKH = textbox_makhachhang.Text.Trim();
            if (string.IsNullOrEmpty(maKH)) return; // Nếu ô trống thì không làm gì

            string query = "SELECT sTenKH, dNgaySinh, sDiaChi, sSDT, sGioiTinh FROM btlKhachHang WHERE sMaKH = @MaKH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Nếu tìm thấy khách hàng
                    {
                        textbox_tenkhachhang.Text = reader["sTenKH"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(reader["dNgaySinh"]);
                        textbox_diachi.Text = reader["sDiaChi"].ToString();
                        textbox_sodienthoai.Text = reader["sSDT"].ToString();

                        // Kiểm tra giới tính
                        string gioiTinh = reader["sGioiTinh"].ToString();
                        if (gioiTinh == "Nam")
                        {
                            checkbox_nam.Checked = true;
                            checkbox_nu.Checked = false;
                        }
                        else if (gioiTinh == "Nữ")
                        {
                            checkbox_nam.Checked = false;
                            checkbox_nu.Checked = true;
                        }

                        MessageBox.Show("Thông tin khách hàng đã được tải lên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Nếu khách hàng không tồn tại, làm rỗng các ô nhập
                        textbox_tenkhachhang.Clear();
                        dateTimePicker1.Value = DateTime.Today;
                        textbox_diachi.Clear();
                        textbox_sodienthoai.Clear();
                        checkbox_nam.Checked = false;
                        checkbox_nu.Checked = false;
                    }
                }
            }
        }

        private DataTable ConvertGioHangToDataTable(BindingList<GioHang> list)
        {
            DataTable dt = new DataTable();

            // Tạo các cột cho DataTable giống class GioHang
            dt.Columns.Add("MaHang", typeof(string));
            dt.Columns.Add("LoaiHang", typeof(string));
            dt.Columns.Add("TenSanPham", typeof(string));
            dt.Columns.Add("TenNCC", typeof(string));
            dt.Columns.Add("SoLuong", typeof(int));
            dt.Columns.Add("GiaHang", typeof(int));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("MauSac", typeof(string));
            dt.Columns.Add("ChatLieu", typeof(string));

            // Duyệt danh sách giỏ hàng và thêm vào DataTable
            foreach (var item in list)
            {
                dt.Rows.Add(item.MaHang, item.LoaiHang, item.TenSanPham, item.TenNCC, item.SoLuong, item.GiaHang, item.Size, item.MauSac, item.ChatLieu);
            }

            return dt;
        }

        private void button_taohoadon_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_mathang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
