using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplonquanlycuahangbanquanao
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Chỉ mở form đăng nhập một lần
            //string tenNguoiDung = null;

            //using (f_dangnhap loginForm = new f_dangnhap())
            //{
            //    if (loginForm.ShowDialog() == DialogResult.OK) // Chỉ chạy 1 lần
            //    {
            //        tenNguoiDung = loginForm.TenNguoiDung;
            //    }
            //    else
            //    {
            //        return; // Nếu đăng nhập thất bại, thoát chương trình
            //    }
            //}

            //// Chạy f_main với tên người dùng sau khi đăng nhập thành công
            //Application.Run(new f_main(tenNguoiDung));
            f_dangnhap loginForm = new f_dangnhap();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Nếu đăng nhập thành công, lấy thông tin nhân viên
                string maNV = loginForm.MaNhanVien;
                string tenNV = loginForm.TenNguoiDung;

                // Khởi tạo f_nhaphang với thông tin nhân viên
                //f_nhaphang nhapHangForm = new f_nhaphang(maNV, tenNV);

                // Mở f_main và truyền form nhập hàng vào
                f_main mainForm = new f_main(maNV, tenNV);

                Application.Run(mainForm); // Chạy form chính
            }
        }
    }

    }
