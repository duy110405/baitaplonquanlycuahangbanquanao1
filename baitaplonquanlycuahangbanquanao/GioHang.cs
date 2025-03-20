using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baitaplonquanlycuahangbanquanao
{
    public class GioHang
    {
        public string MaHang { get; set; }
        public string LoaiHang { get; set; }
        public string TenSanPham { get; set; }
        public string TenNCC { get; set; }
        public int SoLuong { get; set; }
        public int GiaHang { get; set; }
        public string Size { get; set; }
        public string MauSac { get; set; }
        public string ChatLieu { get; set; }

        public GioHang(string maHang, string loaiHang, string tenSanPham, string tenNCC, int soLuong, int giaHang, string size, string mauSac, string chatLieu)
        {
            MaHang = maHang;
            LoaiHang = loaiHang;
            TenSanPham = tenSanPham;
            TenNCC = tenNCC;
            SoLuong = soLuong;
            GiaHang = giaHang;
            Size = size;
            MauSac = mauSac;
            ChatLieu = chatLieu;
        }

        public override string ToString()
        {
            return $"Mã hàng: {MaHang}, Loại hàng: {LoaiHang}, Tên sản phẩm: {TenSanPham}, Tên nhà cung cấp: {TenNCC}, Số lượng: {SoLuong}, Giá: {GiaHang}, Size: {Size}, Màu sắc: {MauSac}, Chất liệu: {ChatLieu}";
        }
    }
}
