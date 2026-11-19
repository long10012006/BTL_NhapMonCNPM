<%@ Page Title="Lịch của tôi" Language="C#" AutoEventWireup="true" CodeBehind="LichCuaToi.aspx.cs" Inherits="SanLichWeb.pages.LichCuaToi.LichCuaToi" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Lịch đặt của tôi
            </h1>
            <p class="subtitle">
                Lọc theo ngày sử dụng và trạng thái. Mỗi lượt có mã riêng và lịch sử thay đổi.
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
    <asp:Label ID="lblSoLuong" runat="server" CssClass="notice" />
    <div class="table-wrap">
        <asp:GridView
            ID="gvDanhSach"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="data-table"
            EmptyDataText="Không có dữ liệu."
            GridLines="None"
            OnRowCommand="gvDanhSach_RowCommand"
            AllowPaging="true"
            PageSize="12"
            OnPageIndexChanging="gvDanhSach_PageIndexChanging" >
            <Columns>
                <asp:BoundField DataField="booking_code" HeaderText="Mã lượt" />
                <asp:BoundField DataField="court_name" HeaderText="Sân" />
                <asp:BoundField DataField="play_date" HeaderText="Ngày" />
                <asp:BoundField DataField="start_time" HeaderText="Bắt đầu" />
                <asp:BoundField DataField="end_time" HeaderText="Kết thúc" />
                <asp:BoundField DataField="contact_name" HeaderText="Khách hàng" />
                <asp:TemplateField HeaderText="Trạng thái">
                    <ItemTemplate>
                        <%# MaHoaHtml(TrangThai(Eval("status"))) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền">
                    <ItemTemplate>
                        <%# MaHoaHtml(Tien(Eval("total_amount"))) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Chi tiết / Xử lý" CommandName="Chon" CommandArgument='<%# Eval("booking_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel ID="pnlChiTiet" runat="server" Visible="false" CssClass="card">
        <h2>
            Chi tiết lượt đặt
        </h2>
        <asp:Label ID="lblChiTiet" runat="server" />
        <div class="actions">
            <asp:Button
                ID="btnHuy"
                runat="server"
                Text="Hủy lượt này"
                CssClass="btn"
                OnClick="btnHuy_Click"
                OnClientClick="return confirm('Bạn muốn hủy lượt đặt này?');" />
            <asp:HyperLink ID="lnkDoi" runat="server" CssClass="btn secondary" Text="Đổi lịch" />
        </div>
        <h3>
            Lịch sử thay đổi
        </h3>
        <div class="table-wrap">
            <asp:GridView ID="gvLichSu" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="Không có dữ liệu." GridLines="None" >
                <Columns>
                    <asp:BoundField DataField="action" HeaderText="Thao tác" />
                    <asp:BoundField DataField="court_name" HeaderText="Sân sau thay đổi" />
                    <asp:BoundField DataField="play_date" HeaderText="Ngày" />
                    <asp:BoundField DataField="start_time" HeaderText="Giờ bắt đầu" />
                    <asp:BoundField DataField="new_amount" HeaderText="Giá mới (VNĐ)" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="display_name" HeaderText="Người thực hiện" />
                    <asp:BoundField DataField="reason" HeaderText="Lý do" />
                    <asp:BoundField DataField="occurred_at_utc" HeaderText="Thời điểm (UTC)" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>
