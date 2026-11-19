<%@ Page Title="Thống kê" Language="C#" AutoEventWireup="true" CodeBehind="ThongKe.aspx.cs" Inherits="SanLichWeb.pages.ThongKe.ThongKe" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Thống kê lượt đặt
            </h1>
            <p class="subtitle">
                Lọc theo ngày sử dụng, sân và trạng thái. Giá trị dự kiến là tổng giá chốt của lượt Đã xác nhận và Hoàn thành.
            </p>
        </div>
    </div>
    <div class="filters">
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtTuNgay" Text="Từ ngày" />
            <asp:TextBox ID="txtTuNgay" runat="server" TextMode="Date"  />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtDenNgay" Text="Đến ngày" />
            <asp:TextBox ID="txtDenNgay" runat="server" TextMode="Date"  />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlSan" Text="Sân" />
            <asp:DropDownList ID="ddlSan" runat="server" >
            </asp:DropDownList>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlTrangThai" Text="Trạng thái" />
            <asp:DropDownList ID="ddlTrangThai" runat="server" >
                <asp:ListItem Value="">
                    Tất cả trạng thái
                </asp:ListItem>
                <asp:ListItem Value="CONFIRMED">
                    Đã xác nhận
                </asp:ListItem>
                <asp:ListItem Value="CANCELLED">
                    Đã hủy
                </asp:ListItem>
                <asp:ListItem Value="COMPLETED">
                    Hoàn thành
                </asp:ListItem>
                <asp:ListItem Value="NO_SHOW">
                    Không đến
                </asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnLoc" runat="server" Text="Lọc dữ liệu" CssClass="btn" OnClick="btnLoc_Click"  />
    </div>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <div class="stats">
        <div class="card">
            <span>
                Số lượt trong bộ lọc
            </span>
            <strong>
                <asp:Label ID="lblSoLuot" runat="server" />
            </strong>
        </div>
        <div class="card">
            <span>
                Giá trị đặt sân dự kiến
            </span>
            <strong>
                <asp:Label ID="lblGiaTri" runat="server" />
            </strong>
        </div>
    </div>
    <p class="notice">
        Đơn vị VNĐ. Loại Đã hủy và Không đến khỏi tổng giá trị. Đây là giá trị dự kiến, không phải tiền thực thu.
    </p>
    <div class="table-wrap">
        <asp:GridView ID="gvThongKe" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="Không có dữ liệu." GridLines="None" >
            <Columns>
                <asp:BoundField DataField="status_name" HeaderText="Trạng thái" />
                <asp:BoundField DataField="count" HeaderText="Số lượt" />
                <asp:BoundField DataField="amount" HeaderText="Giá trị dự kiến (VNĐ)" DataFormatString="{0:N0}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
