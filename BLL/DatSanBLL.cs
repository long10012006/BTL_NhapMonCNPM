using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using SanLichWeb.DAL;
using SanLichWeb.Models;
using SanLichWeb.Helpers;
namespace SanLichWeb.BLL
{
    public class DatSanBLL
    {
        public int Tao(int actorId, int scheduleId, long expectedPrice, string requestKey,
            string source, string name, string phone)
        {
            Guid key;
            QuyTac.Can(Guid.TryParse(requestKey, out key), "Mã yêu cầu không hợp lệ. Hãy tải lại trang.");
            QuyTac.Can(source == "WEB" || source == "PHONE" || source == "COUNTER", "Nguồn tiếp nhận không hợp lệ.");
            using (var db = new Database())
            {
                db.BatDau();
                NguoiDung actor = TaiKhoanBLL.KiemTra(db, actorId, source != "WEB");
                if (source == "WEB")
                {
                    QuyTac.Can(!actor.LaQuanLy, "Quản lý hãy dùng mục Nhập lượt trực tiếp.");
                    name = actor.HoTen;
                    phone = actor.SoDienThoai;
                }
                name = (name ?? "").Trim();
                phone = QuyTac.DienThoai(phone);
                QuyTac.Can(name.Length >= 2 && name.Length <= 100, "Tên liên hệ từ 2 đến 100 ký tự.");
                string payload = new JavaScriptSerializer().Serialize(new { scheduleId, expectedPrice, source, name, phone });
                byte[] hash;
                using (var sha = SHA256.Create()) hash = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));
                DataRow old = DatSanDAL.TheoKhoa(db, actorId, requestKey);
                if (old != null)
                {
                    QuyTac.Can(((byte[])old["request_payload_hash"]).SequenceEqual(hash), "Mã yêu cầu đã dùng cho nội dung khác. Hãy tải lại trang.");
                    int existing = Convert.ToInt32(old["booking_id"]);
                    db.Luu();
                    return existing;
                }
                LichSan lich = SanDAL.TheoId(db, scheduleId);
                QuyTac.KiemTraLich(lich);
                QuyTac.Can(lich.TongTien == expectedPrice, "Giá vừa thay đổi. Hãy tải lại để xem và xác nhận giá mới.");
                int id = DatSanDAL.Them(db, actor, lich, name, phone, source, requestKey, hash);
                DatSanDAL.LichSu(db, id, actorId, "CREATE", null, DatSanDAL.TheoId(db, id), null);
                db.Luu();
                return id;
            }
        }
        public DataTable DanhSach(int actorId, bool management, string from, string to, int court, string status)
        {
            DateTime start, end;
            QuyTac.Can(DateTime.TryParse(from, out start) && DateTime.TryParse(to, out end), "Ngày lọc không hợp lệ.");
            start = DateTime.Parse(from); end = DateTime.Parse(to);
            QuyTac.Can(start <= end, "Ngày bắt đầu phải trước hoặc bằng ngày kết thúc.");
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actorId, management);
                return DatSanDAL.DanhSach(db, management ? 0 : actorId, QuyTac.Ngay(start), QuyTac.Ngay(end), court, status ?? "");
            }
        }
        public DataRow ChiTiet(int actorId, int id, bool management)
        {
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actorId, management);
                DataRow row = DatSanDAL.TheoId(db, id);
                KiemTraChu(row, actorId, management);
                return row;
            }
        }
        private static void KiemTraChu(DataRow row, int actor, bool management)
        {
            QuyTac.Can(row != null, "Không tìm thấy lượt đặt.");
            QuyTac.Can(management || (!row.IsNull("customer_account_id") && Convert.ToInt32(row["customer_account_id"]) == actor), "Bạn không có quyền xem hoặc sửa lượt này.");
        }
        public DataTable LichSu(int actorId, int id, bool management)
        {
            using (var db = new Database())
            {
                TaiKhoanBLL.KiemTra(db, actorId, management);
                KiemTraChu(DatSanDAL.TheoId(db, id), actorId, management);
                return DatSanDAL.LichSuTheoLuot(db, id);
            }
        }
        private static DateTime ThoiDiem(DataRow row, string column)
        {
            return DateTime.Parse(row["play_date"] + " " + row[column], System.Globalization.CultureInfo.InvariantCulture);
        }
        public void Doi(int actorId, int id, int version, int newSchedule, long expectedPrice)
        {
            using (var db = new Database())
            {
                db.BatDau();
                var actor = TaiKhoanBLL.KiemTra(db, actorId, false);
                QuyTac.Can(!actor.LaQuanLy, "Chức năng đổi lịch dành cho chủ lượt đặt trực tuyến.");
                DataRow old = DatSanDAL.TheoId(db, id);
                KiemTraChu(old, actorId, false);
                QuyTac.Can(Convert.ToString(old["status"]) == "CONFIRMED", "Chỉ đổi lượt Đã xác nhận.");
                QuyTac.Can(ThoiDiem(old, "start_time") >= QuyTac.BayGio.AddHours(2), "Chỉ đổi khi còn ít nhất 2 giờ trước giờ cũ.");
                QuyTac.Can(Convert.ToInt32(old["schedule_id"]) != newSchedule, "Hãy chọn lịch khác lịch cũ.");
                LichSan lich = SanDAL.TheoId(db, newSchedule);
                QuyTac.KiemTraLich(lich);
                QuyTac.Can(lich.BatDau >= QuyTac.BayGio.AddHours(2), "Lịch mới phải cách hiện tại ít nhất 2 giờ.");
                QuyTac.Can(lich.TongTien == expectedPrice, "Giá vừa thay đổi. Hãy tải lại và xác nhận giá mới.");
                DatSanDAL.Doi(db, id, version, lich);
                DatSanDAL.LichSu(db, id, actorId, "RESCHEDULE", old, DatSanDAL.TheoId(db, id), null);
                db.Luu();
            }
        }
        public void ChuyenTrangThai(int actorId, int id, int version, string status, string reason, bool management)
        {
            QuyTac.Can(status == "CANCELLED" || status == "COMPLETED" || status == "NO_SHOW", "Trạng thái không hợp lệ.");
            using (var db = new Database())
            {
                db.BatDau();
                TaiKhoanBLL.KiemTra(db, actorId, management);
                DataRow old = DatSanDAL.TheoId(db, id);
                KiemTraChu(old, actorId, management);
                if (status == "CANCELLED" && Convert.ToString(old["status"]) == "CANCELLED")
                {
                    db.Luu();
                    return;
                }
                QuyTac.Can(Convert.ToString(old["status"]) == "CONFIRMED", "Trạng thái kết thúc không được chuyển ngược.");
                if (status == "CANCELLED")
                {
                    if (management)
                    {
                        QuyTac.Can(ThoiDiem(old, "start_time") > QuyTac.BayGio, "Quản lý chỉ hủy trước giờ bắt đầu.");
                        QuyTac.Can(!string.IsNullOrWhiteSpace(reason) && reason.Trim().Length <= 500, "Nhập lý do hủy, tối đa 500 ký tự.");
                    }
                    else QuyTac.Can(ThoiDiem(old, "start_time") >= QuyTac.BayGio.AddHours(2), "Chỉ hủy khi còn ít nhất 2 giờ trước giờ bắt đầu.");
                }
                else
                {
                    QuyTac.Can(management, "Chỉ quản lý được xử lý trạng thái này.");
                    QuyTac.Can(ThoiDiem(old, "end_time") <= QuyTac.BayGio, "Chỉ ghi nhận hoàn thành/không đến sau giờ kết thúc.");
                }
                DatSanDAL.TrangThai(db, id, version, status);
                string action = status == "CANCELLED" ? "CANCEL" : status == "COMPLETED" ? "COMPLETE" : "NO_SHOW";
                DatSanDAL.LichSu(db, id, actorId, action, old, DatSanDAL.TheoId(db, id), reason);
                db.Luu();
            }
        }
    }
}
