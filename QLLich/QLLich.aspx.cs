using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.QLLich
{
    public partial class QLLich : TrangQuanLy
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNgay.Text = QuyTac.Ngay(QuyTac.BayGio.Date);
                NapSan(ddlSan, false, true);
                Thu(lblThongBao, HienThi);
            }
        }
        private void HienThi()
        {
            gvLich.DataSource = new SanBLL().LichNgay(DocNgay(txtNgay.Text), So(ddlSan.SelectedValue), true, ActorId);
            gvLich.DataBind();
        }
        protected void btnXem_Click(object sender, EventArgs e) { Thu(lblThongBao, HienThi); }
        protected void gvLich_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Mo" && e.CommandName != "Khoa") return;
            Thu(lblThongBao, () =>
            {
                new QuanLyBLL().KhoaMo(ActorId, So(e.CommandArgument.ToString()), e.CommandName == "Mo", txtLyDo.Text);
                HienThi();
                lblThongBao.Text = "Đã cập nhật lịch.";
            });
        }

    }
}
