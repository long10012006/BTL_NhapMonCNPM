<%@ Page Title="Mở khóa lịch" Language="C#" AutoEventWireup="true" CodeBehind="QLLich.aspx.cs" Inherits="SanLichWeb.pages.QLLich.QLLich" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Mở / khóa khung giờ
            </h1>
            <p class="subtitle">
                Khung đang có lượt Đã xác nhận không được khóa. Lịch mới mặc định mở; giờ chưa có giá không nhận đặt.
            </p>
        </div>
    </div>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <section class="card">
        <div class="filters">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtNgay" Text="Ngày" />
                <asp:TextBox ID="txtNgay" runat="server" TextMode="Date"  />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlSan" Text="Sân" />
                <asp:DropDownList ID="ddlSan" runat="server" >
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnXem" runat="server" Text="Xem lịch" CssClass="btn" OnClick="btnXem_Click"  />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtLyDo" Text="Lý do khi khóa" />
            <asp:TextBox ID="txtLyDo" runat="server" MaxLength="500" />
        </div>
    </section>
    <div class="table-wrap">
        <asp:GridView
            ID="gvLich"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="data-table"
            EmptyDataText="Không có dữ liệu."
            GridLines="None"
            OnRowCommand="gvLich_RowCommand" >
            <Columns>
                <asp:BoundField DataField="court_name" HeaderText="Sân" />
                <asp:BoundField DataField="start_time" HeaderText="Bắt đầu" />
                <asp:BoundField DataField="end_time" HeaderText="Kết thúc" />
                <asp:TemplateField HeaderText="Đang mở">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("is_open")) == 1 ? "Mở" : "Khóa" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đã đặt">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("busy")) == 1 ? "Đã đặt" : "Trống" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="lock_reason" HeaderText="Lý do khóa" />
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Mo" CommandArgument='<%# Eval("schedule_id") %>' Text="Mở" />
                        ·
                        <asp:LinkButton runat="server" CommandName="Khoa" CommandArgument='<%# Eval("schedule_id") %>' Text="Khóa" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
