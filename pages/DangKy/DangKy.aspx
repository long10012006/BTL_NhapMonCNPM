<%@ Page Title="Đăng ký" Language="C#" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="SanLichWeb.pages.DangKy.DangKy" %>
<!DOCTYPE html>
<html lang="vi">
    <head runat="server">
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <title>
            Đăng ký | Sân Lịch
        </title>
        <link href="../style_Chung.css" rel="stylesheet" />
    </head>
    <body>
        <form id="form1" runat="server">
            <section class="login-screen">
                <div class="login-shell">
                    <div class="login-story">
                        <div class="brand">
                            <div class="brand-mark">
                                SL
                            </div>
                            <div class="brand-name">
                                SÂN LỊCH
                                <span class="brand-sub">
                                    ĐẶT LỊCH SÂN THỂ THAO
                                </span>
                            </div>
                        </div>
                        <p class="login-eyebrow">
                            SÂN BÓNG ĐÁ MINI
                        </p>
                        <h1>
                            Chọn sân phù hợp.
                            <br />
                            Giữ lịch dễ dàng.
                        </h1>
                        <p>
                            Xem lịch trống và giá trước khi đặt. Theo dõi, hủy hoặc đổi lịch trong tài khoản của bạn.
                        </p>
                        <div class="court-art" aria-hidden="true">
                            <i class="court-center">
                            </i>
                        </div>
                    </div>
                    <div class="login-form-wrap">
                        <h2>
                            Đăng ký khách hàng
                        </h2>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtHoTen" Text="Họ và tên" />
                            <asp:TextBox ID="txtHoTen" runat="server" MaxLength="100" />
                        </div>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtDienThoai" Text="Số điện thoại" />
                            <asp:TextBox ID="txtDienThoai" runat="server" MaxLength="20" inputmode="tel" />
                        </div>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtMatKhau" Text="Mật khẩu (ít nhất 8 ký tự)" />
                            <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" MaxLength="128" />
                        </div>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtNhapLai" Text="Nhập lại mật khẩu" />
                            <asp:TextBox ID="txtNhapLai" runat="server" TextMode="Password" MaxLength="128" />
                        </div>
                        <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
                        <asp:Button ID="btnDangKy" runat="server" Text="Tạo tài khoản" CssClass="btn" OnClick="btnDangKy_Click"  />
                        <p>
                            <a href="../DangNhap/DangNhap.aspx">
                                Quay lại đăng nhập
                            </a>
                        </p>
                    </div>
                </div>
            </section>
        </form>
    </body>
</html>
