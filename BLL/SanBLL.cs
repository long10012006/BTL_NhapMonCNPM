using System;
using System.Data;
using SanLichWeb.DAL;
using SanLichWeb.Models;
using SanLichWeb.Helpers;
namespace SanLichWeb.BLL
{
    public class SanBLL
    {
        public DataTable DanhSach(bool hidden, int actor)
        {
            using (var db = new Database())
            {
                if (hidden) TaiKhoanBLL.KiemTra(db, actor, true);
                return SanDAL.DanhSach(db, hidden);
            }
        }
        public DataTable KhungGio()
        {
            using (var db = new Database()) return SanDAL.KhungGio(db);
        }
        public DataTable LichNgay(DateTime date, int court, bool management, int actor)
        {
            QuyTac.Can(date.Date >= QuyTac.BayGio.Date && date.Date <= QuyTac.BayGio.Date.AddDays(14), "Chọn ngày từ hôm nay đến 14 ngày tới.");
            using (var db = new Database())
            {
                if (management) TaiKhoanBLL.KiemTra(db, actor, true);
                db.BatDau();
                SanDAL.SinhLich(db, QuyTac.BayGio.Date);
                db.Luu();
                return SanDAL.LichNgay(db, QuyTac.Ngay(date), court, management);
            }
        }
        public LichSan ChiTiet(int id)
        {
            using (var db = new Database()) return SanDAL.TheoId(db, id);
        }
    }
}
