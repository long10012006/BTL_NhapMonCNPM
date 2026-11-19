using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.QLGia
{
    public partial class QLGia : TrangQuanLy
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NapSan(ddlSan, false, true);
                NapGio(ddlGio);
                txtTuNgay.Text = QuyTac.Ngay(QuyTac.BayGio.Date.AddDays(1));
                Thu(lblThongBao, HienThi);
            }
        }
        private void HienThi()
        {
            gvGia.DataSource = new QuanLyBLL().BangGia(ActorId);
            gvGia.DataBind();
        }
        protected void gvGia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGia.PageIndex = e.NewPageIndex;
            Thu(lblThongBao, HienThi);
        }
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            Thu(lblThongBao, () =>
            {
                long rate;
                QuyTac.Can(long.TryParse(txtGia.Text, out rate), "Nhập đơn giá nguyên đồng hợp lệ.");
                new QuanLyBLL().GiaMoi(ActorId, So(ddlSan.SelectedValue), So(ddlGio.SelectedValue), DocNgay(txtTuNgay.Text), rate);
                HienThi();
                lblThongBao.Text = "Đã áp dụng giá mới. Lượt đã xác nhận giữ nguyên giá cũ.";
            });
        }

    }
}
