using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.DangNhap
{
    public partial class DangNhap : TrangCoSo
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["formExpired"] == "1")
                lblThongBao.Text = BaoVeBieuMau.ThongBao;
        }
        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            Thu(lblThongBao, () =>
            {
                var user = new TaiKhoanBLL().DangNhap(txtDienThoai.Text, txtMatKhau.Text);
                // Tách khỏi session trước đăng nhập; không giữ lại ID phiên cũ.
                Session.Clear();
                Session.Abandon();
                SanLichWeb.Helpers.BaoVeBieuMau.XoaCookieSession(Context);
                System.Web.Security.FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                Response.Redirect("~/DangNhapThanhCong.aspx");
            });
        }

    }
}
