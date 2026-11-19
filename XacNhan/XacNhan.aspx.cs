using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.XacNhan
{
    public partial class XacNhan : TrangKhach
    {

        private int LichId { get { return (int)(ViewState["LichId"] ?? 0); } set { ViewState["LichId"] = value; } }
        private int DoiId { get { return (int)(ViewState["DoiId"] ?? 0); } set { ViewState["DoiId"] = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlXacNhan.Visible = false;
                Thu(lblThongBao, () =>
                {
                    LichId = So(Request.QueryString["lich"]);
                    DoiId = string.IsNullOrEmpty(Request.QueryString["doi"]) ? 0 : So(Request.QueryString["doi"]);
                    var lich = new SanBLL().ChiTiet(LichId);
                    QuyTac.KiemTraLich(lich);
                    if (DoiId > 0)
                    {
                        var old = new DatSanBLL().ChiTiet(ActorId, DoiId, false);
                        ViewState["Version"] = Convert.ToInt32(old["version"]);
                        btnXacNhan.Text = "Xác nhận đổi sang lịch và giá mới";
                    }
                    ViewState["TongTien"] = lich.TongTien;
                    ViewState["RequestKey"] = Guid.NewGuid().ToString();
                    lblSan.Text = HttpUtility.HtmlEncode(lich.TenSan);
                    lblNgayGio.Text = lich.BatDau.ToString("dd/MM/yyyy · HH:mm") + " – " + lich.KetThuc.ToString("HH:mm");
                    lblGiaGio.Text = QuyTac.Tien(lich.GiaGio);
                    lblTongTien.Text = QuyTac.Tien(lich.TongTien);
                    pnlTrucTiep.Visible = NguoiDung.LaQuanLy && DoiId == 0;
                    pnlXacNhan.Visible = true;
                });
            }
        }
        protected void btnXacNhan_Click(object sender, EventArgs e)
        {
            Thu(lblThongBao, () =>
            {
                QuyTac.Can(LichId > 0 && ViewState["TongTien"] != null, "Hãy chọn lại lịch.");
                var bll = new DatSanBLL();
                int id = DoiId;
                if (id > 0)
                    bll.Doi(ActorId, id, Convert.ToInt32(ViewState["Version"]), LichId, Convert.ToInt64(ViewState["TongTien"]));
                else
                    id = bll.Tao(ActorId, LichId, Convert.ToInt64(ViewState["TongTien"]), Convert.ToString(ViewState["RequestKey"]),
                        NguoiDung.LaQuanLy ? ddlNguon.SelectedValue : "WEB", txtTen.Text, txtSo.Text);
                // Redirect sau POST; refresh trang kết quả không tạo lượt mới.
                string target = NguoiDung.LaQuanLy ? "QLDatSan" : "LichCuaToi";
                Response.Redirect("~/pages/" + target + "/" + target + ".aspx?id=" + id);
            });
        }

    }
}
