using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.DangKy
{
    public partial class DangKy : TrangCoSo
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["formExpired"] == "1")
                lblThongBao.Text = BaoVeBieuMau.ThongBao;
        }
        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Thu(lblThongBao, () =>
            {
                new TaiKhoanBLL().DangKy(txtHoTen.Text, txtDienThoai.Text, txtMatKhau.Text, txtNhapLai.Text);
                Response.Redirect("~/pages/DangNhap/DangNhap.aspx");
            });
        }

    }
}
