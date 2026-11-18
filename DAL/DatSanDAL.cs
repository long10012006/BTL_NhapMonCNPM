using System;
using System.Data;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.DAL
{
    public static class DatSanDAL
    {
        private const string ChiTietSql = @"SELECT b.*,s.play_date,s.court_id,t.start_time,t.end_time,c.court_name,
            a.display_name creator FROM BOOKING b JOIN COURT_SCHEDULE s ON s.schedule_id=b.schedule_id
            JOIN COURT c ON c.court_id=s.court_id JOIN TIME_SLOT t ON t.slot_id=s.slot_id
            JOIN ACCOUNT a ON a.account_id=b.created_by_account_id ";
        public static DataRow TheoId(Database db, int id)
        {
            var table = db.Query(ChiTietSql + " WHERE b.booking_id=@p0", id);
            return table.Rows.Count == 0 ? null : table.Rows[0];
        }
        public static DataRow TheoKhoa(Database db, int actor, string key)
        {
            var table = db.Query("SELECT * FROM BOOKING WHERE created_by_account_id=@p0 AND request_key=@p1", actor, key);
            return table.Rows.Count == 0 ? null : table.Rows[0];
        }
        public static DataTable DanhSach(Database db, int owner, string from, string to, int court, string status)
        {
            return db.Query(ChiTietSql + @" WHERE (@p0=0 OR b.customer_account_id=@p0)
                AND s.play_date>=@p1 AND s.play_date<=@p2 AND (@p3=0 OR s.court_id=@p3)
                AND (@p4='' OR b.status=@p4) ORDER BY s.play_date DESC,t.start_time DESC,b.booking_id DESC",
                owner, from, to, court, status);
        }
        public static int Them(Database db, NguoiDung actor, LichSan lich, string name, string phone, string source, string key, byte[] hash)
        {
            string code = "SL-" + Guid.NewGuid().ToString("N").ToUpperInvariant();
            db.Exec(@"INSERT INTO BOOKING(booking_code,schedule_id,customer_account_id,created_by_account_id,price_id,
                contact_name,contact_phone,source,status,hourly_rate_snapshot,duration_minutes,total_amount,
                request_key,request_payload_hash,created_at_utc,updated_at_utc)
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,'CONFIRMED',@p8,90,@p9,@p10,@p11,@p12,@p12)",
                code, lich.Id, source == "WEB" ? (object)actor.Id : null, actor.Id, lich.GiaId,
                name, phone, source, lich.GiaGio, lich.TongTien, key, hash, QuyTac.Utc);
            return Convert.ToInt32(db.Scalar("SELECT last_insert_rowid()"));
        }
        public static void Doi(Database db, int id, int version, LichSan lich)
        {
            QuyTac.Can(db.Exec(@"UPDATE BOOKING SET schedule_id=@p0,price_id=@p1,hourly_rate_snapshot=@p2,
                total_amount=@p3,updated_at_utc=@p4,version=version+1 WHERE booking_id=@p5 AND version=@p6 AND status='CONFIRMED'",
                lich.Id, lich.GiaId, lich.GiaGio, lich.TongTien, QuyTac.Utc, id, version) == 1, "Lượt đặt đã thay đổi. Hãy tải lại.");
        }
        public static void TrangThai(Database db, int id, int version, string status)
        {
            QuyTac.Can(db.Exec(@"UPDATE BOOKING SET status=@p0,updated_at_utc=@p1,version=version+1
                WHERE booking_id=@p2 AND version=@p3 AND status='CONFIRMED'", status, QuyTac.Utc, id, version) == 1,
                "Lượt đặt đã thay đổi. Hãy tải lại.");
        }
        public static void LichSu(Database db, int id, int actor, string action, DataRow before, DataRow after, string reason)
        {
            db.Exec(@"INSERT INTO BOOKING_HISTORY(booking_id,changed_by_account_id,action,old_schedule_id,new_schedule_id,
                old_status,new_status,old_amount,new_amount,reason,occurred_at_utc)
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)",
                id, actor, action, before == null ? null : before["schedule_id"], after["schedule_id"],
                before == null ? null : before["status"], after["status"], before == null ? null : before["total_amount"], after["total_amount"], reason, QuyTac.Utc);
            NhatKyDAL.Ghi(db, actor, action, "BOOKING", id, TomTat(before), TomTat(after));
        }
        private static object TomTat(DataRow row)
        {
            if (row == null) return null;
            return new { Lich = row["schedule_id"], TrangThai = row["status"], TongTien = row["total_amount"], PhienBan = row["version"] };
        }
        public static DataTable LichSuTheoLuot(Database db, int id)
        {
            return db.Query(@"SELECT h.*,a.display_name,c.court_name,s.play_date,t.start_time,t.end_time
                FROM BOOKING_HISTORY h JOIN ACCOUNT a ON a.account_id=h.changed_by_account_id
                JOIN COURT_SCHEDULE s ON s.schedule_id=h.new_schedule_id JOIN COURT c ON c.court_id=s.court_id
                JOIN TIME_SLOT t ON t.slot_id=s.slot_id WHERE h.booking_id=@p0 ORDER BY h.history_id DESC", id);
        }
    }
}
