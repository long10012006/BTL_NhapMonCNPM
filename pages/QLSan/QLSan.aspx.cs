using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Helpers;
using SanLichWeb.Models;
namespace SanLichWeb.pages.QLSan
{
    public partial class QLSan : TrangQuanLy
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Thu(lblThongBao, HienThi);
        }
        private void HienThi()
        {
            gvSan.DataSource = new SanBLL().DanhSach(true, ActorId);
            gvSan.DataBind();
        }
        protected void gvSan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SuaSan") return;
            Thu(lblThongBao, () =>
            {
                int id = So(e.CommandArgument.ToString());
                DataRow row = new QuanLyBLL().San(ActorId, id);
                QuyTac.Can(row != null, "Sân không tồn tại.");
                ViewState["Id"] = id;
                ViewState["Version"] = Convert.ToInt32(row["version"]);
                txtMa.Text = row["court_code"].ToString();
                txtTen.Text = row["court_name"].ToString();
                txtMoTa.Text = row["description"].ToString();
                chkAn.Checked = Convert.ToInt32(row["is_hidden"]) == 1;
            });
        }
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            Thu(lblThongBao, () =>
            {
                new QuanLyBLL().LuuSan(ActorId, (int)(ViewState["Id"] ?? 0), (int)(ViewState["Version"] ?? 0),
                    txtMa.Text, txtTen.Text, txtMoTa.Text, chkAn.Checked);
                Response.Redirect("~/pages/QLSan/QLSan.aspx?saved=1");
            });
        }
        protected void btnMoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/QLSan/QLSan.aspx");
        }

    }
}
