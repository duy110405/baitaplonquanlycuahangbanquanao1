using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplonquanlycuahangbanquanao
{
    public partial class f_doanhthu_thu : Form
    {
        private string maHD;
        hamdungchung dc = new hamdungchung();
        public f_doanhthu_thu(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void f_doanhthu_thu_Load(object sender, EventArgs e)
        {
            LocChiTietHoaDon(maHD); // Gọi hàm lọc khi form load
        }

        private void LocChiTietHoaDon(string maHD)
        {
            if (string.IsNullOrEmpty(maHD))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dieuKien = $"[Mã hóa đơn] like '%{maHD}'";
            DataTable dt = dc.getTableCoDieuKien("v_ChiTietHoaDon", dieuKien);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridView_chitiethoadon.DataSource = dt;
        }

        private void dataGridView_chitiethoadon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
