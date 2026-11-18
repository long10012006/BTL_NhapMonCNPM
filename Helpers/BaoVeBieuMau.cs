using System;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace SanLichWeb.Helpers
{
    // Khóa biểu mẫu độc lập với Session, dùng chung cho toàn bộ Web Forms.
    public static class BaoVeBieuMau
    {
        public const string TenCookie = "SanLich.FormKey.v2";
        public const string ThongBao = "Biểu mẫu cũ đã hết hiệu lực. Trang đã được tải lại; hãy kiểm tra thông tin và thực hiện lại thao tác.";

        public static string LayKhoa(HttpContext context)
        {
            HttpCookie cookie = context.Request.Cookies[TenCookie];
            Guid key;
            bool valid = cookie != null && Guid.TryParseExact(cookie.Value, "N", out key);
            if (!valid)
            {
                key = Guid.NewGuid();
                var newCookie = new HttpCookie(TenCookie, key.ToString("N"))
                {
                    HttpOnly = true,
                    Secure = context.Request.IsSecureConnection,
                    Path = DuongDanCookie(context)
                };
                context.Response.Cookies.Set(newCookie);
                // Không nhận POST khi trình duyệt không gửi lại cookie của form.
                if (context.Request.HttpMethod == "POST")
                {
                    context.Response.Redirect(DuongDanTaiLai(context), true);
                }
            }
            else
            {
                key = Guid.ParseExact(cookie.Value, "N");
            }

            string identity = context.Request.IsAuthenticated
                ? context.User.Identity.Name
                : "Khach";
            return key.ToString("N") + "|" + identity;
        }

        public static string DuongDanCookie(HttpContext context)
        {
            string path = context.Request.ApplicationPath;
            return string.IsNullOrEmpty(path) ? "/" : path;
        }

        public static void XoaCookieSession(HttpContext context)
        {
            var config = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            context.Response.Cookies.Set(new HttpCookie(config.CookieName, "")
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = context.Request.IsSecureConnection,
                Path = DuongDanCookie(context)
            });
        }

        public static bool LaLoiViewState(Exception error)
        {
            for (Exception current = error; current != null; current = current.InnerException)
            {
                if (current is ViewStateException) return true;
                // Một số runtime bọc lỗi đọc ViewState bằng HttpException trực tiếp.
                string stack = current.StackTrace ?? "";
                if (current is HttpException &&
                    stack.IndexOf("System.Web.UI.ObjectStateFormatter", StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        public static string DuongDanTaiLai(HttpContext context)
        {
            var query = HttpUtility.ParseQueryString(context.Request.Url.Query);
            query["formExpired"] = "1";
            return context.Request.Path + "?" + query;
        }
    }
}
