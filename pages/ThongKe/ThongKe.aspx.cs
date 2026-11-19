using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.ThongKe
{
    public partial class ThongKe : TrangQuanLy
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTuNgay.Text = QuyTac.Ngay(new DateTime(QuyTac.BayGio.Year, QuyTac.BayGio.Month, 1));
                txtDenNgay.Text = QuyTac.Ngay(QuyTac.BayGio.Date.AddDays(14));
                NapSan(ddlSan, true, true);
                Thu(lblThongBao, HienThi);
            }
        }
        protected void btnLoc_Click(object sender, EventArgs e) { Thu(lblThongBao, HienThi); }
        private void HienThi()
        {
            var result = new ThongKeBLL().Tinh(ActorId, txtTuNgay.Text, txtDenNgay.Text, So(ddlSan.SelectedValue), ddlTrangThai.SelectedValue);
            lblSoLuot.Text = result.SoLuot.ToString();
            lblGiaTri.Text = QuyTac.Tien(result.GiaTriDuKien);
            gvThongKe.DataSource = result.TheoTrangThai;
            gvThongKe.DataBind();
        }

    }
}
