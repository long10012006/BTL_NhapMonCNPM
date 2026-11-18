using System;
using System.Data;
using SanLichWeb.Models;
using SanLichWeb.Helpers;
namespace SanLichWeb.DAL
{
    public static class SanDAL
    {
        private const string LichSql = @"SELECT s.*,c.court_name,c.description,c.is_hidden,t.start_time,t.end_time,
            p.price_id,p.hourly_rate,
            CASE WHEN EXISTS(SELECT 1 FROM BOOKING b WHERE b.schedule_id=s.schedule_id AND b.status='CONFIRMED') THEN 1 ELSE 0 END busy
            FROM COURT_SCHEDULE s JOIN COURT c ON c.court_id=s.court_id
            JOIN TIME_SLOT t ON t.slot_id=s.slot_id
            LEFT JOIN COURT_PRICE p ON p.court_id=s.court_id AND p.slot_id=s.slot_id
            AND p.valid_from<=s.play_date AND (p.valid_to IS NULL OR p.valid_to>=s.play_date) ";
        public static DataTable DanhSach(Database db, bool includeHidden)
        {
            return db.Query("SELECT * FROM COURT WHERE (@p0=1 OR is_hidden=0) ORDER BY court_id", includeHidden ? 1 : 0);
        }
        public static DataTable KhungGio(Database db)
        {
            return db.Query("SELECT *,substr(start_time,1,5)||' – '||substr(end_time,1,5) label FROM TIME_SLOT WHERE is_active=1 ORDER BY start_time");
        }
        public static void SinhLich(Database db, DateTime today)
        {
            for (int i = 0; i <= 14; i++)
            {
                db.Exec(@"INSERT OR IGNORE INTO COURT_SCHEDULE(court_id,slot_id,play_date,created_at_utc)
                    SELECT c.court_id,t.slot_id,@p0,@p1 FROM COURT c CROSS JOIN TIME_SLOT t WHERE c.is_hidden=0 AND t.is_active=1",
                    QuyTac.Ngay(today.AddDays(i)), QuyTac.Utc);
            }
        }
        public static DataTable LichNgay(Database db, string date, int court, bool management)
        {
            return db.Query(LichSql + @" WHERE s.play_date=@p0 AND (@p1=0 OR s.court_id=@p1)
                AND (@p2=1 OR c.is_hidden=0) ORDER BY c.court_id,t.start_time", date, court, management ? 1 : 0);
        }
        public static LichSan TheoId(Database db, int id)
        {
            var table = db.Query(LichSql + " WHERE s.schedule_id=@p0", id);
            return table.Rows.Count == 0 ? null : LichSan.TuDong(table.Rows[0]);
        }
        public static DataRow SanTheoId(Database db, int id)
        {
            var table = db.Query("SELECT * FROM COURT WHERE court_id=@p0", id);
            return table.Rows.Count == 0 ? null : table.Rows[0];
        }
        public static int LuuSan(Database db, int id, int version, string code, string name, string description, bool hidden)
        {
            if (id == 0)
            {
                db.Exec("INSERT INTO COURT(court_code,court_name,description,is_hidden) VALUES(@p0,@p1,@p2,@p3)", code, name, description, hidden ? 1 : 0);
                return Convert.ToInt32(db.Scalar("SELECT last_insert_rowid()"));
            }
            QuyTac.Can(db.Exec(@"UPDATE COURT SET court_code=@p0,court_name=@p1,description=@p2,is_hidden=@p3,version=version+1
                WHERE court_id=@p4 AND version=@p5", code, name, description, hidden ? 1 : 0, id, version) == 1, "Sân đã thay đổi. Hãy tải lại trang.");
            return id;
        }
        public static DataTable BangGia(Database db)
        {
            return db.Query(@"SELECT p.*,c.court_name,substr(t.start_time,1,5)||' – '||substr(t.end_time,1,5) label
                FROM COURT_PRICE p JOIN COURT c ON c.court_id=p.court_id JOIN TIME_SLOT t ON t.slot_id=p.slot_id
                ORDER BY p.valid_from DESC,c.court_id,t.start_time");
        }
        public static void ThemGia(Database db, int court, int slot, string from, string to, long rate, int actor)
        {
            db.Exec(@"INSERT INTO COURT_PRICE(court_id,slot_id,valid_from,valid_to,hourly_rate,created_by_account_id,created_at_utc)
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)", court, slot, from, to, rate, actor, QuyTac.Utc);
        }
        public static DataTable GiaSauNgay(Database db, int court, int slot, string from)
        {
            return db.Query(@"SELECT * FROM COURT_PRICE WHERE court_id=@p0 AND slot_id=@p1
                AND (valid_to IS NULL OR valid_to>=@p2) ORDER BY valid_from", court, slot, from);
        }
        public static void KetThucGia(Database db, int id, string date)
        {
            db.Exec("UPDATE COURT_PRICE SET valid_to=@p0 WHERE price_id=@p1", date, id);
        }
        public static void KhoaMo(Database db, int id, bool open, string reason)
        {
            db.Exec("UPDATE COURT_SCHEDULE SET is_open=@p0,lock_reason=@p1,version=version+1 WHERE schedule_id=@p2", open ? 1 : 0, reason, id);
        }
    }
}
