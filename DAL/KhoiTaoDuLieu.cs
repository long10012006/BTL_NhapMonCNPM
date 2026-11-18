using System;
using System.IO;
using SanLichWeb.Helpers;
namespace SanLichWeb.DAL
{
    public static class KhoiTaoDuLieu
    {
        private static readonly object Khoa = new object();
        public static void Chay()
        {
            lock (Khoa)
            {
                Directory.CreateDirectory(Database.ThuMuc);
                using (var db = new Database())
                {
                    db.Exec("PRAGMA journal_mode=WAL;");
                    db.BatDau();
                    db.Exec(File.ReadAllText(Path.Combine(Database.ThuMuc, "Schema.sql")));
                    // Chỉ tạo tài khoản mẫu khi CSDL hoàn toàn mới.
                    if (Convert.ToInt32(db.Scalar("SELECT COUNT(*) FROM ACCOUNT")) == 0)
                    {
                        int manager = TaiKhoanDAL.Them(db, "Quản lý mẫu", "0900000001", MatKhau.Bam("QuanLy@123"), "MANAGER");
                        TaiKhoanDAL.Them(db, "Khách hàng mẫu", "0900000002", MatKhau.Bam("Khach@123"), "CUSTOMER");
                        long[] rates = { 220000, 250000, 220000, 260000, 230000, 270000, 220000, 260000 };
                        for (int i = 0; i < 11; i++)
                        {
                            DateTime time = DateTime.Today.AddHours(6).AddMinutes(i * 90);
                            db.Exec("INSERT INTO TIME_SLOT(slot_code,start_time,end_time) VALUES(@p0,@p1,@p2)",
                                "H" + time.ToString("HHmm"), time.ToString("HH:mm:ss"), time.AddMinutes(90).ToString("HH:mm:ss"));
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            int id = SanDAL.LuuSan(db, 0, 0, "SAN" + (i + 1).ToString("00"), "Sân " + (i + 1).ToString("00"),
                                "Sân bóng đá mini · " + (i % 2 == 0 ? "5 người" : "7 người"), false);
                            for (int slot = 1; slot <= 11; slot++)
                                SanDAL.ThemGia(db, id, slot, "2020-01-01", null, rates[i], manager);
                        }
                    }
                    SanDAL.SinhLich(db, QuyTac.BayGio.Date);
                    db.Luu();
                }
            }
        }
    }
}
