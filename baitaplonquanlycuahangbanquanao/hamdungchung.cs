using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace baitaplonquanlycuahangbanquanao
{ 
    
    internal class hamdungchung
    {
       string constr = ConfigurationManager.ConnectionStrings["qlbtl"].ToString();
        public SqlConnection cnn = new SqlConnection();

        

        public static bool thuchiendoanmasql(string constr, string sqlinsert)
        {
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlinsert, cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    int i = cmd.ExecuteNonQuery();
                    cnn.Close();
                    return i > 0;
                }
            }
        }

        public void loadcombobox(string tenbang, ComboBox cmb, string valueMember, string displayMember)
        {
            DataTable tbl = getTable(tenbang);

            if (tbl.Columns.Contains(valueMember) && tbl.Columns.Contains(displayMember))
            {
                cmb.DataSource = tbl;
                cmb.ValueMember = valueMember;
                cmb.DisplayMember = displayMember;
            }
            else
            {
                MessageBox.Show("Cột không tồn tại trong bảng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool ketnoi()
        {
            try
            {
                if (cnn.State == System.Data.ConnectionState.Open) cnn.Close();
                cnn.ConnectionString = constr;
                cnn.Open();
            }
            catch
            {
                MessageBox.Show("loi ket noi");
                return false;
            }
            return true;

        }

        public DataTable getTable(string tenbang)
        {
            string sql = "Select * from " + tenbang;
            SqlDataAdapter ad = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;

        }

        public void loadgridview(string bangmuonnhapjdulieu, DataGridView dgv)
        {
            if (ketnoi() == false)
            {
                return;
            }
            try
            {
                DataTable dt = getTable(bangmuonnhapjdulieu);
                dgv.DataSource = dt;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.Dispose();
            }
            catch { MessageBox.Show("box"); }
        }


        public void loadcombobox(string tenbang, ComboBox cmb)
        {
            DataTable tbl = getTable(tenbang);
            cmb.DataSource = tbl;
            cmb.ValueMember = tbl.Columns[0].ColumnName;
            cmb.DisplayMember = tbl.Columns[1].ColumnName;
        }

        public static bool thuchienkiemtrakhoachinh(string constr, int Ma, string bangthuchien)
        {
            string sqlinsert = $"SELECT * from dbo.tblSinhVien Where '{bangthuchien}' = '{Ma}' ";
            bool check;
            check = false;
            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlinsert, cnn))
                {
                    cnn.Open();
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read() == true) check = true;
                        else check = false;
                        rd.Close();
                    }
                    cnn.Close();
                }
            }
            Console.WriteLine("check");
            Console.WriteLine(check);
            return check;
        }

        public void CapNhatSoLuongMatHang(string maMH, int soLuongThayDoi)
        {
            //if (cnn.State == ConnectionState.Closed)
            //{
            //    ketnoi(); // Đảm bảo kết nối đã mở
            //}

            
            string query = "UPDATE btlMatHang SET iSoluong = iSoluong + " + soLuongThayDoi + " WHERE sMaMH = '" + maMH + "'";

            thuchiendoanmasql(constr, query);
            //using (SqlCommand cmd = new SqlCommand(query, cnn))
            //{
            //    cmd.Parameters.AddWithValue("@soLuongThayDoi", soLuongThayDoi);
            //    cmd.Parameters.AddWithValue("@maMH", maMH);
            //    cmd.ExecuteNonQuery();
            //}


        }

        public DataTable getTableCoDieuKien(string tenBang, string dieuKien)
        {
            DataTable dt = new DataTable();
            string query = $"SELECT * FROM {tenBang}";

            if (!string.IsNullOrEmpty(dieuKien))
            {
                query += " WHERE " + dieuKien;
            }

            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public void CapNhatSoLuongMatHang(string maMH, int soLuongThayDoi, string mausac, string size)
        {
            //if (cnn.State == ConnectionState.Closed)
            //{
            //    ketnoi(); // Đảm bảo kết nối đã mở
            //}


            //string query = "UPDATE btlMatHang SET iSoluong = iSoluong + " + soLuongThayDoi + " WHERE sMaMH = '" + maMH + "' ";

            //thuchiendoanmasql(constr, query);
            //using (SqlCommand cmd = new SqlCommand(query, cnn))
            //{
            //    cmd.Parameters.AddWithValue("@soLuongThayDoi", soLuongThayDoi);
            //    cmd.Parameters.AddWithValue("@maMH", maMH);
            //    cmd.ExecuteNonQuery();
            //}
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string query = "UPDATE btlChiTietMatHang " +
                               "SET iSoLuong = iSoLuong + @soLuongThayDoi " +
                               "WHERE sMaMH = @maMH AND sMauSac = @mauSac AND sSize = @size";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@soLuongThayDoi", soLuongThayDoi);
                    cmd.Parameters.AddWithValue("@maMH", maMH);
                    cmd.Parameters.AddWithValue("@mauSac", mausac);
                    cmd.Parameters.AddWithValue("@size", size);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để cập nhật số lượng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }

    }
}
