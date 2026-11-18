using System;
using System.Web;
using SanLichWeb.DAL;
using SanLichWeb.Helpers;
namespace SanLichWeb
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            KhoiTaoDuLieu.Chay();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            // Form cũ hoặc cookie tài khoản đã thay đổi: từ chối POST rồi GET lại.
            // Không bỏ kiểm tra MAC, không chạy lại thao tác ghi dữ liệu tự động.
            if (Request.HttpMethod == "POST" && BaoVeBieuMau.LaLoiViewState(error))
            {
                System.Diagnostics.Trace.TraceWarning("A stale Web Forms POST was rejected: " + Request.Path);
                Server.ClearError();
                Response.Clear();
                Response.Redirect(BaoVeBieuMau.DuongDanTaiLai(Context), false);
                CompleteRequest();
                return;
            }
            System.Diagnostics.Trace.TraceError(error.ToString());
        }
    }
}
