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
    public partial class f_doanhthu_chi : Form
    {

        private string maDNH;
        hamdungchung dc = new hamdungchung();

        public f_doanhthu_chi(string maDNH)
        {
            InitializeComponent();
            this.maDNH = maDNH;
        }

        private void f_doanhthu_chi_Load(object sender, EventArgs e)
        {
            LocChiTietDNH(maDNH);
        }

        private void LocChiTietDNH(string maDNH)
        {
            if (string.IsNullOrEmpty(maDNH))
            {
                MessageBox.Show("Vui lòng nhập mã đơn nhập hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dieuKien = $"[Mã đơn nhập hàng] like '%{maDNH}'";
            DataTable dt = dc.getTableCoDieuKien("v_ChiTietDNH", dieuKien);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridView_chitietDNH.DataSource = dt;
        }

        private void dataGridView_chitietDNH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void f_doanhthu_chi_Load_1(object sender, EventArgs e)
        {

        }
    }
}
