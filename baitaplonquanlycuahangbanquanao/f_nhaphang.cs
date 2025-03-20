using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_nhaphang : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        hamdungchung ham = new hamdungchung();
        //public string currentUserID = "NV002";
        private string maNhanVien; 


        private string maNV;
        private string tenNV;
        // code sửa chat gpt
        public f_nhaphang(string maNV, string tenNV)
        {
            InitializeComponent();
            this.Load += new EventHandler(f_nhaphang_Load);

            this.maNV = maNV;
            this.tenNV = tenNV;
            txbtennguoinhap.Text = maNV;

        }
        // Hàm tạo mã nhập hàng tự động
        private string GenerateMaNhapHang()
        {
            return "DHN" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void f_nhaphang_Load(object sender, EventArgs e)
        {
            txbmadonhangnhap.Text = GenerateMaNhapHang();
            ham.loadgridview("v_MatHang_fNhapHang", dgvdssp);
            LoadComboBoxData(); // Load dữ liệu vào combobox
                                // Thêm các cột cần thiết
            dgvhangnhap.Columns.Add("sMaHDN", "Mã hóa đơn");
            dgvhangnhap.Columns.Add("sMaNV", "Mã người nhập");
            dgvhangnhap.Columns.Add("sMaNCC", "Mã nhà cung cấp");
            dgvhangnhap.Columns.Add("dNgayNhap", "Ngày nhập");
            dgvhangnhap.Columns.Add("sMaMH", "Mã mặt hàng");
            dgvhangnhap.Columns.Add("sTenMH", "Tên mặt hàng");
            dgvhangnhap.Columns.Add("sTenLoaiHang", "Loại hàng");
            dgvhangnhap.Columns.Add("sSize", "Size");
            dgvhangnhap.Columns.Add("sMauSac", "Màu sắc");
            dgvhangnhap.Columns.Add("sChatLieu", "Chất liệu");
            dgvhangnhap.Columns.Add("fGiaHang", "Giá nhập");
            dgvhangnhap.Columns.Add("iSoLuong", "Số lượng");
            dgvhangnhap.Columns.Add("iTongTien", "Thành tiền");
        }
        
        private void LoadComboBoxData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                LoadComboBox("SELECT sMaNCC FROM btlNhaCungCap", cbnhacungcap, "sMaNCC");
            }
        }
        private void LoadComboBox(string query, ComboBox cb, string valueField)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, new SqlConnection(connectionString));
            DataTable dt = new DataTable();
            da.Fill(dt);
            cb.DataSource = dt;
            cb.DisplayMember = valueField;
            cb.ValueMember = valueField;
            cb.SelectedIndex = -1;
        }
        private void UpdateTongTien()
        {
            int tongTien = 0;
            foreach (DataGridViewRow row in dgvhangnhap.Rows)
            {
                if (row.Cells["iTongTien"].Value != null)
                    tongTien += Convert.ToInt32(row.Cells["iTongTien"].Value);
            }
            txbtongtien.Text = tongTien.ToString();
        }
        
        private void btnthem_Click(object sender, EventArgs e)
        {
            //// Cập nhật tổng tiền
            //UpdateTongTien();
            string maHDN = "DHN" + DateTime.Now.ToString("yyyyMMddHHmmss"); // Mã hóa đơn nhập
            string maNCC = cbnhacungcap.SelectedValue.ToString(); // Mã nhà cung cấp
            string maMH = textBox_Mamathang.Text.Trim(); // Mã mặt hàng
            string tenMH = txbtenmathang.Text.Trim(); // Tên mặt hàng
            string tenLoaiHang = comboBox_Tenloaihang.Text.Trim(); // Tên loại hàng
            string kichThuoc = txbsize.Text.Trim(); // Kích thước
            string mauSac = txbmausac.Text.Trim(); // Màu sắc
            string chatLieu = txbchatlieu.Text.Trim(); // Chất liệu
            double giaNhap = double.Parse(txbgianhap.Text.Trim()); // Giá nhập
            int soLuong = int.Parse(txbsoluongnhap.Text.Trim()); // Số lượng
            DateTime ngayNhap = dtngaynhap.Value; // Ngày nhập

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(maMH) || string.IsNullOrEmpty(maHDN) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daTonTai = false;

            // Kiểm tra sản phẩm đã có trong dgvhangnhap chưa (dựa vào mã MH, kích thước và màu sắc)
            foreach (DataGridViewRow row in dgvhangnhap.Rows)
            {
                if (row.Cells["sMaMH"].Value?.ToString() == maMH &&
                    row.Cells["sSize"].Value?.ToString() == kichThuoc &&
                    row.Cells["sMauSac"].Value?.ToString() == mauSac)
                {
                    // Nếu trùng mã mặt hàng, kích thước, màu sắc -> Cập nhật số lượng và tổng tiền
                    int oldSoLuong = Convert.ToInt32(row.Cells["iSoLuong"].Value);
                    row.Cells["iSoLuong"].Value = oldSoLuong + soLuong;
                    row.Cells["iTongTien"].Value = (oldSoLuong + soLuong) * giaNhap;

                    daTonTai = true;
                    break; // Thoát vòng lặp
                }
            }

            // Nếu chưa có mặt hàng với cùng mã MH, kích thước và màu sắc, thêm mới vào DataGridView
            if (!daTonTai)
            {
                dgvhangnhap.Rows.Add(maHDN, txbtennguoinhap.Text.Trim(), maNCC, ngayNhap.ToString("yyyy-MM-dd"),
                    maMH, tenMH, tenLoaiHang, kichThuoc, mauSac, chatLieu, giaNhap, soLuong, soLuong * giaNhap);
            }

            // Cập nhật tổng tiền
            UpdateTongTien();
        }





        private void txbsoluong_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbsize_TextChanged(object sender, EventArgs e)
        {

        }

   
        private void btnreset_Click(object sender, EventArgs e)
        {
            ham.loadgridview("v_MatHang_fNhapHang", dgvdssp);
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (dgvhangnhap.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvhangnhap.SelectedRows)
                {
                    dgvhangnhap.Rows.Remove(row); // Xóa dòng được chọn
                }
                UpdateTongTien(); // Cập nhật tổng tiền sau khi xóa
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        

        private void dgvhangnhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e) //btnxacnhan nha bro
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    decimal tongTienDonHang = 0; // Tổng tiền của toàn bộ đơn nhập
                    string maHDN = dgvhangnhap.Rows[0].Cells["sMaHDN"].Value.ToString(); // Mã nhập hàng
                    string maNCC = dgvhangnhap.Rows[0].Cells["sMaNCC"].Value.ToString();
                    DateTime ngayNhap = Convert.ToDateTime(dgvhangnhap.Rows[0].Cells["dNgayNhap"].Value);

                    // 1️⃣ Chèn vào bảng btlDonNhapHang trước
                    string insertDNH = @"
            INSERT INTO btlDonNhapHang (sMaNhapHang, sMaNCC, sMaNV, dNgayNhapHang, iTongTien) 
            VALUES (@MaHDN, @MaNCC,@MaNV, @NgayNhap, @TongTien)";

                    using (SqlCommand cmd = new SqlCommand(insertDNH, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaHDN", maHDN);
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                        cmd.Parameters.AddWithValue("@TongTien", 0); // Tổng tiền cập nhật sau
                        cmd.ExecuteNonQuery();
                    }

                    // 2️⃣ Chèn dữ liệu vào btlChiTietDonNhapHang
                    foreach (DataGridViewRow row in dgvhangnhap.Rows)
                    {
                        if (row.Cells["sMaMH"].Value != null && row.Cells["iSoLuong"].Value != null)
                        {
                            string maMH = row.Cells["sMaMH"].Value.ToString();
                            int soLuongNhap = Convert.ToInt32(row.Cells["iSoLuong"].Value);
                            string size = row.Cells["sSize"].Value.ToString();
                            string mauSac = row.Cells["sMauSac"].Value.ToString();

                            // 🔹 Lấy giá nhập từ btlMatHang
                            decimal giaNhap = 0;
                            string queryGiaNhap = "SELECT fGiaHang FROM btlMatHang WHERE sMaMH = @MaMH";

                            using (SqlCommand cmdGia = new SqlCommand(queryGiaNhap, conn, transaction))
                            {
                                cmdGia.Parameters.AddWithValue("@MaMH", maMH);
                                object result = cmdGia.ExecuteScalar();
                                if (result != null)
                                {
                                    giaNhap = Convert.ToDecimal(result);
                                }
                            }

                            // 🔹 Tính tổng tiền sản phẩm
                            decimal tongTienSP = giaNhap * soLuongNhap;
                            tongTienDonHang += tongTienSP;

                            // 3️⃣ Cập nhật số lượng trong btlChiTietMatHang
                            string updateCTMH = @"UPDATE btlChiTietMatHang SET iSoLuong = iSoLuong + @SoLuong  WHERE sMaMH = @MaMH AND sSize = @Size AND sMauSac = @MauSac";

                            using (SqlCommand cmd = new SqlCommand(updateCTMH, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SoLuong", soLuongNhap);
                                cmd.Parameters.AddWithValue("@MaMH", maMH);
                                cmd.Parameters.AddWithValue("@Size", size);
                                cmd.Parameters.AddWithValue("@MauSac", mauSac);
                                cmd.ExecuteNonQuery();
                            }
                            // Kiểm tra xem sMaMH có tồn tại trong btlMatHang không
                            string checkMaMH = "SELECT COUNT(*) FROM btlMatHang WHERE sMaMH = @MaMH";
                            using (SqlCommand cmdCheck = new SqlCommand(checkMaMH, conn, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@MaMH", maMH);
                                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                                if (count == 0)
                                {
                                    MessageBox.Show($"Mã sản phẩm {maMH} không tồn tại trong btlMatHang. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction.Rollback();
                                    return;
                                }
                            }

                            // 4️⃣ Lưu vào btlChiTietDonNhapHang
                            string insertCTDNH = @"
                    INSERT INTO btlChiTietDonNhapHang (sMaNhapHang, sMaMH, fSoLuongNhap, iGiaNhap, sSize, sMauSac) 
                    VALUES (@MaHDN, @MaMH, @SoLuong, @GiaNhap, @Size, @MauSac)";

                            using (SqlCommand cmd = new SqlCommand(insertCTDNH, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaHDN", maHDN);
                                cmd.Parameters.AddWithValue("@MaMH", maMH);
                                cmd.Parameters.AddWithValue("@SoLuong", soLuongNhap);
                                cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                                cmd.Parameters.AddWithValue("@Size", size);
                                cmd.Parameters.AddWithValue("@MauSac", mauSac);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // 5️⃣ Cập nhật tổng tiền trong btlDonNhapHang
                    string updateTongTien = @"
            UPDATE btlDonNhapHang 
            SET iTongTien = @TongTien 
            WHERE sMaNhapHang = @MaHDN";

                    using (SqlCommand cmd = new SqlCommand(updateTongTien, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TongTien", tongTienDonHang);
                        cmd.Parameters.AddWithValue("@MaHDN", maHDN);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Xác nhận nhập hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 🔹 Xóa dữ liệu nhập hàng
                    dgvhangnhap.Rows.Clear();
                    txbtongtien.Text = "0";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txbsoluongnhap_TextChanged(object sender, EventArgs e)
        {

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
        private void dgvdssp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Đảm bảo không phải tiêu đề cột
            {
                DataGridViewRow row = dgvdssp.Rows[e.RowIndex];

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

                textBox_Mamathang.Text = row.Cells["Mã mặt hàng"].Value.ToString();
                textBox_Maloaihang.Text = row.Cells["Mã loại hàng"].Value.ToString();
                comboBox_Tenloaihang.Text = row.Cells["Loại hàng"].Value.ToString();
                txbtenmathang.Text = row.Cells["Tên sản phẩm"].Value.ToString();
                txbsoluong.Text = row.Cells["Tổng số lượng"].Value.ToString();
                txbgianhap.Text = row.Cells["Giá hàng"].Value.ToString();
                txbchatlieu.Text = row.Cells["Chất liệu"].Value.ToString();
                txbsize.Text = sizeCell.Value?.ToString();
                txbmausac.Text = mauSacCell.Value?.ToString();
            }
        }

        private void cbtenloaihang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Tenloaihang.SelectedValue != null)
            {
                string tenLoaiHang = comboBox_Tenloaihang.SelectedValue.ToString();
                LoadMatHangTheoLoai(tenLoaiHang);
            }
        }

        private void LoadMatHangTheoLoai(string maLoaiHang)
        {
            string query = "SELECT * FROM v_MatHang_ChiTiet WHERE [Loại hàng] = @tenLoaiHang";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tenLoaiHang", GetTenLoaiHang(maLoaiHang));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvdssp.DataSource = dt;
            }
        }

        private string GetTenLoaiHang(string maLoaiHang)
        {
            string query = "SELECT sTenLoaiHang FROM btlLoaiHang WHERE sMaLoaiHang = @maLoaiHang";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maLoaiHang", maLoaiHang);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }

        private void cbnhacungcap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbnhacungcap.SelectedValue != null)
            {
                string tenNCC = cbnhacungcap.SelectedValue.ToString();
                LoadMatHangTheoNCC(tenNCC);
            }
        }
        private void LoadMatHangTheoNCC(string maNCC)
        {
            string query = "SELECT * FROM v_MatHang_ChiTiet WHERE [Tên nhà cung cấp] = @tenNCC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tenNCC", GetTenNCC(maNCC));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvdssp.DataSource = dt;
            }
        }

        private string GetTenNCC(string maNCC)
        {
            string query = "SELECT sTenNCC FROM btlNhaCungCap WHERE sMaNCC = @maNCC";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maNCC", maNCC);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }

        private void dgvdssp_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvmathang.Refresh();
            // Kiểm tra nếu chưa có cột "Kích thước" và "Màu sắc" thì thêm vào DataGridView
            if (!dgvdssp.Columns.Contains("sSize"))
            {
                DataGridViewComboBoxColumn sizeColumn = new DataGridViewComboBoxColumn
                {
                    Name = "sSize",
                    HeaderText = "Kích thước",
                    DataPropertyName = "sSize"
                };
                dgvdssp.Columns.Add(sizeColumn);
            }

            if (!dgvdssp.Columns.Contains("sMauSac"))
            {
                DataGridViewComboBoxColumn mauSacColumn = new DataGridViewComboBoxColumn
                {
                    Name = "sMauSac",
                    HeaderText = "Màu sắc",
                    DataPropertyName = "sMauSac"
                };
                dgvdssp.Columns.Add(mauSacColumn);
            }

            // Duyệt qua từng dòng để cập nhật danh sách kích thước & màu sắc
            foreach (DataGridViewRow row in dgvdssp.Rows)
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

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvdssp.CurrentCell == null) return;

            int rowIndex = dgvdssp.CurrentCell.RowIndex;
            int columnIndex = dgvdssp.CurrentCell.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                DataGridViewRow row = dgvdssp.Rows[rowIndex];

                // Nếu cột hiện tại là fSize, cập nhật txbsize
                if (dgvdssp.Columns[columnIndex].Name == "sSize")
                {
                    txbsize.Text = row.Cells["sSize"].Value?.ToString();
                }
                // Nếu cột hiện tại là sMauSac, cập nhật txbmausac
                else if (dgvdssp.Columns[columnIndex].Name == "sMauSac")
                {
                    txbmausac.Text = row.Cells["sMauSac"].Value?.ToString();
                }
            }
        }
        private void dgvdssp_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged; // Tránh đăng ký nhiều lần
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void f_nhaphang_Load_1(object sender, EventArgs e)
        {

        }
    }
}
