using System;
using System.Data;
using SanLichWeb.Models;
using SanLichWeb.Helpers;
namespace SanLichWeb.DAL
{
    public static class TaiKhoanDAL
    {
        public static DataRow TheoSo(Database db, string phone)
        {
            var table = db.Query("SELECT * FROM ACCOUNT WHERE phone_normalized=@p0", phone);
            return table.Rows.Count == 0 ? null : table.Rows[0];
        }
        public static NguoiDung TheoId(Database db, int id)
        {
            var table = db.Query("SELECT * FROM ACCOUNT WHERE account_id=@p0 AND is_active=1", id);
            return table.Rows.Count == 0 ? null : Doc(table.Rows[0]);
        }
        public static NguoiDung Doc(DataRow row)
        {
            return new NguoiDung
            {
                Id = Convert.ToInt32(row["account_id"]),
                HoTen = Convert.ToString(row["display_name"]),
                SoDienThoai = Convert.ToString(row["phone_normalized"]),
                VaiTro = Convert.ToString(row["role"])
            };
        }
        public static int Them(Database db, string name, string phone, string hash, string role)
        {
            db.Exec(@"INSERT INTO ACCOUNT(phone_normalized,display_name,password_hash,role,created_at_utc)
                VALUES(@p0,@p1,@p2,@p3,@p4)", phone, name, hash, role, QuyTac.Utc);
            return Convert.ToInt32(db.Scalar("SELECT last_insert_rowid()"));
        }
    }
}
