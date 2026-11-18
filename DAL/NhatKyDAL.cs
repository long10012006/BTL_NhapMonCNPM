using System;
using System.Web.Script.Serialization;
using SanLichWeb.Helpers;
namespace SanLichWeb.DAL
{
    public static class NhatKyDAL
    {
        public static System.Data.DataTable DanhSach(Database db)
        {
            return db.Query(@"SELECT l.*,a.display_name FROM AUDIT_LOG l LEFT JOIN ACCOUNT a ON a.account_id=l.actor_account_id
                ORDER BY l.audit_id DESC LIMIT 300");
        }
        public static void Ghi(Database db, int? actor, string action, string target, object id, object before, object after)
        {
            var json = new JavaScriptSerializer();
            db.Exec(@"INSERT INTO AUDIT_LOG(actor_account_id,action,target_type,target_id,old_values_json,new_values_json,correlation_id,occurred_at_utc)
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)",
                actor.HasValue ? (object)actor.Value : null, action, target, Convert.ToString(id),
                before == null ? null : json.Serialize(before), after == null ? null : json.Serialize(after), Guid.NewGuid().ToString(), QuyTac.Utc);
        }
    }
}
