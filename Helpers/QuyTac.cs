using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SanLichWeb.Models;
namespace SanLichWeb.Helpers
{
    public class LoiNghiepVu : Exception
    {
        public LoiNghiepVu(string message) : base(message) { }
    }
    public static class QuyTac
    {
        // Đồng hồ có thể cố định trong bộ kiểm thử; ứng dụng luôn dùng UTC máy chủ.
        internal static Func<DateTime> LayUtc = () => DateTime.UtcNow;
        public static DateTime BayGio { get { return LayUtc().AddHours(7); } }
        public static string Utc { get { return LayUtc().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); } }
        public static string Ngay(DateTime value) { return value.ToString("yyyy-MM-dd"); }
        public static string DienThoai(string value)
        {
            string phone = Regex.Replace(value ?? "", @"[\s.()\-]", "");
            if (!Regex.IsMatch(phone, @"^0[0-9]{9}$"))
                throw new LoiNghiepVu("Số điện thoại phải gồm 10 chữ số, bắt đầu bằng 0.");
            return phone;
        }
        public static void Can(bool condition, string message)
        {
            if (!condition) throw new LoiNghiepVu(message);
        }
        public static void KiemTraLich(LichSan lich)
        {
            Can(lich != null, "Không tìm thấy lịch sân.");
            Can(!lich.SanAn && lich.DangMo, "Sân đang ẩn hoặc khung giờ đang khóa.");
            Can(lich.BatDau > BayGio && lich.BatDau.Date <= BayGio.Date.AddDays(14), "Chỉ đặt giờ tương lai trong 14 ngày tới.");
            Can(!lich.DaDat, "Khung giờ vừa có người đặt. Hãy chọn giờ khác.");
            Can(lich.GiaId > 0 && lich.GiaGio > 0, "Chưa có giá hợp lệ cho khung này.");
        }
        public static string TrangThai(string code)
        {
            switch (code)
            {
                case "CONFIRMED": return "Đã xác nhận";
                case "CANCELLED": return "Đã hủy";
                case "COMPLETED": return "Hoàn thành";
                case "NO_SHOW": return "Không đến";
                default: return code;
            }
        }
        public static string Tien(object value)
        {
            return Convert.ToInt64(value).ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }
    }
}
