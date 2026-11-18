using System;
using System.Data;
using System.Data.SQLite;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SanLichWeb.BLL;
using SanLichWeb.Models;
namespace SanLichWeb.Helpers
{
    public class TrangCoSo : Page
    {
        protected NguoiDung NguoiDung { get; private set; }
        protected virtual bool CanDangNhap { get { return false; } }
        protected virtual bool CanQuanLy { get { return false; } }
        protected override void OnInit(EventArgs e)
        {
            ViewStateUserKey = BaoVeBieuMau.LayKhoa(Context);
            // Session có thể mất khi IIS Express khởi động lại. Cookie đăng nhập
            // đã được ASP.NET xác thực mới là nguồn xác định tài khoản.
            int signedId;
            if (Request.IsAuthenticated && int.TryParse(User.Identity.Name, out signedId))
            {
                NguoiDung = new TaiKhoanBLL().HienTai(signedId);
                if (NguoiDung != null) Session["NguoiDung"] = NguoiDung;
            }
            else
            {
                Session.Remove("NguoiDung");
            }
            if ((CanDangNhap || CanQuanLy) && NguoiDung == null)
            {
                Response.Redirect("~/pages/DangNhap/DangNhap.aspx", true);
                return;
            }
            if (CanQuanLy && !NguoiDung.LaQuanLy) throw new HttpException(403, "Chỉ dành cho quản lý.");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            base.OnInit(e);
        }
        protected override void OnPreLoad(EventArgs e)
        {
            const string tenKhoa = "SanLich.FormBinding.v2";
            if (!IsPostBack)
            {
                ViewState[tenKhoa] = ViewStateUserKey;
            }
            else if (!string.Equals(ViewState[tenKhoa] as string, ViewStateUserKey, StringComparison.Ordinal))
            {
                // Kiểm tra trước Page_Load và trước mọi sự kiện ghi dữ liệu.
                Response.Redirect(BaoVeBieuMau.DuongDanTaiLai(Context), true);
                return;
            }
            base.OnPreLoad(e);
        }

        protected int ActorId { get { return NguoiDung == null ? 0 : NguoiDung.Id; } }
        protected void Thu(Label label, Action action)
        {
            try
            {
                label.Text = "";
                action();
            }
            catch (LoiNghiepVu ex) { label.Text = HttpUtility.HtmlEncode(ex.Message); }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                label.Text = "Không lưu được dữ liệu: bản ghi bị trùng hoặc dữ liệu vừa thay đổi. Hãy tải lại và thử lại.";
            }
        }
        protected int So(string value)
        {
            int number;
            QuyTac.Can(int.TryParse(value, out number) && number >= 0, "Mã lựa chọn không hợp lệ.");
            return number;
        }
        protected DateTime DocNgay(string value)
        {
            DateTime date;
            QuyTac.Can(DateTime.TryParseExact(value, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out date), "Ngày không hợp lệ.");
            return date;
        }
        protected string MaHoaHtml(object value) { return HttpUtility.HtmlEncode(Convert.ToString(value)); }
        protected string Tien(object value) { return QuyTac.Tien(value); }
        protected string TrangThai(object value) { return QuyTac.TrangThai(Convert.ToString(value)); }
        protected void NapSan(DropDownList ddl, bool all, bool hidden)
        {
            ddl.DataSource = new SanBLL().DanhSach(hidden, ActorId);
            ddl.DataTextField = "court_name";
            ddl.DataValueField = "court_id";
            ddl.DataBind();
            if (all) ddl.Items.Insert(0, new ListItem("Tất cả sân", "0"));
        }
        protected void NapGio(DropDownList ddl)
        {
            ddl.DataSource = new SanBLL().KhungGio();
            ddl.DataTextField = "label";
            ddl.DataValueField = "slot_id";
            ddl.DataBind();
        }
    }
    public class TrangKhach : TrangCoSo
    {
        protected override bool CanDangNhap { get { return true; } }
    }
    public class TrangQuanLy : TrangCoSo
    {
        protected override bool CanQuanLy { get { return true; } }
    }
}
