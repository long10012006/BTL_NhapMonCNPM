using System;
using System.Data;
namespace SanLichWeb.Models
{
    public class LichSan
    {
        public int Id { get; set; }
        public int SanId { get; set; }
        public int GiaId { get; set; }
        public string TenSan { get; set; }
        public DateTime BatDau { get; set; }
        public DateTime KetThuc { get; set; }
        public long GiaGio { get; set; }
        public long TongTien { get { return (GiaGio * 90 + 30) / 60; } }
        public bool DangMo { get; set; }
        public bool DaDat { get; set; }
        public bool SanAn { get; set; }
        public static LichSan TuDong(DataRow r)
        {
            return new LichSan
            {
                Id = Convert.ToInt32(r["schedule_id"]),
                SanId = Convert.ToInt32(r["court_id"]),
                GiaId = r.IsNull("price_id") ? 0 : Convert.ToInt32(r["price_id"]),
                TenSan = Convert.ToString(r["court_name"]),
                BatDau = DateTime.Parse(Convert.ToString(r["play_date"]) + " " + r["start_time"], System.Globalization.CultureInfo.InvariantCulture),
                KetThuc = DateTime.Parse(Convert.ToString(r["play_date"]) + " " + r["end_time"], System.Globalization.CultureInfo.InvariantCulture),
                GiaGio = r.IsNull("hourly_rate") ? 0 : Convert.ToInt64(r["hourly_rate"]),
                DangMo = Convert.ToInt32(r["is_open"]) == 1,
                SanAn = Convert.ToInt32(r["is_hidden"]) == 1,
                DaDat = Convert.ToInt32(r["busy"]) == 1
            };
        }
    }
}
