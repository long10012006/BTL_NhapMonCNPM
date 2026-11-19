<%@ Page Title="Xác nhận đặt sân" Language="C#" AutoEventWireup="true" CodeBehind="XacNhan.aspx.cs" Inherits="SanLichWeb.pages.XacNhan.XacNhan" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Xác nhận lựa chọn
            </h1>
            <p class="subtitle">
                Kiểm tra sân, giờ và giá trước khi xác nhận. Không thanh toán trực tuyến.
            </p>
        </div>
    </div>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <section class="card narrow">
        <asp:Panel ID="pnlXacNhan" runat="server">
            <h2>
                <asp:Label ID="lblSan" runat="server" />
            </h2>
            <p>
                <asp:Label ID="lblNgayGio" runat="server" />
            </p>
            <p>
                Đơn giá mỗi giờ:
                <asp:Label ID="lblGiaGio" runat="server" />
            </p>
            <p class="price-total">
                Tổng tiền 90 phút:
                <strong>
                    <asp:Label ID="lblTongTien" runat="server" />
                </strong>
            </p>
            <asp:Panel ID="pnlTrucTiep" runat="server">
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="txtTen" Text="Tên liên hệ" />
                    <asp:TextBox ID="txtTen" runat="server" MaxLength="100" />
                </div>
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="txtSo" Text="Số điện thoại" />
                    <asp:TextBox ID="txtSo" runat="server" MaxLength="20" />
                </div>
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="ddlNguon" Text="Nguồn tiếp nhận" />
                    <asp:DropDownList ID="ddlNguon" runat="server" >
                        <asp:ListItem Value="PHONE">
                            Điện thoại
                        </asp:ListItem>
                        <asp:ListItem Value="COUNTER">
                            Tại quầy
                        </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </asp:Panel>
            <asp:Button ID="btnXacNhan" runat="server" Text="Xác nhận đặt sân" CssClass="btn" OnClick="btnXacNhan_Click"  />
        </asp:Panel>
        <asp:HyperLink ID="lnkKetQua" runat="server" Visible="false" CssClass="btn" Text="Xem lượt đặt" />
        <asp:HyperLink runat="server" NavigateUrl="~/pages/TrangChu/TrangChu.aspx" Text="Quay lại chọn sân" />
    </section>
</asp:Content>
