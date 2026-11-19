<%@ Page Title="Đặt sân" Language="C#" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="SanLichWeb.pages.TrangChu.TrangChu" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Chọn sân và khung giờ
            </h1>
            <p class="subtitle">
                Xem giá từng khung 90 phút. Đặt từ hôm nay đến 14 ngày tới; xác nhận tự động khi hợp lệ.
            </p>
        </div>
    </div>
    <asp:Panel ID="pnlLoc" runat="server" CssClass="filters" DefaultButton="btnXem">
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtNgay" Text="Ngày đặt" />
            <asp:TextBox ID="txtNgay" runat="server" TextMode="Date"  />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlSan" Text="Sân" />
            <asp:DropDownList ID="ddlSan" runat="server" >
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnXem" runat="server" Text="Xem lịch" CssClass="btn" OnClick="btnXem_Click"  />
    </asp:Panel>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <asp:Label ID="lblDoiLich" runat="server" CssClass="notice" />
    <div class="court-list">
        <asp:Repeater ID="rpSan" runat="server">
            <ItemTemplate>
                <article class="court-card">
                    <div class="pitch" aria-label="Minh họa sân bóng">
                        <i class="pitch-line">
                        </i>
                        <i class="goal left">
                        </i>
                        <i class="goal right">
                        </i>
                        <span class="pitch-label">
                            SÂN MINI
                        </span>
                    </div>
                    <div class="court-main">
                        <h2 class="court-title">
                            <%# MaHoaHtml(Eval("Ten")) %>
                        </h2>
                        <p class="court-meta">
                            <%# MaHoaHtml(Eval("MoTa")) %>
                        </p>
                        <div class="slots">
                            <asp:Repeater runat="server" DataSource='<%# Eval("Gio") %>'>
                                <ItemTemplate>
                                    <asp:HyperLink
                                        runat="server"
                                        CssClass="slot"
                                        Enabled='<%# Eval("ConTrong") %>'
                                        ToolTip='<%# Eval("LyDo") %>'
                                        NavigateUrl='<%# LinkXacNhan(Eval("Id")) %>' >
                                        <%# MaHoaHtml(Eval("Nhan")) %>
                                        <br />
                                        <small>
                                            <%# MaHoaHtml(Eval("Gia")) %>
                                        </small>
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <p class="notice">
        Ô xám: đã đặt, đang khóa, hết giờ hoặc chưa có giá. Giá trên mỗi ô là tổng tiền 90 phút.
    </p>
</asp:Content>
