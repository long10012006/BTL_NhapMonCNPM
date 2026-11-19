<%@ Page Title="Đăng nhập" Language="C#" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="SanLichWeb.pages.DangNhap.DangNhap" %>
<!DOCTYPE html>
<html lang="vi">
    <head runat="server">
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <title>
            Đăng nhập | Sân Lịch
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
                            Đăng nhập
                        </h2>
                        <p>
                            Dùng số điện thoại của khách hàng hoặc quản lý.
                        </p>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtDienThoai" Text="Số điện thoại" />
                            <asp:TextBox ID="txtDienThoai" runat="server" TextMode="SingleLine" MaxLength="20" inputmode="tel" autocomplete="username" />
                        </div>
                        <div class="field">
                            <asp:Label runat="server" AssociatedControlID="txtMatKhau" Text="Mật khẩu" />
                            <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" MaxLength="128" autocomplete="current-password" />
                        </div>
                        <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
                        <asp:Button ID="btnDangNhap" runat="server" Text="Đăng nhập" CssClass="btn" OnClick="btnDangNhap_Click"  />
                        <p>
                            <a href="../DangKy/DangKy.aspx">
                                Tạo tài khoản khách hàng
                            </a>
                            ·
                            <a href="../TrangChu/TrangChu.aspx">
                                Xem sân trước
                            </a>
                        </p>
                        <div class="demo-hint">
                            Tài khoản bài mẫu:
                            <br />
                            Khách:
                            <strong>
                                0900000002 / Khach@123
                            </strong>
                            <br />
                            Quản lý:
                            <strong>
                                0900000001 / QuanLy@123
                            </strong>
                        </div>
                    </div>
                </div>
            </section>
        </form>
    </body>
</html>
