using System;
using System.Web;
using System.Web.Security;
using SanLichWeb.BLL;
namespace SanLichWeb
{
    public class DangNhapThanhCong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id;
            if (!Request.IsAuthenticated || !int.TryParse(User.Identity.Name, out id))
            {
                Response.Redirect("~/pages/DangNhap/DangNhap.aspx");
                return;
            }
            var user = new TaiKhoanBLL().HienTai(id);
            if (user == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/pages/DangNhap/DangNhap.aspx");
                return;
            }
            Session["NguoiDung"] = user;
            Response.Redirect(user.LaQuanLy ? "~/pages/QLDatSan/QLDatSan.aspx" : "~/pages/TrangChu/TrangChu.aspx");
        }
    }
}
