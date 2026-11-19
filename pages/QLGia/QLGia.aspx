<%@ Page Title="Quản lý giá" Language="C#" AutoEventWireup="true" CodeBehind="QLGia.aspx.cs" Inherits="SanLichWeb.pages.QLGia.QLGia" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Quản lý bảng giá
            </h1>
            <p class="subtitle">
                Đơn giá VNĐ/giờ; tổng tiền mỗi lượt = đơn giá × 90/60. Giá đã chốt trên lượt cũ được giữ nguyên.
            </p>
        </div>
    </div>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <section class="card">
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlSan" Text="Sân" />
            <asp:DropDownList ID="ddlSan" runat="server" >
            </asp:DropDownList>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlGio" Text="Khung giờ" />
            <asp:DropDownList ID="ddlGio" runat="server" >
            </asp:DropDownList>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtTuNgay" Text="Hiệu lực từ ngày (hôm nay hoặc tương lai)" />
            <asp:TextBox ID="txtTuNgay" runat="server" TextMode="Date"  />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtGia" Text="Đơn giá mỗi giờ (VNĐ)" />
            <asp:TextBox ID="txtGia" runat="server" TextMode="Number" min="1" max="100000000" step="1" />
        </div>
        <asp:Button ID="btnLuu" runat="server" Text="Áp dụng giá mới" CssClass="btn" OnClick="btnLuu_Click"  />
        <p class="notice">
            Mức giá cũ tự kết thúc vào ngày trước ngày hiệu lực mới. Chọn ngày sau mức giá cuối đã khai báo; không tạo khoảng giá chồng nhau.
        </p>
    </section>
    <div class="table-wrap">
        <asp:GridView
            ID="gvGia"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="data-table"
            EmptyDataText="Không có dữ liệu."
            GridLines="None"
            AllowPaging="true"
            PageSize="15"
            OnPageIndexChanging="gvGia_PageIndexChanging" >
            <Columns>
                <asp:BoundField DataField="court_name" HeaderText="Sân" />
                <asp:BoundField DataField="label" HeaderText="Khung giờ" />
                <asp:BoundField DataField="hourly_rate" HeaderText="Đơn giá/giờ (VNĐ)" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="valid_from" HeaderText="Từ ngày" />
                <asp:BoundField DataField="valid_to" HeaderText="Đến ngày (trống = tiếp tục)" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
