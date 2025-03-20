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
    public partial class f_main : Form
    {
        public f_main(string maNV, string tenNV)
        {
            InitializeComponent();
            this.maNV = maNV;
            this.tenNV = tenNV;
        }



        // mới thêm
        private string maNV;
        private string tenNV;

        public string tenNguoiDung { get; private set; }  // lưu tên đăng nhập sau khi dăng nhập thành công

        private void OpenChildForm(Form childForm)
        {
            // Xóa các form con đang mở trước đó
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }

            childForm.MdiParent = this;  // Gán Form cha
            childForm.FormBorderStyle = FormBorderStyle.None;  // Ẩn viền form con
            childForm.Dock = DockStyle.Fill;  // Lấp đầy form cha
            childForm.Show();
        }

        private void f_main_Load(object sender, EventArgs e)
        {
            
        }

        private void đơnHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenChildForm(new f_donhang(maNV));
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void hàngHoáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_hanghoa());
        }
        
        private void đốiTácToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_nhacungcap());
        }

        private void nhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_nhaphang(maNV,tenNV));

        }


        private void doanhSốToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_doanhthu());
        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_nhanvien());
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_khachhang());
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new f_sanpham());
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
