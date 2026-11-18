using System;
using SanLichWeb.DAL;
using SanLichWeb.Models;
using SanLichWeb.Helpers;
namespace SanLichWeb.BLL
{
    public class TaiKhoanBLL
    {
        public NguoiDung DangNhap(string phone, string password)
        {
            phone = QuyTac.DienThoai(phone);
            using (var db = new Database())
            {
                var row = TaiKhoanDAL.TheoSo(db, phone);
                if (row == null || Convert.ToInt32(row["is_active"]) != 1 || !MatKhau.Dung(password, Convert.ToString(row["password_hash"])))
                    throw new LoiNghiepVu("Số điện thoại hoặc mật khẩu không đúng.");
                return TaiKhoanDAL.Doc(row);
            }
        }
        public void DangKy(string name, string phone, string password, string repeat)
        {
            name = (name ?? "").Trim();
            phone = QuyTac.DienThoai(phone);
            QuyTac.Can(name.Length >= 2 && name.Length <= 100, "Họ tên từ 2 đến 100 ký tự.");
            QuyTac.Can(password != null && password.Length >= 8 && password.Length <= 128, "Mật khẩu từ 8 đến 128 ký tự.");
            QuyTac.Can(password == repeat, "Mật khẩu nhập lại chưa khớp.");
            string hash = MatKhau.Bam(password);
            using (var db = new Database())
            {
                db.BatDau();
                QuyTac.Can(TaiKhoanDAL.TheoSo(db, phone) == null, "Số điện thoại đã được đăng ký.");
                int id = TaiKhoanDAL.Them(db, name, phone, hash, "CUSTOMER");
                NhatKyDAL.Ghi(db, id, "REGISTER", "ACCOUNT", id, null, new { HoTen = name, VaiTro = "CUSTOMER" });
                db.Luu();
            }
        }
        public NguoiDung HienTai(int id)
        {
            using (var db = new Database()) return TaiKhoanDAL.TheoId(db, id);
        }
        public static NguoiDung KiemTra(Database db, int id, bool manager)
        {
            var user = TaiKhoanDAL.TheoId(db, id);
            QuyTac.Can(user != null, "Phiên đăng nhập không hợp lệ. Hãy đăng nhập lại.");
            QuyTac.Can(!manager || user.LaQuanLy, "Chỉ quản lý được thực hiện thao tác này.");
            return user;
        }
    }
}
