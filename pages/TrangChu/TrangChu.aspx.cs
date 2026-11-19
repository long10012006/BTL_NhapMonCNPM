using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.TrangChu
{
    public partial class TrangChu : TrangCoSo
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNgay.Text = QuyTac.Ngay(QuyTac.BayGio.Date);
                txtNgay.Attributes["min"] = txtNgay.Text;
                txtNgay.Attributes["max"] = QuyTac.Ngay(QuyTac.BayGio.Date.AddDays(14));
                NapSan(ddlSan, true, false);
                if (!string.IsNullOrEmpty(Request.QueryString["doi"]))
                    lblDoiLich.Text = "Đang đổi lịch: chọn giờ mới, sau đó xem và xác nhận lại giá. Lịch cũ được giữ đến khi đổi thành công.";
                Thu(lblThongBao, HienThi);
            }
        }
        protected string LinkXacNhan(object id)
        {
            string link = "~/pages/XacNhan/XacNhan.aspx?lich=" + Convert.ToInt32(id);
            int doi;
            if (int.TryParse(Request.QueryString["doi"], out doi) && doi > 0) link += "&doi=" + doi;
            return link;
        }
        protected void btnXem_Click(object sender, EventArgs e) { Thu(lblThongBao, HienThi); }
        private void HienThi()
        {
            DataTable table = new SanBLL().LichNgay(DocNgay(txtNgay.Text), So(ddlSan.SelectedValue), false, ActorId);
            var list = new List<SanView>();
            foreach (var group in table.Rows.Cast<DataRow>().GroupBy(r => Convert.ToInt32(r["court_id"])))
            {
                DataRow first = group.First();
                var san = new SanView { Ten = first["court_name"].ToString(), MoTa = first["description"].ToString(), Gio = new List<GioView>() };
                foreach (DataRow row in group)
                {
                    var lich = LichSan.TuDong(row);
                    bool available = lich.DangMo && !lich.DaDat && lich.GiaId > 0 && lich.BatDau > QuyTac.BayGio;
                    int doi;
                    if (int.TryParse(Request.QueryString["doi"], out doi) && doi > 0)
                        available = available && lich.BatDau >= QuyTac.BayGio.AddHours(2);
                    san.Gio.Add(new GioView
                    {
                        Id = lich.Id,
                        Nhan = lich.BatDau.ToString("HH:mm") + " – " + lich.KetThuc.ToString("HH:mm"),
                        Gia = lich.GiaId == 0 ? "Chưa có giá" : QuyTac.Tien(lich.TongTien),
                        ConTrong = available,
                        LyDo = available ? "Còn trống – chọn để xác nhận" : "Không thể đặt: đã chiếm, khóa, hết giờ hoặc chưa có giá"
                    });
                }
                list.Add(san);
            }
            rpSan.DataSource = list;
            rpSan.DataBind();
            if (list.Count == 0) lblThongBao.Text = "Không có sân đang hiển thị.";
        }

    }
}
