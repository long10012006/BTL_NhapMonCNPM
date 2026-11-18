using System;
using System.Data;
using System.Linq;
using SanLichWeb.Helpers;
namespace SanLichWeb.BLL
{
    public class KetQuaThongKe
    {
        public int SoLuot { get; set; }
        public long GiaTriDuKien { get; set; }
        public DataTable TheoTrangThai { get; set; }
    }
    public class ThongKeBLL
    {
        public KetQuaThongKe Tinh(int actor, string from, string to, int court, string status)
        {
            DataTable bookings = new DatSanBLL().DanhSach(actor, true, from, to, court, status);
            var result = new KetQuaThongKe { SoLuot = bookings.Rows.Count, TheoTrangThai = new DataTable() };
            result.TheoTrangThai.Columns.Add("status_name");
            result.TheoTrangThai.Columns.Add("count", typeof(int));
            result.TheoTrangThai.Columns.Add("amount", typeof(long));
            foreach (var group in bookings.Rows.Cast<DataRow>().GroupBy(r => r["status"].ToString()))
            {
                long amount = group.Key == "CONFIRMED" || group.Key == "COMPLETED"
                    ? group.Sum(r => Convert.ToInt64(r["total_amount"])) : 0;
                result.GiaTriDuKien += amount;
                result.TheoTrangThai.Rows.Add(QuyTac.TrangThai(group.Key), group.Count(), amount);
            }
            return result;
        }
    }
}
