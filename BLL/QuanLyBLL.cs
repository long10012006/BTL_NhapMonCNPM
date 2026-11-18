using System;
using System.Data;
using SanLichWeb.DAL;
using SanLichWeb.Helpers;
namespace SanLichWeb.BLL
{
    public class QuanLyBLL
    {
        public DataRow San(int actor, int id)
        {
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actor, true);
                return SanDAL.SanTheoId(db, id);
            }
        }
        public void LuuSan(int actor, int id, int version, string code, string name, string description, bool hidden)
        {
            code = (code ?? "").Trim(); name = (name ?? "").Trim();
            QuyTac.Can(System.Text.RegularExpressions.Regex.IsMatch(code, @"^[A-Za-z0-9_-]{2,20}$"), "Mã sân gồm 2–20 chữ/số, dấu gạch.");
            QuyTac.Can(name.Length >= 2 && name.Length <= 100 && description.Length <= 500, "Tên sân 2–100 ký tự, mô tả tối đa 500 ký tự.");
            using (var db = new Database())
            {
                db.BatDau();
                TaiKhoanBLL.KiemTra(db, actor, true);
                DataRow before = id == 0 ? null : SanDAL.SanTheoId(db, id);
                object oldValue = before == null ? null : new { Ma = before["court_code"], Ten = before["court_name"], MoTa = before["description"], An = before["is_hidden"] };
                id = SanDAL.LuuSan(db, id, version, code, name, description, hidden);
                NhatKyDAL.Ghi(db, actor, "SAVE_COURT", "COURT", id, oldValue, new { Ma = code, Ten = name, MoTa = description, An = hidden });
                SanDAL.SinhLich(db, QuyTac.BayGio.Date);
                db.Luu();
            }
        }
        public DataTable BangGia(int actor)
        {
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actor, true);
                return SanDAL.BangGia(db);
            }
        }
        public void GiaMoi(int actor, int court, int slot, DateTime from, long rate)
        {
            QuyTac.Can(from.Date >= QuyTac.BayGio.Date, "Ngày hiệu lực không được ở quá khứ.");
            QuyTac.Can(rate > 0 && rate <= 100000000, "Đơn giá phải từ 1 đến 100.000.000 VNĐ/giờ.");
            using (var db = new Database())
            {
                db.BatDau();
                TaiKhoanBLL.KiemTra(db, actor, true);
                QuyTac.Can(SanDAL.SanTheoId(db, court) != null, "Sân không tồn tại.");
                string date = QuyTac.Ngay(from);
                DataTable prices = SanDAL.GiaSauNgay(db, court, slot, date);
                foreach (DataRow row in prices.Rows)
                    QuyTac.Can(string.CompareOrdinal(Convert.ToString(row["valid_from"]), date) < 0, "Đã có giá từ ngày này hoặc một ngày sau đó. Hãy chọn ngày sau mức giá cuối.");
                foreach (DataRow row in prices.Rows)
                {
                    SanDAL.KetThucGia(db, Convert.ToInt32(row["price_id"]), QuyTac.Ngay(from.AddDays(-1)));
                    NhatKyDAL.Ghi(db, actor, "END_PRICE", "COURT_PRICE", row["price_id"],
                        new { DenNgay = row["valid_to"].ToString(), Gia = row["hourly_rate"] }, new { DenNgay = QuyTac.Ngay(from.AddDays(-1)) });
                }
                SanDAL.ThemGia(db, court, slot, date, null, rate, actor);
                NhatKyDAL.Ghi(db, actor, "NEW_PRICE", "COURT_PRICE", court + ":" + slot, null, new { TuNgay = date, GiaGio = rate });
                db.Luu();
            }
        }
        public void KhoaMo(int actor, int id, bool open, string reason)
        {
            using (var db = new Database())
            {
                db.BatDau();
                TaiKhoanBLL.KiemTra(db, actor, true);
                var lich = SanDAL.TheoId(db, id);
                QuyTac.Can(lich != null && lich.BatDau > QuyTac.BayGio, "Chỉ đổi trạng thái lịch chưa bắt đầu.");
                QuyTac.Can(open || !lich.DaDat, "Không được khóa khung đang có lượt Đã xác nhận. Hãy xử lý lượt trước.");
                QuyTac.Can(open || (!string.IsNullOrWhiteSpace(reason) && reason.Length <= 500), "Nhập lý do khóa, tối đa 500 ký tự.");
                SanDAL.KhoaMo(db, id, open, open ? null : reason.Trim());
                NhatKyDAL.Ghi(db, actor, open ? "OPEN_SLOT" : "LOCK_SLOT", "COURT_SCHEDULE", id,
                    new { Mo = lich.DangMo }, new { Mo = open, LyDo = reason });
                db.Luu();
            }
        }
        public DataTable NhatKy(int actor)
        {
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actor, true);
                return NhatKyDAL.DanhSach(db);
            }
        }
    }
}
