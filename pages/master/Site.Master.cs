using System;
using System.Web;
using System.Web.Security;
using SanLichWeb.Models;
using SanLichWeb.BLL;
namespace SanLichWeb.pages.master
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblBieuMauCu.Visible = !Page.IsPostBack && Request.QueryString["formExpired"] == "1";
            lblBieuMauCu.Text = SanLichWeb.Helpers.BaoVeBieuMau.ThongBao;
            var saved = Session["NguoiDung"] as NguoiDung;
            var user = saved == null ? null : new TaiKhoanBLL().HienTai(saved.Id);
            phQuanLy.Visible = user != null && user.LaQuanLy;
            lnkLich.Visible = user != null && !user.LaQuanLy;
            btnDangXuat.Visible = user != null;
            lnkDangNhap.Visible = user == null;
            lblNguoiDung.Text = user == null ? "Khách xem lịch" : HttpUtility.HtmlEncode(user.HoTen);
        }
        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            SanLichWeb.Helpers.BaoVeBieuMau.XoaCookieSession(Context);
            Response.Redirect("~/pages/DangNhap/DangNhap.aspx");
        }
    }
}
