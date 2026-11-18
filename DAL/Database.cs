using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web.Hosting;
namespace SanLichWeb.DAL
{
    // Một đối tượng = một kết nối. Toàn bộ giao dịch sử dụng cùng đối tượng này.
    public sealed class Database : IDisposable
    {
        private readonly SQLiteConnection connection;
        private bool inTransaction;
        public static string DataDirectoryOverride;
        public static string ThuMuc
        {
            get { return DataDirectoryOverride ?? HostingEnvironment.MapPath("~/App_Data"); }
        }
        public Database()
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = Path.Combine(ThuMuc, "DatLichSan.db"),
                Version = 3,
                DefaultTimeout = 5,
                Pooling = true
            };
            connection = new SQLiteConnection(builder.ConnectionString);
            connection.Open();
            Exec("PRAGMA foreign_keys=ON; PRAGMA busy_timeout=5000;");
        }
        public void BatDau()
        {
            Exec("BEGIN IMMEDIATE;");
            inTransaction = true;
        }
        public void Luu()
        {
            Exec("COMMIT;");
            inTransaction = false;
        }
        private SQLiteCommand Lenh(string sql, object[] values)
        {
            var command = new SQLiteCommand(sql, connection);
            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];
                DbType type = value is byte[] ? DbType.Binary :
                    value is int ? DbType.Int32 : value is long ? DbType.Int64 : DbType.String;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@p" + i;
                parameter.DbType = type;
                parameter.Value = value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            return command;
        }
        public int Exec(string sql, params object[] values)
        {
            using (var command = Lenh(sql, values)) return command.ExecuteNonQuery();
        }
        public object Scalar(string sql, params object[] values)
        {
            using (var command = Lenh(sql, values)) return command.ExecuteScalar();
        }
        public DataTable Query(string sql, params object[] values)
        {
            using (var command = Lenh(sql, values))
            using (var reader = command.ExecuteReader())
            {
                var table = new DataTable();
                // Kết quả JOIN chỉ dùng để hiển thị; không nhập ràng buộc UNIQUE
                // của bảng nguồn vào DataTable (nhiều lượt có thể cùng sân/tài khoản).
                for (int i = 0; i < reader.FieldCount; i++)
                    table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                while (reader.Read())
                {
                    object[] valuesInRow = new object[reader.FieldCount];
                    reader.GetValues(valuesInRow);
                    table.Rows.Add(valuesInRow);
                }
                return table;
            }
        }
        public void Dispose()
        {
            if (inTransaction)
            {
                try { Exec("ROLLBACK;"); }
                catch (SQLiteException) { }
            }
            connection.Dispose();
        }
    }
}
