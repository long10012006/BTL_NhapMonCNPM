using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.QLDatSan
{
    public partial class QLDatSan : TrangQuanLy
    {

        private const bool QuanLy = true;
        private int IdDangChon
        {
            get { return (int)(ViewState["IdDangChon"] ?? 0); }
            set { ViewState["IdDangChon"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTuNgay.Text = "2000-01-01";
                txtDenNgay.Text = "2099-12-31";
                NapSan(ddlSan, true, QuanLy);
                Thu(lblThongBao, () =>
                {
                    HienThi();
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id)) Chon(id);
                });
            }
        }
        protected void btnLoc_Click(object sender, EventArgs e)
        {
            gvDanhSach.PageIndex = 0;
            Thu(lblThongBao, HienThi);
        }
        private void HienThi()
        {
            var list = new DatSanBLL().DanhSach(ActorId, QuanLy, txtTuNgay.Text, txtDenNgay.Text, So(ddlSan.SelectedValue), ddlTrangThai.SelectedValue);
            gvDanhSach.DataSource = list;
            gvDanhSach.DataBind();
            lblSoLuong.Text = "Tìm thấy " + list.Rows.Count + " lượt đặt.";
        }
        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDanhSach.PageIndex = e.NewPageIndex;
            Thu(lblThongBao, HienThi);
        }
        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Chon") Thu(lblThongBao, () => Chon(So(Convert.ToString(e.CommandArgument))));
        }
        private void Chon(int id)
        {
            var bll = new DatSanBLL();
            DataRow row = bll.ChiTiet(ActorId, id, QuanLy);
            IdDangChon = id;
            ViewState["Version"] = Convert.ToInt32(row["version"]);
            lblChiTiet.Text = HttpUtility.HtmlEncode(row["booking_code"] + " · " + row["contact_name"] + " · " + row["contact_phone"]
                + " · " + row["court_name"] + " · " + row["play_date"] + " · " + row["start_time"] + "–" + row["end_time"]
                + " · " + QuyTac.Tien(row["total_amount"]) + " · " + QuyTac.TrangThai(row["status"].ToString()) + " · Nguồn: " + row["source"]);
            bool confirmed = row["status"].ToString() == "CONFIRMED";
            btnHuy.Visible = confirmed;
            btnHoanThanh.Visible = confirmed;
            btnKhongDen.Visible = confirmed;
            gvLichSu.DataSource = bll.LichSu(ActorId, id, QuanLy);
            gvLichSu.DataBind();
            pnlChiTiet.Visible = true;
        }
        private void XuLy(string status)
        {
            Thu(lblThongBao, () =>
            {
                QuyTac.Can(IdDangChon > 0, "Hãy chọn một lượt đặt.");
                new DatSanBLL().ChuyenTrangThai(ActorId, IdDangChon, Convert.ToInt32(ViewState["Version"]), status, txtLyDo.Text, QuanLy);
                Chon(IdDangChon);
                HienThi();
                lblThongBao.Text = "Đã cập nhật lượt đặt.";
            });
        }
        protected void btnHuy_Click(object sender, EventArgs e) { XuLy("CANCELLED"); }
        protected void btnHoanThanh_Click(object sender, EventArgs e) { XuLy("COMPLETED"); }
        protected void btnKhongDen_Click(object sender, EventArgs e) { XuLy("NO_SHOW"); }

    }
}
